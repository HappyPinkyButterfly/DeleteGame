using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEditor;

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

    public bool topFirstMove { get; set; }
    public bool botFirstMove { get; set; }


    public Field field;

    public Cell cellPrefab;

    public Delete deletePrefab;

    private CellForPrefab[] cells;
    private Row[] rows;

    public Material material;

    public int cellsInUse = 0;

    public Sprite draw;
    public TopTurn topTurn;
    public BotTurn botTurn;
    public Move movePrefab;
    public bool moveProccess { get; set; }
    public Cell selectedCellForMove { get; set; }

    public Heal healPrefab;
    public ArtificialTerrain artificialTerrainPrefab;
    public bool artTerProcess { get; set; }
    public Sprite artTer;
    public bool healProccess { get; set; }

    public Sprite healedCell;
    public bool boardType;

    public bool startStep;




    public void Start()
    {
        startStep = true;
        boardType = transform.Find("TopEndStep") != null;
        Debug.Log(boardType);
        rows = GetComponentsInChildren<Row>();
        cells = GetComponentsInChildren<CellForPrefab>();
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].location = new Vector2Int(x, y);
                Cell newCell = Instantiate(cellPrefab, rows[y].cells[x].transform);
                newCell.transform.position = rows[y].cells[x].transform.position;
                newCell.location = rows[y].cells[x].location;

            }
        }
    }
    public void SetSymbolsFromManager()
    {
        if (SymbolManger.Instance == null) return;

        basicSymP2 = SymbolManger.spriteSetList[SymbolManger.Instance.indexTop][0];
        basicSymP1 = SymbolManger.spriteSetList[SymbolManger.Instance.indexBot][0];

        originSymP2 = SymbolManger.spriteSetList[SymbolManger.Instance.indexTop][2];
        originSymP1 = SymbolManger.spriteSetList[SymbolManger.Instance.indexBot][2];

        deleteOriginSymP2 = SymbolManger.spriteSetList[SymbolManger.Instance.indexTop][1];
        deleteOriginSymP1 = SymbolManger.spriteSetList[SymbolManger.Instance.indexBot][1];
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
        if (!boardType)
        {
            turnPlayer = !turnPlayer;

        }
        else
        {
            startStep = false;
        }

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
        turnPlayer = Random.Range(0, 2) == 1;
        topFirstMove = true;
        botFirstMove = true;
        connectionTable.Clear();
        deleteProccess = false;
        startStep = true;
        cellsInUse = 0;
        SetSymbolsFromManager();

        bool firstSkipped = false;

        Move[] allMoves = FindObjectsByType<Move>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Move move in allMoves)
        {
            if (!firstSkipped && move.isPlayerDown == !turnPlayer)
            {
                // Prvi delete za začetnega igralca nastavimo kot že porabljen
                move.moveUsed = true;
                move.moveButtonImage.color = Color.clear;
                firstSkipped = true;
            }
            else
            {
                move.moveUsed = false;
                move.moveButtonImage.color = Color.white; 
            }
                
        }

        ArtificialTerrain[] allArtTer = FindObjectsByType<ArtificialTerrain>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (ArtificialTerrain artTer in allArtTer)
        {
            artTer.artTerUsed = false;
            artTer.artTerButtonImage.color = Color.white;
        }

        // Resetiraj vse celice
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            cell.ResetCell();
        }

        // Resetiraj vse celice
        Heal[] allHeals = FindObjectsByType<Heal>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Heal heal in allHeals)
        {
            heal.healUsed = false;
            heal.healButtonImage.color = Color.white;
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

    public bool EnemyHasNormalSymbol()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            if (cell.state.occupation == 1 && cell.state.symbolOwner != turnPlayer)
            {
                return true;
            }
        }
        return false;
    }

    public Cell GetCellAtPosition(Vector2Int pos)
    {
        foreach (Row row in rows)
        {
            foreach (CellForPrefab cell in row.cells)
            {
                if (cell.location == pos)
                {
                    return cell.GetComponentInChildren<Cell>();
                }
            }
        }
        return null;
    }

    // V Board.cs spremenite ResetMoveProcess() na:
    public void ResetMoveProcess()
    {
        moveProccess = false;
        selectedCellForMove = null;
        turnPlayer = !turnPlayer;
        startStep = true;
    }

    public void CancelDeleteProcess()
    {
        deleteProccess = false;

        // Clear all highlights
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            cell.buttonImage.color = Color.white;
        }
    }
    public void CancelMoveProcess()
    {
        moveProccess = false;
        selectedCellForMove = null;

        // Clear all highlights
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            cell.buttonImage.color = Color.white;
        }
    }
    
    public void CancelProcess()
    {

        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            cell.buttonImage.color = Color.white;
        }
    }
   
}

