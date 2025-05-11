using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour
{
    public bool turnPlayer { get; set; }
    // true je zgornji - 1, false je spodnji - 2
    public Sprite basicSymP1;
    public Sprite basicSymP2;

    public Sprite originSymP1;
    public Sprite originSymP2;

    public Sprite deleteOriginSymP1;

    public Sprite deleteOriginSymP2;

    public Sprite emptyCell;

    public Sprite terrain;

    public UpDelete upDelete;

    public BotDelete botDelete;

    public bool deleteProccess { get; set; }

    public List<Cell> connectionTable { get; set; } = new List<Cell>();

    public bool disable { get; set; }

    public Sprite corneredBoarders;

    public TopScoreBoard topScoreBoard;

    public BotScoreBoard botScoreBoard;

    public Sprite cover;

    public bool topFirstMove {get;set;}
    public bool botFirstMove {get;set;}

    public bool disableBoard = false;

    public Field field;

    public Cell cellPrefab;

    public Delete deletePrefab;

    private CellForPrefab[] cells;
    private Row[] rows;

    public void Start()
    {
        rows = GetComponentsInChildren<Row>();
        cells = GetComponentsInChildren<CellForPrefab>();
        for( int y = 0; y < rows.Length; y++ )
        {
          for(int x=0;x < rows[y].cells.Length; x++)
          {
            rows[y].cells[x].location = new Vector2Int(x,y);
            Cell newCell = Instantiate(cellPrefab,transform);
            newCell.transform.position = rows[y].cells[x].transform.position;
            newCell.location = rows[y].cells[x].location;
          }
        }
    }

    public bool CheckForConnection()
    {
        Vector2Int pos1 = connectionTable[0].location;
        Vector2Int pos2 = connectionTable[1].location;
        Vector2Int pos3 = connectionTable[2].location;

        // Razvrstimo celice po x ali y, da preverimo zaporednost
        bool isHorizontal = pos1.y == pos2.y && pos2.y == pos3.y;
        bool isVertical = pos1.x == pos2.x && pos2.x == pos3.x;
        bool isDiagonal = Mathf.Abs(pos1.x - pos2.x) == Mathf.Abs(pos1.y - pos2.y) &&
                        (Mathf.Abs(pos2.x - pos3.x) == Mathf.Abs(pos2.y - pos3.y));

        if (isHorizontal)
        {
            // Razvrsti po x in preveri, ali so tri zaporedne
            int[] xValues = { pos1.x, pos2.x, pos3.x };
            System.Array.Sort(xValues);
            return xValues[2] - xValues[1] == 1 && xValues[1] - xValues[0] == 1;
        }
        else if (isVertical)
        {
            // Razvrsti po y in preveri, ali so tri zaporedne
            int[] yValues = { pos1.y, pos2.y, pos3.y };
            System.Array.Sort(yValues);
            return yValues[2] - yValues[1] == 1 && yValues[1] - yValues[0] == 1;
        }
        else if (isDiagonal)
        {
            // Razvrsti po x in preveri, ali so diagonalne koordinate zaporedne
            // (Diagonala ima lahko naklon +1 ali -1)
            int[] xValues = { pos1.x, pos2.x, pos3.x };
            System.Array.Sort(xValues);
            int[] yValues = { pos1.y, pos2.y, pos3.y };
            System.Array.Sort(yValues);

            // Preveri, ali se x in y premikata enako (za diagonalo)
            bool isDiagonal1 = xValues[1] - xValues[0] == 1 && xValues[2] - xValues[1] == 1 &&
                            yValues[1] - yValues[0] == 1 && yValues[2] - yValues[1] == 1;
            bool isDiagonal2 = xValues[1] - xValues[0] == 1 && xValues[2] - xValues[1] == 1 &&
                            yValues[0] - yValues[1] == 1 && yValues[1] - yValues[2] == 1;

            return isDiagonal1 || isDiagonal2;

        }
        return false;
    }

    public void SuccessfulConnection()
    {
        foreach (Cell cell in connectionTable)
        {
            cell.buttonImage.sprite = terrain;
            cell.state.occupation = 3;
        }
        connectionTable.Clear();
        turnPlayer = !turnPlayer;

    }

    private IEnumerator ConnectionRoutine()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public void UnsuccessfulConnection()
    {
        StartCoroutine(UnsuccessfulConnectionRoutine());
    }

    private IEnumerator UnsuccessfulConnectionRoutine()
    {
        foreach (Cell cell in connectionTable)
        {
            cell.buttonImage.color = Color.red;
        }

        yield return new WaitForSeconds(0.5f); // počakaj 1 sekundo

        foreach (Cell cell in connectionTable)
        {
            cell.buttonImage.color = Color.white;
        }

        connectionTable.Clear();
    }
    public void ResetGame()
    {

        disable = false;
        turnPlayer = true;
        topFirstMove = true;
        botFirstMove = true;
        connectionTable.Clear();
        deleteProccess = false;

        // Resetiraj vse celice
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            cell.ResetCell();
        }

       
        Delete[] allDeletes = FindObjectsByType<Delete>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Delete delete in allDeletes)
        {
            delete.ResetDelete();
        }

        // Resetiraj točke
        if (topScoreBoard != null)
            topScoreBoard.ResetPoints();
        
        if (botScoreBoard != null)
            botScoreBoard.ResetPoints();
    }

   
}

