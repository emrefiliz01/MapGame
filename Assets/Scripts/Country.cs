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
         if (!isPlayerCountry)                 
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
        defaultColor = Color.green;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.green;
    }

    private void OnMouseDown()
    {
        if (isPlayerCountry) return;

        GameManager.Instance.SelectCountry(this);
    }
}
