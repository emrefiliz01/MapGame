using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
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
        Country playerCountry = allCountries[randomIndex];
        playerCountry.SetAsPlayerCountry();

        UIManager.Instance.UpdatePlayerCountryUI(playerCountry);
    }
}
