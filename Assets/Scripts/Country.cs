using UnityEngine;

public class Country : MonoBehaviour
{
    public CountrySO countryData;
    public SpriteRenderer spriteRenderer;

    private Color defaultColor;
    private bool isPlayerCountry = false;

    void Start()
    {
         spriteRenderer = GetComponent<SpriteRenderer>();
         if (!isPlayerCountry) // Only set default if it hasn't been overridden by SetAsPlayerCountry potentially running before Start? 
                               // Actually Start runs after Awake. SetAsPlayerCountry is called in GameManager Start.
                               // Best to just set defaultColor here, and if SetAsPlayerCountry is called later, it overwrites it.
         {
            defaultColor = spriteRenderer.color;
         }
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void ResetColor()
    {
        spriteRenderer.color = defaultColor;
    }

    public void SetAsPlayerCountry()
    {
        isPlayerCountry = true;
        defaultColor = Color.green; // Update default color so ResetColor works correctly
        spriteRenderer = GetComponent<SpriteRenderer>(); // Ensure component is retrieved if this is called early
        spriteRenderer.color = Color.green;
    }

    private void OnMouseDown()
    {
        if (isPlayerCountry) return;

        UIManager.Instance.ShowCountryInfo(this);
    }
}
