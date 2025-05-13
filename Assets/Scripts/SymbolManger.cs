using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class SymbolManger : MonoBehaviour
{
    public static string symbolsPath = "Assets/Symbols";
    
    public static List<List<Sprite>> spriteSetList = new List<List<Sprite>>();

    public Image topOrigin;
    public Image topBasic;

    public Image botOrigin;
    public Image botBasic;

    int indexTop = 0;
    int indexBot = 1;

    private Sprite deleteTop {get;set;}
    private Sprite deleteBot {get;set;}


    private void Start()
    {
        LoadSprites();

        deleteTop = spriteSetList[indexTop][1];
        deleteBot = spriteSetList[indexBot][1];

        topOrigin.sprite = spriteSetList[indexTop][2];
        botOrigin.sprite = spriteSetList[indexBot][2];

        topBasic.sprite = spriteSetList[indexTop][0];
        botBasic.sprite = spriteSetList[indexBot][0];
        
    }

    public static void LoadSprites()
    {
    // dobim vse poti do vseh folderjev v assets/symbols  
    string[] symbolFolders = Directory.GetDirectories(symbolsPath);
    foreach (string folderPath in symbolFolders)
        {
            string relativePath = folderPath.Replace("\\", "/");
            string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { relativePath });
            List<Sprite> spriteSet = new List<Sprite>();
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                spriteSet.Add(sprite);
            }
            spriteSetList.Add(spriteSet);
        }
    }
    public void NextSymbolSetTop()
    {
        if(indexTop + 1 == indexBot)
        {
            indexTop++;
            indexTop++;
        }
        else
        {
           indexTop++; 
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
        }
        else
        {
           indexTop--; 
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
        }
        else
        {
           indexBot++; 
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
        }
        else
        {
           indexBot--; 
        }
        botOrigin.sprite = spriteSetList[indexBot][2];
        botBasic.sprite = spriteSetList[indexBot][0];
        deleteBot = spriteSetList[indexBot][1];
    }
}
