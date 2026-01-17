using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private List<string> inBattleStatus;
    private Country selectedCountry;
    private Country playerCountry;
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

    private void SelectPlayerCountry()
    {
        Country[] allCountries = FindObjectsOfType<Country>();

        int randomIndex = Random.Range(0, allCountries.Length);
        playerCountry = allCountries[randomIndex];
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
        selectedCountry.SetColor(Color.white);

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

        int playerArmy = playerCountry.countryArmyPopulation;
        int enemyArmy = selectedCountry.countryArmyPopulation;

        return playerArmy > enemyArmy;
    }

    [Header("Loot Settings")]
    [SerializeField] private int foodLootAmount = 50;
    [SerializeField] private int goldLootAmount = 50;

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
}