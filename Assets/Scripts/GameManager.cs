using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Country Data")]
    [SerializeField] private List<CountrySO> allCountrySOs;
    
    [Header("Battle Settings")]
    [SerializeField] private List<string> inBattleStatus;
    [SerializeField] private int armyGainOnWin = 50;
    [SerializeField] private int armyLossOnDefeat = 100;
    [SerializeField] private int maxLosses = 3;
    private int lossCount = 0;
    public List<string> InBattleStatus => inBattleStatus;
    public int LossCount => lossCount;
    
    [Header("Loot Settings")]
    [SerializeField] private int foodLootAmount = 50;
    [SerializeField] private int goldLootAmount = 50;

    [Header("Difficulty Colors")]
    [SerializeField] private Color easyColor = new Color(0.4f, 0.8f, 0.4f);
    [SerializeField] private Color mediumColor = new Color(1f, 0.8f, 0.2f);
    [SerializeField] private Color hardColor = new Color(0.9f, 0.3f, 0.3f);
    [SerializeField] private Color playerCountryColor = new Color(0.2f, 0.8f, 0.2f, 1f);

    public Color EasyColor => easyColor;
    public Color MediumColor => mediumColor;
    public Color HardColor => hardColor;
    public Color PlayerCountryColor => playerCountryColor;

    private Country selectedCountry;
    private Country playerCountry;
    public Country PlayerCountry => playerCountry;
    private Country defeatedCountry;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SelectPlayerCountry();
    }

    public int CalculateHiddenScore(Country c)
    {
        float score = c.countryArmyPopulation * 1f + c.countryFood * 0.5f + c.countryGold * 0.75f;
        return Mathf.CeilToInt(score);
    }

    public int CalculateHiddenScoreFromSO(CountrySO so)
    {
        float score = so.countryArmyPopulation * 1f + so.countryFood * 0.5f + so.countryGold * 0.75f;
        return Mathf.CeilToInt(score);
    }

    public float CalculateWinRate(Country player, Country enemy)
    {
        int playerScore = CalculateHiddenScore(player);
        int enemyScore = CalculateHiddenScore(enemy);
        
        if (enemyScore == 0) return 0.90f;
        
        float ratio = (float)playerScore / enemyScore;
        
        if (ratio >= 0.9f)
        {
            return Random.Range(0.75f, 0.90f);
        }
        else if (ratio >= 0.75f)
        {
            return Random.Range(0.55f, 0.70f);
        }
        else if (ratio >= 0.5f)
        {
            return Random.Range(0.35f, 0.50f);
        }
        else
        {
            return Random.Range(0.10f, 0.25f);
        }
    }

    private void SelectPlayerCountry()
    {
        Country[] allCountries = FindObjectsOfType<Country>();
        
        List<Country> sortedCountries = allCountries
            .Where(c => c.countryData != null)
            .OrderBy(c => CalculateHiddenScoreFromSO(c.countryData))
            .ToList();
        
        int easyCount = Mathf.Min(3, sortedCountries.Count);
        if (easyCount > 0)
        {
            int randomIndex = Random.Range(0, easyCount);
            playerCountry = sortedCountries[randomIndex];
        }
        else
        {
            playerCountry = allCountries[Random.Range(0, allCountries.Length)];
        }
        
        playerCountry.SetAsPlayerCountry();

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowPlayerStatsPanel();
        }

        if (PlayerStatsDisplay.Instance != null)
        {
            PlayerStatsDisplay.Instance.UpdateDisplay(playerCountry);
        }
    }

    public void SelectCountry(Country country)
    {
        if (selectedCountry != null)
        {
            selectedCountry.ResetColor();
        }

        selectedCountry = country;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowInfoPanel();
        }

        if (CountryInfoDisplay.Instance != null)
        {
            CountryInfoDisplay.Instance.UpdateDisplay(country);
        }
    }

    public void DeselectCountry()
    {
        if (selectedCountry != null)
        {
            selectedCountry.ResetColor();
            selectedCountry = null;
        }

        UIManager.Instance.HideInfoPanel();
    }

    public bool AttackCountry()
    {
        if (selectedCountry == null || playerCountry == null)
        {
            return false;
        }

        float winRate = CalculateWinRate(playerCountry, selectedCountry);
        bool playerWon = Random.value < winRate;

        if (playerWon)
        {
            playerCountry.countryArmyPopulation += armyGainOnWin;
        }
        else
        {
            lossCount++;
            playerCountry.countryArmyPopulation -= armyLossOnDefeat;
            if (playerCountry.countryArmyPopulation < 0)
            {
                playerCountry.countryArmyPopulation = 0;
            }
        }
        
        if (PlayerStatsDisplay.Instance != null)
        {
            PlayerStatsDisplay.Instance.UpdateDisplay(playerCountry);
        }

        return playerWon;
    }

    public bool IsGameOver()
    {
        return lossCount >= maxLosses;
    }

    public void LootFood()
    {
        if (selectedCountry != null && playerCountry != null)
        {
            playerCountry.countryFood += foodLootAmount;
            selectedCountry.countryFood -= foodLootAmount;
            
            if (PlayerStatsDisplay.Instance != null)
            {
                PlayerStatsDisplay.Instance.UpdateDisplay(playerCountry);
            }
        }
    }

    public void LootGold()
    {
        if (selectedCountry != null && playerCountry != null)
        {
            playerCountry.countryGold += goldLootAmount;
            selectedCountry.countryGold -= goldLootAmount;

            if (PlayerStatsDisplay.Instance != null)
            {
                PlayerStatsDisplay.Instance.UpdateDisplay(playerCountry);
            }
        }
    }

    public void SetDefeatedCountry()
    {
        defeatedCountry = selectedCountry;
    }

    public void ConquerDefeatedCountry()
    {
        if (defeatedCountry != null)
        {
            defeatedCountry.SetAsConqueredCountry();
            
            if (playerCountry != null)
            {
                defeatedCountry.UpdateMapFlag(playerCountry.countryFlag);
            }
            
            defeatedCountry = null;

            CheckWinCondition();
        }
    }

    private void CheckWinCondition()
    {
        Country[] allCountries = FindObjectsOfType<Country>();
        bool allConquered = true;

        foreach (Country country in allCountries)
        {
            if (!country.isPlayerCountry)
            {
                allConquered = false;
                break;
            }
        }

        if (allConquered)
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowGameWonPanel();
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Randomize All Country Stats")]
    public void RandomizeAllCountryStats()
    {
        if (allCountrySOs == null || allCountrySOs.Count != 11)
        {
            Debug.LogWarning("Need exactly 11 CountrySOs in the list!");
            return;
        }

        List<CountrySO> shuffled = allCountrySOs.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < 3; i++)
        {
            shuffled[i].countryArmyPopulation = Random.Range(150, 301);
            shuffled[i].countryFood = Random.Range(50, 150);
            shuffled[i].countryGold = Random.Range(30, 100);
            UnityEditor.EditorUtility.SetDirty(shuffled[i]);
        }

        for (int i = 3; i < 7; i++)
        {
            shuffled[i].countryArmyPopulation = Random.Range(400, 601);
            shuffled[i].countryFood = Random.Range(100, 250);
            shuffled[i].countryGold = Random.Range(80, 180);
            UnityEditor.EditorUtility.SetDirty(shuffled[i]);
        }

        for (int i = 7; i < 11; i++)
        {
            shuffled[i].countryArmyPopulation = Random.Range(700, 1001);
            shuffled[i].countryFood = Random.Range(200, 400);
            shuffled[i].countryGold = Random.Range(150, 300);
            UnityEditor.EditorUtility.SetDirty(shuffled[i]);
        }

        UnityEditor.AssetDatabase.SaveAssets();
        Debug.Log("Country stats randomized! 3 Easy, 4 Medium, 4 Hard");
    }
#endif
}