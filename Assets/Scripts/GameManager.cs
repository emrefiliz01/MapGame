using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Country selectedCountry;
    private Country playerCountry;

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

    public void AttackCountry()
    {
        if (selectedCountry == null || playerCountry == null)
        {
            return;
        }

        int playerArmy = playerCountry.countryData.countryArmyPopulation;
        int enemyArmy = selectedCountry.countryData.countryArmyPopulation;

        UIManager.Instance.HideInfoPanel();

        if (playerArmy > enemyArmy)
        {
            UIManager.Instance.ShowVictoryPanel();
        }
        else
        {
            UIManager.Instance.ShowGameOverPanel();
        }
    }

    [Header("Loot Settings")]
    [SerializeField] private int foodLootAmount = 50;
    [SerializeField] private int goldLootAmount = 50;

    public void LootFood()
    {
        if (selectedCountry != null && playerCountry != null)
        {
            playerCountry.countryData.countryFood += foodLootAmount;
            selectedCountry.countryData.countryFood -= foodLootAmount;
            
            if (PlayerStatsDisplay.Instance != null)
            {
                PlayerStatsDisplay.Instance.UpdateDisplay(playerCountry);
            }
        }
        DeselectCountry();
    }

    public void LootGold()
    {
        if (selectedCountry != null && playerCountry != null)
        {
            playerCountry.countryData.countryGold += goldLootAmount;
            selectedCountry.countryData.countryGold -= goldLootAmount;

            if (PlayerStatsDisplay.Instance != null)
            {
                PlayerStatsDisplay.Instance.UpdateDisplay(playerCountry);
            }
        }
        DeselectCountry();
    }
}