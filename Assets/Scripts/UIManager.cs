using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TMP_Text countryNameText;
    [SerializeField] private TMP_Text countryPopulationText;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowCountryInfo(Country country)
    {
        infoPanel.SetActive(true);
        countryNameText.text = country.countryName;
        countryPopulationText.text = "Population: " + country.countryPopulation.ToString();
    }

    public void ClosePanel()
    {
        infoPanel.SetActive(false);
    }
}
