using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SymbolManger : MonoBehaviour
{
    // Constants for better readability
    private const int BASIC_SPRITE_INDEX = 0;
    private const int DELETE_SPRITE_INDEX = 1;
    private const int ORIGIN_SPRITE_INDEX = 2;
    
    // Path configuration
    public static string symbolsPath = "Assets/Symbols";
    public static int numberOfSymbolSets = 6;
    
    // Sprite storage
    public static List<List<Sprite>> spriteSetList = new List<List<Sprite>>();
    
    // UI References
    public Image topOrigin;
    public Image topBasic;
    public Image botOrigin;
    public Image botBasic;
    
    // Current indices
    public int indexTop = 0;
    public int indexBot = 1;
    
    // Private sprite cache
    private Sprite deleteTop { get; set; }
    private Sprite deleteBot { get; set; }
    
    // Singleton pattern
    public static SymbolManger Instance { get; private set; }

    private void Awake()
    {
        // Singleton implementation
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        LoadSprites();
    }

    private void Start()
    {
        InitializeDisplay();
    }

    /// <summary>
    /// Loads all sprite sets from Resources folder
    /// </summary>
    public static void LoadSprites()
    {
        spriteSetList.Clear(); // Clear existing sprites
        
        for(int i = 0; i < numberOfSymbolSets; i++)
        {   
            Sprite[] sprites = Resources.LoadAll<Sprite>($"Symbols/set{i}");
            if(sprites != null && sprites.Length > 0)
            {
                spriteSetList.Add(new List<Sprite>(sprites));
            }
            else
            {
                Debug.LogWarning($"Failed to load sprites for set {i}");
            }
        }
    }
    
    /// <summary>
    /// Initializes the display with default sprites
    /// </summary>
    private void InitializeDisplay()
    {
        if(!ValidateSpriteIndices()) return;
        
        deleteTop = spriteSetList[indexTop][DELETE_SPRITE_INDEX];
        deleteBot = spriteSetList[indexBot][DELETE_SPRITE_INDEX];

        UpdateTopDisplay();
        UpdateBottomDisplay();
    }
    
    /// <summary>
    /// Validates current sprite indices
    /// </summary>
    private bool ValidateSpriteIndices()
    {
        if(spriteSetList.Count == 0)
        {
            Debug.LogError("No sprite sets loaded");
            return false;
        }
        
        indexTop = Mathf.Clamp(indexTop, 0, spriteSetList.Count - 1);
        indexBot = Mathf.Clamp(indexBot, 0, spriteSetList.Count - 1);
        
        // Ensure top and bottom don't show the same set
        if(indexTop == indexBot)
        {
            indexBot = (indexBot + 1) % spriteSetList.Count;
        }
        
        return true;
    }
    
    /// <summary>
    /// Updates top display with current sprites
    /// </summary>
    private void UpdateTopDisplay()
    {
        if(!ValidateSpriteIndices()) return;
        
        topOrigin.sprite = spriteSetList[indexTop][ORIGIN_SPRITE_INDEX];
        topBasic.sprite = spriteSetList[indexTop][BASIC_SPRITE_INDEX];
        deleteTop = spriteSetList[indexTop][DELETE_SPRITE_INDEX];
    }
    
    /// <summary>
    /// Updates bottom display with current sprites
    /// </summary>
    private void UpdateBottomDisplay()
    {
        if(!ValidateSpriteIndices()) return;
        
        botOrigin.sprite = spriteSetList[indexBot][ORIGIN_SPRITE_INDEX];
        botBasic.sprite = spriteSetList[indexBot][BASIC_SPRITE_INDEX];
        deleteBot = spriteSetList[indexBot][DELETE_SPRITE_INDEX];
    }
    
    /// <summary>
    /// Modular index increment with collision avoidance
    /// </summary>
    private int IncrementIndex(int currentIndex, int otherIndex)
    {
        int newIndex = (currentIndex + 1) % spriteSetList.Count;
        
        // Skip if it would match the other display
        if(newIndex == otherIndex)
        {
            newIndex = (newIndex + 1) % spriteSetList.Count;
        }
        
        return newIndex;
    }
    
    /// <summary>
    /// Modular index decrement with collision avoidance
    /// </summary>
    private int DecrementIndex(int currentIndex, int otherIndex)
    {
        int newIndex = currentIndex - 1;
        if(newIndex < 0) newIndex = spriteSetList.Count - 1;
        
        // Skip if it would match the other display
        if(newIndex == otherIndex)
        {
            newIndex = newIndex - 1;
            if(newIndex < 0) newIndex = spriteSetList.Count - 1;
        }
        
        return newIndex;
    }

    // Public navigation methods
    
    public void NextSymbolSetTop()
    {
        indexTop = IncrementIndex(indexTop, indexBot);
        UpdateTopDisplay();
    }

    public void NextSymbolSetBot()
    {
        indexBot = IncrementIndex(indexBot, indexTop);
        UpdateBottomDisplay();
    }

    public void PreviousSymbolSetTop()
    {
        indexTop = DecrementIndex(indexTop, indexBot);
        UpdateTopDisplay();
    }

    public void PreviousSymbolSetBot()
    {
        indexBot = DecrementIndex(indexBot, indexTop);
        UpdateBottomDisplay();
    }
}