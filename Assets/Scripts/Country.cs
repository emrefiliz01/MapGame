using UnityEngine;

public class Country : MonoBehaviour
{
    public string countryName;
    public int countryPopulation;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
         spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        UIManager.Instance.ShowCountryInfo(this);
    }
}
