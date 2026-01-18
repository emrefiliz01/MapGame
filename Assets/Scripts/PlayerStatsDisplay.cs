using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsDisplay : MonoBehaviour
{
    public static PlayerStatsDisplay Instance;

    [SerializeField] private TMP_Text ourCountryNameText;
    [SerializeField] private TMP_Text ourCountryArmyText;
    [SerializeField] private TMP_Text ourCountryFoodText;
    [SerializeField] private TMP_Text ourCountryGoldText;
    [SerializeField] private Image ourCountryFlagImage;
    [SerializeField] private float initDelay = 0.1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Invoke(nameof(DelayedInit), initDelay);
    }

    private void DelayedInit()
    {
        if (GameManager.Instance != null && GameManager.Instance.PlayerCountry != null)
        {
            UpdateDisplay(GameManager.Instance.PlayerCountry);
        }
    }

    public void UpdateDisplay(Country country)
    {
        ourCountryNameText.text = country.countryName;
        ourCountryArmyText.text = "Army: " + country.countryArmyPopulation;
        ourCountryFoodText.text = "Food: " + country.countryFood;
        ourCountryGoldText.text = "Gold: " + country.countryGold;
        
        if (ourCountryFlagImage != null && country.countryFlag != null)
        {
            ourCountryFlagImage.sprite = country.countryFlag;
        }
    }
}
