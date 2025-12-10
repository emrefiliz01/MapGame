using UnityEngine;

public class Country : MonoBehaviour
{
    public string countryName;
    public int countryPopulation;
    public SpriteRenderer spriteRenderer;

    private Color defaultColor;

    void Start()
    {
         spriteRenderer = GetComponent<SpriteRenderer>();
         defaultColor = spriteRenderer.color;
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void ResetColor()
    {
        spriteRenderer.color = defaultColor;
    }

    private void OnMouseDown()
    {
        UIManager.Instance.ShowCountryInfo(this);
    }
}
