using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject ourCountryInfoPanel;

    [Header("Info Panel Tabs")]
    [SerializeField] private GameObject attackTab;
    [SerializeField] private GameObject battleTab;
    [SerializeField] private Image battleFillImage;
    [SerializeField] private float battleDuration = 10f;

    public bool isInfoPanelOpened = false;
    private bool isBattleInProgress = false;

    [Header("Victory Panel Buttons")]
    public Button foodButton;
    public Button goldButton;

    private RectTransform infoPanelRect;

    private void Awake()
    {
        Instance = this;
        if (infoPanel != null)
        {
            infoPanelRect = infoPanel.GetComponent<RectTransform>();
        }
    }

    private void Start()
    {
        if (foodButton != null)
        {
            foodButton.onClick.AddListener(OnFoodButtonClicked);
        }

        if (goldButton != null)
        {
            goldButton.onClick.AddListener(OnGoldButtonClicked);
        }
    }

    private void OnFoodButtonClicked()
    {
        GameManager.Instance.LootFood();
        victoryPanel.SetActive(false);
    }

    private void OnGoldButtonClicked()
    {
        GameManager.Instance.LootGold();
        victoryPanel.SetActive(false);
    }

    public void ShowInfoPanel()
    {
        isInfoPanelOpened = true;
        
        if (attackTab != null) attackTab.SetActive(true);
        if (battleTab != null) battleTab.SetActive(false);
        if (battleFillImage != null) battleFillImage.fillAmount = 0f;
        
        if (infoPanelRect != null)
        {
            if (!infoPanel.activeSelf)
            {
                infoPanelRect.DOKill();
                infoPanelRect.localScale = Vector3.zero;
                infoPanel.SetActive(true);
                infoPanelRect.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            }
        }
        else
        {
            infoPanel.SetActive(true);
        }
    }

    public void HideInfoPanel()
    {
        isInfoPanelOpened = false;
        if (infoPanelRect != null)
        {
            infoPanelRect.DOKill();
            infoPanelRect.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                infoPanel.SetActive(false);
            });
        }
        else
        {
            infoPanel.SetActive(false);
        }
    }

    public void ShowPlayerStatsPanel()
    {
        ourCountryInfoPanel.SetActive(true);
    }

    public void ShowGameOverPanel()
    {
        isInfoPanelOpened = true;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void ShowVictoryPanel()
    {
        isInfoPanelOpened = true;
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }

    public void ClosePanel()
    {
        GameManager.Instance.DeselectCountry();
    }

    public void StartBattle()
    {
        if (isBattleInProgress) return;
        
        if (attackTab != null) attackTab.SetActive(false);
        if (battleTab != null) battleTab.SetActive(true);
        
        StartCoroutine(BattleFillRoutine());
    }

    private IEnumerator BattleFillRoutine()
    {
        isBattleInProgress = true;
        float elapsed = 0f;
        
        while (elapsed < battleDuration)
        {
            elapsed += Time.deltaTime;
            if (battleFillImage != null)
            {
                battleFillImage.fillAmount = elapsed / battleDuration;
            }
            yield return null;
        }
        
        if (battleFillImage != null) battleFillImage.fillAmount = 1f;
        
        bool playerWon = GameManager.Instance.AttackCountry();
        isBattleInProgress = false;
        
        isInfoPanelOpened = false;
        if (infoPanelRect != null)
        {
            infoPanelRect.DOKill();
            infoPanelRect.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                infoPanel.SetActive(false);
            });
        }
        else
        {
            infoPanel.SetActive(false);
        }
        
        if (playerWon)
        {
            victoryPanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(true);
        }
    }
}
