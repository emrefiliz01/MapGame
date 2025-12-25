using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text countryNameText;
    [SerializeField] private TMP_Text countryPopulationText;
    [SerializeField] private TMP_Text countryFoodText;

    [Header("Our Country Info")]
    [SerializeField] private GameObject ourCountryInfoPanel;
    [SerializeField] private TMP_Text ourCountryArmyText;
    [SerializeField] private TMP_Text ourCountryFoodText;

    private Country selectedCountry;
    private RectTransform infoPanelRect;

    private void Awake()
    {
        Instance = this;
        if (infoPanel != null)
        {
            infoPanelRect = infoPanel.GetComponent<RectTransform>();
        }
    }

    public void UpdatePlayerCountryUI(Country country)
    {
        ourCountryInfoPanel.SetActive(true);
        ourCountryArmyText.text = "Army: " + country.countryData.countryArmyPopulation;
        ourCountryFoodText.text = "Food: " + country.countryData.countryFood;
    }

    public void ShowCountryInfo(Country country)
    {
        if (selectedCountry != null)
        {
            selectedCountry.ResetColor();
        }

        selectedCountry = country;
        selectedCountry.SetColor(Color.white);

        if (infoPanelRect != null)
        {
            if (!infoPanel.activeSelf)
            {
                infoPanelRect.DOKill();
                infoPanelRect.localScale = Vector3.zero;
                infoPanel.SetActive(true);
                infoPanelRect.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            }
        }
        else
        {
             infoPanel.SetActive(true);
        }

        if (country.countryData != null)
        {
            countryNameText.text = country.countryData.countryName;
            countryPopulationText.text = country.countryData.countryArmyPopulation.ToString();
            countryFoodText.text = country.countryData.countryFood.ToString();
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
