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
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void ShowVictoryPanel()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }

    public void ClosePanel()
    {
        GameManager.Instance.DeselectCountry();
    }
}
