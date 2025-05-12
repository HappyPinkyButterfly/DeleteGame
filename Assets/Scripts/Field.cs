using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
  
  public  List<Sprite> basicSymbol;
  public  List<Sprite> originSymbol;
  private Button[] buttons;

  public Board board;

  private CellForPrefab[] cells {get; set;}
  public Row[] rows {get;set;}
 
    private void Awake()
    {
      basicSymbol = new List<Sprite>(2);
      originSymbol = new List<Sprite>(2);
      buttons = GetComponentsInChildren<Button>();

      rows = GetComponentsInChildren<Row>();
      cells = GetComponentsInChildren<CellForPrefab>();

    }

    
}
