using UnityEngine;

[CreateAssetMenu(fileName = "New Country", menuName = "Country Data")]
public class CountrySO : ScriptableObject
{
    public string countryName;
    public int countryArmyPopulation;
}
