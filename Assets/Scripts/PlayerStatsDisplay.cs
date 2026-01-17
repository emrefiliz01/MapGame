using UnityEngine;
using TMPro;

public class PlayerStatsDisplay : MonoBehaviour
{
    public static PlayerStatsDisplay Instance;

    [SerializeField] private TMP_Text ourCountryNameText;
    [SerializeField] private TMP_Text ourCountryArmyText;
    [SerializeField] private TMP_Text ourCountryFoodText;
    [SerializeField] private TMP_Text ourCountryGoldText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateDisplay(Country country)
    {
        ourCountryNameText.text = country.countryName;
        ourCountryArmyText.text = "Army: " + country.countryArmyPopulation;
        ourCountryFoodText.text = "Food: " + country.countryFood;
        ourCountryGoldText.text = "Gold: " + country.countryGold;
    }
}
