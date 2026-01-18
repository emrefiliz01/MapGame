using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountryInfoDisplay : MonoBehaviour
{
    public static CountryInfoDisplay Instance;

    [SerializeField] private TMP_Text countryNameText;
    [SerializeField] private TMP_Text countryPopulationText;
    [SerializeField] private TMP_Text countryFoodText;
    [SerializeField] private TMP_Text countryGoldText;
    [SerializeField] private TMP_Text winPercentageText;
    [SerializeField] private Image countryFlagImage;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateDisplay(Country country)
    {
        countryNameText.text = country.countryName;
        countryPopulationText.text = country.countryArmyPopulation.ToString();
        countryFoodText.text = country.countryFood.ToString();
        if (countryGoldText != null) countryGoldText.text = country.countryGold.ToString();
        countryFlagImage.sprite = country.countryFlag;

        if (winPercentageText != null && GameManager.Instance != null && GameManager.Instance.PlayerCountry != null)
        {
            float winRate = GameManager.Instance.CalculateWinRate(GameManager.Instance.PlayerCountry, country);
            int percentage = Mathf.RoundToInt(winRate * 100f);
            winPercentageText.text = percentage + "%";
        }
    }
}
