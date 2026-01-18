using UnityEngine;

public class Country : MonoBehaviour
{
    public CountrySO countryData;
    public SpriteRenderer spriteRenderer;

    [Header("Flag Display")]
    [SerializeField] private GameObject flagContainer;
    [SerializeField] private SpriteRenderer mapFlagRenderer;

    public string countryName;
    public Sprite countryFlag;
    public int countryArmyPopulation;
    public int countryFood;
    public int countryGold;

    private Color defaultColor;
    private Color hoverColor;
    public bool isPlayerCountry = false;
    private bool isHovering = false;

    void Start()
    {
         spriteRenderer = GetComponent<SpriteRenderer>();
         
         if (countryData != null)
         {
             countryName = countryData.countryName;
             countryFlag = countryData.countryFlag;
             countryArmyPopulation = countryData.countryArmyPopulation;
             countryFood = countryData.countryFood;
             countryGold = countryData.countryGold;
             
             if (mapFlagRenderer != null)
             {
                 mapFlagRenderer.sprite = countryFlag;
             }
         }
         
         if (flagContainer != null)
         {
             flagContainer.SetActive(isPlayerCountry);
         }
         
         if (!isPlayerCountry)                 
         {
            AssignDifficultyColor();
         }
    }

    private void AssignDifficultyColor()
    {
        int difficulty = GetDifficultyLevel();
        
        if (difficulty == 0)
            defaultColor = GameManager.Instance.EasyColor;
        else if (difficulty == 1)
            defaultColor = GameManager.Instance.MediumColor;
        else
            defaultColor = GameManager.Instance.HardColor;
        
        hoverColor = defaultColor * 1.3f;
        hoverColor.a = 1f;
        spriteRenderer.color = defaultColor;
    }

    public int GetDifficultyLevel()
    {
        if (countryArmyPopulation <= 300)
            return 0;
        else if (countryArmyPopulation <= 600)
            return 1;
        else
            return 2;
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
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        defaultColor = GameManager.Instance.PlayerCountryColor;
        spriteRenderer.color = defaultColor;
        
        if (flagContainer != null)
        {
            flagContainer.SetActive(true);
        }
    }

    public void SetAsConqueredCountry()
    {
        isPlayerCountry = true;
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        defaultColor = GameManager.Instance.PlayerCountryColor;
        spriteRenderer.color = defaultColor;
        
        if (flagContainer != null)
        {
            flagContainer.SetActive(true);
        }
    }

    public void UpdateMapFlag(Sprite newFlag)
    {
        if (mapFlagRenderer != null)
        {
            mapFlagRenderer.sprite = newFlag;
        }
    }

    private void OnMouseEnter()
    {
        if (isPlayerCountry) return;
        if (UIManager.Instance != null && UIManager.Instance.isInfoPanelOpened) return;

        isHovering = true;
        spriteRenderer.color = hoverColor;
    }

    private void OnMouseExit()
    {
        if (isPlayerCountry) return;
        if (UIManager.Instance != null && UIManager.Instance.isInfoPanelOpened) return;

        isHovering = false;
        spriteRenderer.color = defaultColor;
    }

    private void OnMouseDown()
    {
        if (isPlayerCountry) return;
        if (UIManager.Instance.isInfoPanelOpened) return;
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

        if (SoundManager.Instance != null) SoundManager.Instance.PlayClickSound();
        GameManager.Instance.SelectCountry(this);
    }
}
