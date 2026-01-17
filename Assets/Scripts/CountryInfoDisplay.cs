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
        countryNameText.text = country.countryName;
        countryPopulationText.text = country.countryArmyPopulation.ToString();
        countryFoodText.text = country.countryFood.ToString();
    }
}
