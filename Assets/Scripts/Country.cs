using UnityEngine;

public class Country : MonoBehaviour
{
    public CountrySO countryData;
    public SpriteRenderer spriteRenderer;

    private Color defaultColor;
    private Color hoverColor;
    private bool isPlayerCountry = false;
    private bool isHovering = false;

    void Start()
    {
         spriteRenderer = GetComponent<SpriteRenderer>();
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
