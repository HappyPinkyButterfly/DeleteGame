using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    #region Game State
    [Header("Game State")]
    [Tooltip("Current player turn (true = Player 1/top, false = Player 2/bot)")]
    public bool turnPlayer { get; set; }
    [Tooltip("Whether game is in initial placement phase")]
    public bool startStep;
    [Header("First Move Flags")]
    public bool topFirstMove { get; set; }
    public bool botFirstMove { get; set; }
    [Tooltip("Count of occupied cells")]
    public int cellsInUse = 0;
    [Tooltip("Differentiates between game modes")]
    public bool boardType;
    [Tooltip("Balancing")]
    public bool firstSkipped;
    #endregion

    #region Game Components
    [Header("Player Components")]
    public UpDelete upDelete;
    public BotDelete botDelete;
    public TopScoreBoard topScoreBoard;
    public BotScoreBoard botScoreBoard;
    public TopTurn topTurn;
    public BotTurn botTurn;
    public TopEndStep topEndStep;
    public BotEndStep botEndStep;
    #endregion

    #region Prefabs
    [Header("Prefabs")]
    public Cell cellPrefab;
    public Delete deletePrefab;
    public Move movePrefab;
    public Heal healPrefab;
    public ArtificialTerrain artificialTerrainPrefab;
    #endregion

    #region Game Assets
    [Header("Symbol Sprites")]
    public Sprite basicSymP1;
    public Sprite basicSymP2;
    public Sprite originSymP1;
    public Sprite originSymP2;
    public Sprite deleteOriginSymP1;
    public Sprite deleteOriginSymP2;

    [Header("Cell States")]
    public Sprite emptyCell;
    public Sprite terrain;
    public Sprite artTer;
    public Sprite healedCell;

    [Header("UI Elements")]
    public Sprite draw;
    public Sprite mainMenu;
    public Sprite undo;
    public Image menuUndo;
    public bool menuButtonState;
    #endregion

    #region Processes
    public bool deleteProccess { get; set; }
    public bool moveProccess { get; set; }
    public bool artTerProcess { get; set; }
    public bool healProccess { get; set; }
    #endregion

    #region Internal Data
    private CellForPrefab[] cells;
    public bool disable { get; set; }
    private Row[] rows;
    public List<Cell> connectionTable { get; set; } = new List<Cell>();
    public Material material;
    public Cell selectedCellForMove { get; set; }
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        InitializeBoard();
    }
    #endregion

    #region Initialization
    private void InitializeBoard()
    {
        startStep = true;
        boardType = transform.Find("TopEndStep") != null;
        SetupBoardCells();
    }

    private void SetupBoardCells()
    {
        rows = GetComponentsInChildren<Row>();
        cells = GetComponentsInChildren<CellForPrefab>();

        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].cells.Length; x++)
            {
                InitializeCell(rows[y].cells[x], x, y);
            }
        }
    }

    private void InitializeCell(CellForPrefab cellPrefab, int x, int y)
    {
        cellPrefab.location = new Vector2Int(x, y);
        Cell newCell = Instantiate(this.cellPrefab, cellPrefab.transform);
        newCell.transform.position = cellPrefab.transform.position;
        newCell.location = cellPrefab.location;
    }

    /// <summary>
    /// Loads player symbols from the SymbolManager
    /// </summary>
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
    #endregion


    #region Connection Handeling
    /// <summary>
    /// Checks if three selected cells form a valid connection
    /// </summary>
    public bool CheckForConnection()
    {
        if (connectionTable.Count != 3) return false;

        Vector2Int pos1 = connectionTable[0].location;
        Vector2Int pos2 = connectionTable[1].location;
        Vector2Int pos3 = connectionTable[2].location;

        return CheckHorizontalConnection(pos1, pos2, pos3) || 
               CheckVerticalConnection(pos1, pos2, pos3) || 
               CheckDiagonalConnection(pos1, pos2, pos3);
    }

    private bool CheckHorizontalConnection(Vector2Int pos1, Vector2Int pos2, Vector2Int pos3)
    {
        if (pos1.y != pos2.y || pos2.y != pos3.y) return false;

        int[] xValues = { pos1.x, pos2.x, pos3.x };
        System.Array.Sort(xValues);
        return xValues[2] - xValues[1] == 1 && xValues[1] - xValues[0] == 1;
    }

    private bool CheckVerticalConnection(Vector2Int pos1, Vector2Int pos2, Vector2Int pos3)
    {
        if (pos1.x != pos2.x || pos2.x != pos3.x) return false;

        int[] yValues = { pos1.y, pos2.y, pos3.y };
        System.Array.Sort(yValues);
        return yValues[2] - yValues[1] == 1 && yValues[1] - yValues[0] == 1;
    }

    private bool CheckDiagonalConnection(Vector2Int pos1, Vector2Int pos2, Vector2Int pos3)
    {
        if (Mathf.Abs(pos1.x - pos2.x) != Mathf.Abs(pos1.y - pos2.y) ||
            Mathf.Abs(pos2.x - pos3.x) != Mathf.Abs(pos2.y - pos3.y))
            return false;

        int[] xValues = { pos1.x, pos2.x, pos3.x };
        int[] yValues = { pos1.y, pos2.y, pos3.y };
        System.Array.Sort(xValues);
        System.Array.Sort(yValues);

        bool diagonal1 = xValues[1] - xValues[0] == 1 && xValues[2] - xValues[1] == 1 &&
                        yValues[1] - yValues[0] == 1 && yValues[2] - yValues[1] == 1;

        bool diagonal2 = xValues[1] - xValues[0] == 1 && xValues[2] - xValues[1] == 1 &&
                        yValues[0] - yValues[1] == 1 && yValues[1] - yValues[2] == 1;

        return diagonal1 || diagonal2;
    }
    public void SuccessfulConnection()
    {
        ConvertCellsToTerrain();
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
    private void ConvertCellsToTerrain()
    {
        foreach (Cell cell in connectionTable)
        {
            cell.buttonImage.sprite = terrain;
            cell.state.occupation = (int)Cell.OccupationState.Terrain;
        }
    }

    /// <summary>
    /// Handles failed connection attempt with visual feedback
    /// </summary>
    public void UnsuccessfulConnection()
    {
        StartCoroutine(UnsuccessfulConnectionRoutine());
    }

     private IEnumerator UnsuccessfulConnectionRoutine()
    {
        HighlightConnectionCells(Color.red);
        yield return new WaitForSeconds(0.5f);
        HighlightConnectionCells(Color.white);
        connectionTable.Clear();
    }

    private void HighlightConnectionCells(Color color)
    {
        foreach (Cell cell in connectionTable)
        {
            cell.buttonImage.color = color;
        }
    }
    #endregion
    
    #region Game Reset
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
        firstSkipped = false;
        SetSymbolsFromManager();
        ResetMoveProcess();
        ResetMoves();
        ResetDeletes();
        ResetHeals();
        ResetArtTers();
        ResetCells();

        if (topScoreBoard != null)
            topScoreBoard.ResetPoints();

        if (botScoreBoard != null)
            botScoreBoard.ResetPoints();

        //topTurn.ResetTimer();
        //botTurn.ResetTimer();
    }

    public void ResetMoves()
    {
        Move[] allMoves = FindObjectsByType<Move>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Move move in allMoves)
        {
            if (!firstSkipped && move.isPlayerDown == !turnPlayer)
            {
                // One move taken from starting player for balancing
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
    }
    
    public void ResetDeletes()
    {
        Delete[] allDeletes = FindObjectsByType<Delete>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Delete delete in allDeletes)
        {
            delete.ResetDelete();
        }
    }

    public void ResetHeals()
    {
        Heal[] allHeals = FindObjectsByType<Heal>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Heal heal in allHeals)
        {
            heal.healUsed = false;
            heal.healButtonImage.color = Color.white;
        }
    }

    public void ResetArtTers()
    {
        ArtificialTerrain[] allArtTer = FindObjectsByType<ArtificialTerrain>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (ArtificialTerrain artTer in allArtTer)
        {
            artTer.artTerUsed = false;
            artTer.artTerButtonImage.color = Color.white;
        }
    }
    
    public void ResetCells()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            cell.ResetCell();
        }
    }
    
    public void ResetMoveProcess()
    {
        moveProccess = false;
        selectedCellForMove = null;
        if (turnPlayer)
        {
            topTurn.SwitchTurn();
        }
        else
        {
            botTurn.SwitchTurn();
        }
    }
    #endregion

    #region Process Handeling

    public void CancelDeleteProcess()
    {
        if (deleteProccess)
        {
            if (!turnPlayer)
            {
                Delete[] allDeletes = upDelete.GetComponentsInChildren<Delete>();
                foreach (Delete delete in allDeletes)
                {
                    if (delete.deleteUsed)
                    {
                        delete.ResetDelete();
                        break;
                    }
                }
            }
            else
            {
                Delete[] allDeletes = botDelete.GetComponentsInChildren<Delete>();
                foreach (Delete delete in allDeletes)
                {
                    if (delete.deleteUsed)
                    {
                        delete.ResetDelete();
                        break;
                    }
                }
            }
            deleteProccess = false;
        }

    }
    public void CancelMoveProcess()
    {
        if (moveProccess)
        {
            if (!turnPlayer)
            {
                Move[] allMoves = topEndStep.GetComponentsInChildren<Move>();
                foreach (Move move in allMoves)
                {
                    if (move.moveUsed)
                    {
                        move.moveUsed = false;
                        move.moveButtonImage.color = Color.white;
                        break;
                    }
                }
            }
            else
            {
                Move[] allMoves = botEndStep.GetComponentsInChildren<Move>();
                foreach (Move move in allMoves)
                {
                    if (move.moveUsed)
                    {
                        move.moveUsed = false;
                        move.moveButtonImage.color = Color.white;
                        break;
                    }
                }
            }
            moveProccess = false;
            selectedCellForMove = null;
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

    public void CancelHealProcess()
    {
        if (healProccess)
        {
            if (!turnPlayer)
            {
                Heal[] allHeals = topEndStep.GetComponentsInChildren<Heal>();
                foreach (Heal heal in allHeals)
                {
                    if (heal.healUsed)
                    {
                        heal.healUsed = false;
                        heal.healButtonImage.color = Color.white;
                        break;
                    }
                }
            }
            else
            {
                Heal[] allHeals = botEndStep.GetComponentsInChildren<Heal>();
                foreach (Heal heal in allHeals)
                {
                    if (heal.healUsed)
                    {
                        heal.healUsed = false;
                        heal.healButtonImage.color = Color.white;
                        break;
                    }
                }
            }
            healProccess = false;
        }
    }

    public void CancelArtTerrProcess()
    {
        if (artTerProcess)
        {
            if (!turnPlayer)
            {
                ArtificialTerrain[] allArtTer = topEndStep.GetComponentsInChildren<ArtificialTerrain>();
                foreach (ArtificialTerrain artTer in allArtTer)
                {
                    if (artTer.artTerUsed)
                    {
                        artTer.artTerUsed = false;
                        artTer.artTerButtonImage.color = Color.white;
                        break;
                    }
                }
            }
            else
            {
                ArtificialTerrain[] allArtTer = botEndStep.GetComponentsInChildren<ArtificialTerrain>();
                foreach (ArtificialTerrain artTer in allArtTer)
                {
                    if (artTer.artTerUsed)
                    {
                        artTer.artTerUsed = false;
                        artTer.artTerButtonImage.color = Color.white;
                        break;
                    }
                }
            }
            artTerProcess = false;
        }
    }

    public void CancelAllProcesses()
    {
        // Cancels all processes
        CancelDeleteProcess();
        CancelMoveProcess();
        CancelHealProcess();
        CancelArtTerrProcess();

        // Cancels all highlights
        CancelProcess();
    }
    #endregion

    #region Utility Methods
    /// <summary>
    /// Checks if enemy has normal non-origin symbols on the board
    /// </summary>
    public bool EnemyHasNormalSymbol()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            if (cell.state.occupation == (int)Cell.OccupationState.BasicSymbol && cell.state.symbolOwner != turnPlayer)
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
    

    public void MainMenuUndo(string type)
    {
        menuUndo.sprite = type == "MainMenu" ? mainMenu : undo;
        menuButtonState = type == "MainMenu";
    }
    #endregion
   
}

