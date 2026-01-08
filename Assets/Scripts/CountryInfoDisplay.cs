using UnityEngine;
using TMPro;

public class CountryInfoDisplay : MonoBehaviour
{
    public static CountryInfoDisplay Instance;

    [SerializeField] private TMP_Text countryNameText;
    [SerializeField] private TMP_Text countryPopulationText;
    [SerializeField] private TMP_Text countryFoodText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateDisplay(Country country)
    {
        countryNameText.text = country.countryData.countryName;
        countryPopulationText.text = country.countryData.countryArmyPopulation.ToString();
        countryFoodText.text = country.countryData.countryFood.ToString();
    }
}
