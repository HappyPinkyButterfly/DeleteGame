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
        if(indexTop + 1 == indexBot)
        {
            indexTop++;
            indexTop++;
            if(indexTop > spriteSetList.Count - 1)
            {
                indexTop = 0;
            }
        }
        else
        {
           indexTop++; 
           if(indexTop > spriteSetList.Count - 1)
            {
                indexTop = 0;
            }
        }
        topOrigin.sprite = spriteSetList[indexTop][2];
        topBasic.sprite = spriteSetList[indexTop][0];
        deleteTop = spriteSetList[indexTop][1];
    }

    public void PreviusSymbolSetTop()
    {
        if(indexTop - 1 == indexBot)
        {
            indexTop--;
            indexTop--;
            if(indexTop < 0)
            {
                indexTop = spriteSetList.Count - 1;
            }
        }
        else
        {
           indexTop--;
           if(indexTop < 0)
            {
                indexTop = spriteSetList.Count - 1;
            } 
        }
        topOrigin.sprite = spriteSetList[indexTop][2];
        topBasic.sprite = spriteSetList[indexTop][0];
        deleteTop = spriteSetList[indexTop][1];
    }

    public void NextSymbolSetBot()
    {
        if(indexTop == indexBot + 1)
        {
            indexBot++;
            indexBot++;
            if(indexBot > spriteSetList.Count - 1)
            {
                indexBot = 0;
            }
        }
        else
        {
           indexBot++;
           if(indexBot > spriteSetList.Count - 1)
            {
                indexBot = 0;
            } 
        }
        botOrigin.sprite = spriteSetList[indexBot][2];
        botBasic.sprite = spriteSetList[indexBot][0];
        deleteBot = spriteSetList[indexBot][1];
    }

    public void PreviousSymbolSetBot()
    {
        if(indexTop == indexBot - 1)
        {
            indexBot--;
            indexBot--;
            if(indexBot < 0)
            {
                indexBot = spriteSetList.Count - 1;
            }
        }
        else
        {
           indexBot--;
           if(indexBot < 0)
            {
                indexBot = spriteSetList.Count - 1;
            } 
        }
        botOrigin.sprite = spriteSetList[indexBot][2];
        botBasic.sprite = spriteSetList[indexBot][0];
        deleteBot = spriteSetList[indexBot][1];
    }
}
