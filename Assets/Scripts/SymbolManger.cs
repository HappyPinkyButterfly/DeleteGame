using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;

public class SymbolManger : MonoBehaviour
{
    public static string symbolsPath = "Assets/Symbols";
    
    public static List<List<Sprite>> spriteSetList = new List<List<Sprite>>();

    public Image topOrigin;
    public Image topBasic;

    public Image botOrigin;
    public Image botBasic;

    public int indexTop = 0;
    public int indexBot = 1;

    public static int numberOfSymbolSets = 3;

    private Sprite deleteTop {get;set;}
    private Sprite deleteBot {get;set;}
    public static SymbolManger Instance { get; private set; }

    private void Update()
    {
     
    }

    private void Awake()
    {
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
        
        Debug.Log(spriteSetList.Count);
        Debug.Log("Zacetni top:  " + indexTop );
        Debug.Log("Zacetni bot:  " + indexBot );
        deleteTop = spriteSetList[indexTop][1];
        deleteBot = spriteSetList[indexBot][1];

        topOrigin.sprite = spriteSetList[indexTop][2];
        botOrigin.sprite = spriteSetList[indexBot][2];

        topBasic.sprite = spriteSetList[indexTop][0];
        botBasic.sprite = spriteSetList[indexBot][0];
        
    }

    public static void LoadSprites()
    {
        for(int i = 0; i < numberOfSymbolSets; i++)
        {   
            Sprite[] sprites = Resources.LoadAll<Sprite>("Symbols/set" + i);
            Debug.Log(sprites.Length);
            spriteSetList.Add(new List<Sprite>(sprites));
            Debug.Log(spriteSetList.Count);
        }
    }
    
    public void NextSymbolSetTop()
    {
        if(indexTop + 1 != indexBot && indexTop + 1 < spriteSetList.Count)
        {
            indexTop++;
        }
        else if(
        indexTop + 1 != indexBot &&
        indexTop + 1 == spriteSetList.Count &&
        indexTop + 1 - spriteSetList.Count != indexBot
        )
        {
           indexTop = indexTop + 1 - spriteSetList.Count; 
        }
        else if(
        indexTop + 1 != indexBot &&
        indexTop + 1 == spriteSetList.Count &&
        indexTop + 1 - spriteSetList.Count == indexBot
        )
        {
            indexTop = indexTop + 2 - spriteSetList.Count; 
        }
        else if(
            indexTop + 1 == indexBot && 
            indexTop + 2 < spriteSetList.Count)
        {
            indexTop++;
            indexTop++;
        }
        else if(
        indexTop + 1 == indexBot &&
        indexTop + 2 == spriteSetList.Count)
        {
            indexTop = indexTop + 2 - spriteSetList.Count;
        }

        topOrigin.sprite = spriteSetList[indexTop][2];
        topBasic.sprite = spriteSetList[indexTop][0];
        deleteTop = spriteSetList[indexTop][1];
        Debug.Log("top:  " + indexTop );
        Debug.Log("bot:  " + indexBot );
    }

    public void NextSymbolSetBot()
    {

        if(indexBot + 1 != indexTop && indexBot + 1 < spriteSetList.Count)
        {
            indexBot++;
        }
        else if(
        indexBot + 1 != indexTop &&
        indexBot + 1 == spriteSetList.Count &&
        indexBot + 1 - spriteSetList.Count != indexTop
        )
        {
           indexBot = indexBot + 1 - spriteSetList.Count; 
        }
        else if(
        indexBot + 1 != indexTop &&
        indexBot + 1 == spriteSetList.Count &&
        indexBot + 1 - spriteSetList.Count == indexTop
        )
        {
            indexBot = indexBot + 2 - spriteSetList.Count; 
        }
        else if(
            indexBot + 1 == indexTop && 
            indexBot + 2 < spriteSetList.Count)
        {
            indexBot++;
            indexBot++;
        }
        else if(
        indexBot + 1 == indexTop &&
        indexBot + 2 == spriteSetList.Count)
        {
            indexBot = indexBot + 2 - spriteSetList.Count;
        }
        

        botOrigin.sprite = spriteSetList[indexBot][2];
        botBasic.sprite = spriteSetList[indexBot][0];
        deleteBot = spriteSetList[indexBot][1];
        Debug.Log("top:  " + indexTop );
        Debug.Log("bot:  " + indexBot );
    }

    public void PreviusSymbolSetTop()
    {
        int x = 0;
        while(x != 2)
        {
            if(indexTop -1 <= 0)
            {
                indexTop = spriteSetList.Count - 1;
            }
            if(indexBot == indexTop)
            {
                indexTop--;
            }
            x++;
        }
        

        topOrigin.sprite = spriteSetList[indexTop][2];
        topBasic.sprite = spriteSetList[indexTop][0];
        deleteTop = spriteSetList[indexTop][1];
        Debug.Log("top:  " + indexTop );
        Debug.Log("bot:  " + indexBot );
    }

    

    public void PreviousSymbolSetBot()
    {
        int x = 0;
        while(x != 2)
        {

            if(indexBot - 1 <= 0)
            {
                indexBot = spriteSetList.Count - 1;
            }
            if(indexBot == indexTop)
            {
                indexBot--;
            }
            x++;
        }

        botOrigin.sprite = spriteSetList[indexBot][2];
        botBasic.sprite = spriteSetList[indexBot][0];
        deleteBot = spriteSetList[indexBot][1];
        Debug.Log("top:  " + indexTop );
        Debug.Log("bot:  " + indexBot );
    }
}
