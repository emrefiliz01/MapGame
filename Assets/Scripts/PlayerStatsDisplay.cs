using UnityEngine;
using TMPro;

public class PlayerStatsDisplay : MonoBehaviour
{
    public static PlayerStatsDisplay Instance;

    [SerializeField] private TMP_Text ourCountryArmyText;
    [SerializeField] private TMP_Text ourCountryFoodText;
    [SerializeField] private TMP_Text ourCountryGoldText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateDisplay(Country country)
    {
        ourCountryArmyText.text = "Army: " + country.countryData.countryArmyPopulation;
        ourCountryFoodText.text = "Food: " + country.countryData.countryFood;
        ourCountryGoldText.text = "Gold: " + country.countryData.countryGold;
    }
}
