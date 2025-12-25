using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text countryNameText;
    [SerializeField] private TMP_Text countryPopulationText;

    private Country selectedCountry;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowCountryInfo(Country country)
    {
        if (selectedCountry != null)
        {
            selectedCountry.ResetColor();
        }

        selectedCountry = country;
        selectedCountry.SetColor(Color.white);

        infoPanel.SetActive(true);
        if (country.countryData != null)
        {
            countryNameText.text = country.countryData.countryName;
            countryPopulationText.text = "Army Population: " + country.countryData.countryArmyPopulation.ToString();
        }
    }

    public void ActivateGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void ClosePanel()
    {
        if (selectedCountry != null)
        {
            selectedCountry.ResetColor();
            selectedCountry = null;
        }
        infoPanel.SetActive(false);
    }
}
