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

    private void Start()
    {
      
    }

    public void DetermineLocationForCells()
    {
        for( int y = 0; y < rows.Length; y++ )
        {
          for(int x=0;x < rows[y].cells.Length; x++)
          {
            rows[y].cells[x].location = new Vector2Int(x,y);
          }
        }
    }

    public bool EnemyHasNormalSymbol()
    {
      // for( int y = 0; y < rows.Length; y++ )
      //   {
      //     for(int x=0;x < rows[y].cells.Length; x++)
      //     {
      //       if (rows[y].cells[x].state.occupation == 1 && rows[y].cells[x].state.symbolOwner != board.turnPlayer)
      //       {
      //         return true;
      //       }
      //     }
      //   }
      // return false;
      return true;
    }
}
