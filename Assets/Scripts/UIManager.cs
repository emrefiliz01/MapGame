using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject lostBattlePanel;
    [SerializeField] private GameObject gameWonPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject ourCountryInfoPanel;

    [Header("Info Panel Tabs")]
    [SerializeField] private GameObject attackTab;
    [SerializeField] private GameObject battleTab;
    [SerializeField] private Image battleFillImage;
    [SerializeField] private TMP_Text battleStatusText;
    [SerializeField] private float battleDuration = 10f;

    public bool isInfoPanelOpened = false;
    private bool isBattleInProgress = false;
    private bool battleResultShown = false;
    private Coroutine battleCoroutine;
    private Coroutine statusCoroutine;

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
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClickSound();
        GameManager.Instance.LootFood();
        GameManager.Instance.ConquerDefeatedCountry();
        isInfoPanelOpened = false;
        victoryPanel.SetActive(false);
        if (SoundManager.Instance != null) SoundManager.Instance.PlayBackgroundMusic();
    }

    private void OnGoldButtonClicked()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClickSound();
        GameManager.Instance.LootGold();
        GameManager.Instance.ConquerDefeatedCountry();
        isInfoPanelOpened = false;
        victoryPanel.SetActive(false);
        if (SoundManager.Instance != null) SoundManager.Instance.PlayBackgroundMusic();
    }

    public void StartGame()
    {
        if (mainMenu != null) mainMenu.SetActive(false);
        if (gamePanel != null) gamePanel.SetActive(true);
    }

    public void ShowInfoPanel()
    {
        isInfoPanelOpened = true;
        
        if (battleCoroutine != null)
        {
            StopCoroutine(battleCoroutine);
            battleCoroutine = null;
        }
        if (statusCoroutine != null)
        {
            StopCoroutine(statusCoroutine);
            statusCoroutine = null;
        }
        isBattleInProgress = false;
        
        if (attackTab != null) attackTab.SetActive(true);
        if (battleTab != null) battleTab.SetActive(false);
        if (battleFillImage != null) battleFillImage.fillAmount = 0f;
        if (battleStatusText != null) battleStatusText.text = "";
        
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

    public void ShowGameWonPanel()
    {
        isInfoPanelOpened = true;
        if (gameWonPanel != null)
        {
            gameWonPanel.SetActive(true);
        }
    }

    public void ClosePanel()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClickSound();
        GameManager.Instance.DeselectCountry();
    }

    public void StartBattle()
    {
        if (isBattleInProgress) return;
        
        battleResultShown = false;
        
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayBattleMusic();
        }
        
        if (battleCoroutine != null)
        {
            StopCoroutine(battleCoroutine);
            battleCoroutine = null;
        }
        
        if (statusCoroutine != null)
        {
            StopCoroutine(statusCoroutine);
            statusCoroutine = null;
        }
        
        if (battleFillImage != null) battleFillImage.fillAmount = 0f;
        
        if (attackTab != null) attackTab.SetActive(false);
        if (battleTab != null) battleTab.SetActive(true);
        
        battleCoroutine = StartCoroutine(BattleFillRoutine());
        statusCoroutine = StartCoroutine(BattleStatusRoutine());
    }

    private IEnumerator BattleStatusRoutine()
    {
        var statusList = GameManager.Instance.InBattleStatus;
        if (statusList == null || statusList.Count == 0 || battleStatusText == null) yield break;
        
        while (isBattleInProgress)
        {
            int randomIndex = Random.Range(0, statusList.Count);
            battleStatusText.text = statusList[randomIndex];
            yield return new WaitForSeconds(2f);
        }
        
        battleStatusText.text = "";
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
        
        if (battleResultShown)
        {
            isBattleInProgress = false;
            battleCoroutine = null;
            yield break;
        }
        battleResultShown = true;
        
        if (battleFillImage != null) battleFillImage.fillAmount = 1f;
        
        bool playerWon = GameManager.Instance.AttackCountry();
        isBattleInProgress = false;
        battleCoroutine = null;
        
        if (playerWon)
        {
            GameManager.Instance.SetDefeatedCountry();
        }
        
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
            if (SoundManager.Instance != null) SoundManager.Instance.PlayWinMusic();
            victoryPanel.SetActive(true);
        }
        else
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayLoseMusic();
            
            if (GameManager.Instance.IsGameOver())
            {
                gameOverPanel.SetActive(true);
            }
            else
            {
                if (lostBattlePanel != null) lostBattlePanel.SetActive(true);
            }
        }
    }

    public void OnExitButtonClicked()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void HideLostBattlePanel()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClickSound();
        if (SoundManager.Instance != null) SoundManager.Instance.PlayBackgroundMusic();
        if (lostBattlePanel != null) lostBattlePanel.SetActive(false);
        isInfoPanelOpened = false;
    }
}
