using UnityEngine;

public class Country : MonoBehaviour
{
    public CountrySO countryData;
    public SpriteRenderer spriteRenderer;

    // Runtime data (copies of SO data that we can modify without affecting the asset)
    public string countryName;
    public int countryArmyPopulation;
    public int countryFood;
    public int countryGold;

    private Color defaultColor;
    private Color hoverColor;
    private bool isPlayerCountry = false;
    private bool isHovering = false;

    void Start()
    {
         spriteRenderer = GetComponent<SpriteRenderer>();
         
         // Copy data from ScriptableObject to runtime variables
         if (countryData != null)
         {
             countryName = countryData.countryName;
             countryArmyPopulation = countryData.countryArmyPopulation;
             countryFood = countryData.countryFood;
             countryGold = countryData.countryGold;
         }
         
         if (!isPlayerCountry)                 
         {
            defaultColor = spriteRenderer.color;
            hoverColor = defaultColor * 1.3f;
            hoverColor.a = defaultColor.a;
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

    public void SetAsConqueredCountry()
    {
        isPlayerCountry = true;
        defaultColor = Color.green;
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.color = Color.green;
    }

    private void OnMouseEnter()
    {
        if (isPlayerCountry) return;

        isHovering = true;
        spriteRenderer.color = hoverColor;
    }

    private void OnMouseExit()
    {
        if (isPlayerCountry) return;

        isHovering = false;
        spriteRenderer.color = defaultColor;
    }

    private void OnMouseDown()
    {
        if (isPlayerCountry) return;
        if (UIManager.Instance.isInfoPanelOpened) return;

        GameManager.Instance.SelectCountry(this);
    }
}
