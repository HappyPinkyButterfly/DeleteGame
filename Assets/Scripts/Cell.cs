
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Represents a single cell on the game board with state management and interaction handling.
/// Handles all cell behaviors including placement, movement, special abilities, and visual feedback.
/// </summary>
public class Cell : MonoBehaviour
{
    #region Enums
    /// <summary>
    /// Represents the occupation state of a cell
    /// </summary>
    public enum OccupationState
    {
        Empty = 0,          // Empty cell
        BasicSymbol = 1,    // Basic player symbol
        OriginSymbol = 2,   // Master/origin symbol
        Terrain = 3,        // Converted terrain from connection
        ArtificialTerrain = 4, // Player-created blocking terrain
        HealedEmpty = 5     // Healed cell (can be reclaimed)
    }
    #endregion

    #region References
    [Header("Component References")]
    [SerializeField] public Image buttonImage;
    private Board board;
    private TopTurn topTurn;
    #endregion

    #region Properties
    public CellState state { get; private set; }
    public Vector2Int location { get; set; }
    public List<Cell> moveTable { get; private set; } = new List<Cell>();
    #endregion

    public void Awake()
    {
        board = FindFirstObjectByType<Board>();
        topTurn = FindFirstObjectByType<TopTurn>();
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = board.emptyCell;
        state = gameObject.AddComponent<CellState>();

    }
    #region Click Handling
    /// <summary>
    /// Main click handler for cell interactions
    /// </summary>
    public void CellClick()
    {
        if (HandleInitialPlacement()) return;
        if (HandleConnectionAttempt()) return;
        if (HandleDelete()) return;
        if (HandleMoveAction()) return;
        if (HandleSpecialAbilityActions()) return;

    }

    #endregion

    #region Handle Initial Placement

    public bool HandleInitialPlacement()
    {
        if (board.startStep &&
            !board.deleteProccess &&
            !board.moveProccess &&
            !board.artTerProcess &&
            !board.healProccess &&
            (state.occupation == (int)OccupationState.Empty ||
            state.occupation == (int)OccupationState.HealedEmpty) &&
            board.connectionTable.Count == 0)
        {
            HandleSymbolPlacement();
            return true;
        }
        return false;

    }

    private void HandleSymbolPlacement()
    {
        if (board.turnPlayer && board.topFirstMove)
        {
            PlaceOriginSymbol(board.originSymP1, true);
            board.topFirstMove = false;
        }
        else if (!board.turnPlayer && board.botFirstMove)
        {
            PlaceOriginSymbol(board.originSymP2, false);
            board.botFirstMove = false;
        }
        else
        {
            PlaceBasicSymbol();
        }

        AdvanceGameState();
    }

    private void PlaceOriginSymbol(Sprite originSprite, bool isPlayer1)
    {
        buttonImage.sprite = originSprite;
        state.symbolOwner = isPlayer1;
        state.occupation = (int)OccupationState.OriginSymbol;
        board.cellsInUse++;
    }

    private void PlaceBasicSymbol()
    {
        buttonImage.sprite = board.turnPlayer ? board.basicSymP1 : board.basicSymP2;
        state.symbolOwner = board.turnPlayer;
        state.occupation = (int)OccupationState.BasicSymbol;
        board.cellsInUse++;
    }
    private void AdvanceGameState()
    {
        if (!board.boardType)
        {
            board.turnPlayer = !board.turnPlayer;
        }
        else
        {
            board.startStep = false;
        }
    }
    #endregion

    #region Handle Connection Attempt
    private bool HandleConnectionAttempt()
    {
        if (board.startStep &&
            (state.occupation == (int)OccupationState.BasicSymbol ||
             state.occupation == (int)OccupationState.OriginSymbol) &&
            board.turnPlayer == state.symbolOwner &&
            !board.deleteProccess &&
            !board.moveProccess &&
            !board.artTerProcess &&
            !board.healProccess)
        {
            ToggleCellInConnectionTable();
            return true;
        }
        return false;
    }

    private void ToggleCellInConnectionTable()
    {
        if (!board.connectionTable.Contains(this))
        {
            board.connectionTable.Add(this);
            buttonImage.color = Color.green;

            if (board.connectionTable.Count == 3)
            {
                if (board.CheckForConnection())
                {
                    AwardConnectionPoints();
                    board.SuccessfulConnection();
                }
                else
                {
                    board.UnsuccessfulConnection();
                }
            }
        }
        else
        {
            buttonImage.color = Color.white;
            board.connectionTable.Remove(this);
        }
    }

    private void AwardConnectionPoints()
    {
        if (!board.turnPlayer)
        {
            board.topScoreBoard.AddVictoryPointTop();
            board.upDelete.getOneDeleteBack();
        }
        else
        {
            board.botScoreBoard.AddVictoryPointBot();
            board.botDelete.getOneDeleteBack();
        }
    }
    #endregion

    #region Handle Delete
    private bool HandleDelete()
    {
        if (board.startStep &&
            state.occupation == (int)OccupationState.BasicSymbol &&
            board.turnPlayer != state.symbolOwner &&
            board.connectionTable.Count == 0 &&
            !board.artTerProcess &&
            !board.healProccess &&
            !board.moveProccess &&
            board.deleteProccess)
        {
            DeleteSymbol();
            return true;
        }
        return false;
    }

    private void DeleteSymbol()
    {
        buttonImage.sprite = board.turnPlayer ? board.originSymP1 : board.originSymP2;
        state.occupation = (int)OccupationState.OriginSymbol;
        state.symbolOwner = !state.symbolOwner;
        board.deleteProccess = false;
        ClearAllDeleteHighlights();
        board.MainMenuUndo("MainMenu");
        AdvanceGameState();
    }

    #endregion

    #region Handle Move Action
    private bool HandleMoveAction()
    {
        if (board.moveProccess &&
            !board.startStep &&
            board.connectionTable.Count == 0 &&
            !board.artTerProcess &&
            !board.healProccess &&
            !board.deleteProccess)
        {

            if (board.selectedCellForMove == null)
            {
                return TrySelectCellForMove();
            }
            else
            {
                return TryMoveSymbolToThisCell();
            }
        }

        return false;
    }
    private bool TrySelectCellForMove()
    {
        if (state.symbolOwner == board.turnPlayer &&
            (state.occupation == (int)OccupationState.BasicSymbol ||
             state.occupation == (int)OccupationState.OriginSymbol))
        {
            FindPossibleMoves();
            if (moveTable.Count > 0)
            {
                ClearAllMoveHighlights();
                board.selectedCellForMove = this;
                HighlightSelection();
                return true;
            }
        }
        return false;
    }

    private bool TryMoveSymbolToThisCell()
    {
        if (board.selectedCellForMove != null &&
            this != board.selectedCellForMove &&
            board.selectedCellForMove.moveTable != null &&
            board.selectedCellForMove.moveTable.Contains(this))
        {
            MoveSymbolToThisCell();
            board.ResetMoveProcess();
            board.MainMenuUndo("MainMenu");
            return true;
        }
        return false;
    }

    public void FindPossibleMoves()
    {
        moveTable.Clear();

        // all 8 way possible directions
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 0),   // right
            new Vector2Int(1, 1),   // right up
            new Vector2Int(0, 1),    // up
            new Vector2Int(-1, 1),  // left up
            new Vector2Int(-1, 0),  // left
            new Vector2Int(-1, -1), // left down
            new Vector2Int(0, -1),  // down
            new Vector2Int(1, -1)   // right down
        };

        foreach (Vector2Int direction in directions)
        {
            CheckDirection(direction);
        }
    }
    private void CheckDirection(Vector2Int direction)
    {
        Vector2Int currentPos = location;

        while (true)
        {
            currentPos += direction;

            // Check if we're out of bounds
            if (currentPos.x < 0 || currentPos.y < 0 || currentPos.x > 7 || currentPos.y > 7)
            {
                break;
            }

            Cell neighbor = board.GetCellAtPosition(currentPos);

            // Skip if out of bounds
            if (neighbor == null)
            {
                break;
            }

            // Check occupation state
            switch (neighbor.state.occupation)
            {
                case 0: // Empty cell - valid target
                case 5: // Healed cell - valid target
                    moveTable.Add(neighbor);
                    return; // Found a valid target, stop searching this direction

                case 1: // Basic symbol - blocking
                case 2: // Origin symbol - blocking
                    return; // Hit a blocking cell, stop searching this direction

                case 3: // Terrain - pass through
                case 4: // Artificial terrain - pass through
                    continue; // Keep searching in this direction

                default:
                    Debug.LogWarning($"Unknown occupation state: {neighbor.state.occupation}");
                    return;
            }
        }
    }

    private void MoveSymbolToThisCell()
    {
        // move symbol
        this.buttonImage.sprite = board.selectedCellForMove.buttonImage.sprite;
        this.state.occupation = board.selectedCellForMove.state.occupation;
        this.state.symbolOwner = board.selectedCellForMove.state.symbolOwner;

        board.selectedCellForMove.ResetCell();
        board.selectedCellForMove.ClearHighlights();
        board.moveProccess = false;
        board.selectedCellForMove = null;
    }

    #endregion

    #region Handle Special Actions

    private bool HandleSpecialAbilityActions()
    {
        if (!board.moveProccess &&
            !board.startStep &&
            board.connectionTable.Count == 0 &&
            !board.artTerProcess &&
            board.healProccess &&
            !board.deleteProccess &&
            state.occupation == (int)OccupationState.Terrain
            )
        {
            HandleHealAction();
            return true;
        }

        if (!board.moveProccess &&
            !board.startStep &&
            board.connectionTable.Count == 0 &&
            board.artTerProcess &&
            !board.healProccess &&
            !board.deleteProccess &&
            state.occupation == (int)OccupationState.Empty)
        {
            HandleArtificialTerrainAction();
            return true;
        }

        return false;
    }

    private void HandleHealAction()
    {
        buttonImage.sprite = board.healedCell;
        buttonImage.color = Color.white;
        state.occupation = (int)OccupationState.HealedEmpty;
        board.healProccess = false;
        board.cellsInUse--;
        EndTurn();
        board.MainMenuUndo("MainMenu");
        ResetTerrainAlpha();
    }

    private void HandleArtificialTerrainAction()
    {
        buttonImage.sprite = board.artTer;
        state.occupation = (int)OccupationState.ArtificialTerrain;
        board.artTerProcess = false;
        board.cellsInUse++;
        EndTurn();
        board.MainMenuUndo("MainMenu");
        ClearHighlightsArtTer();
    }
    private void EndTurn()
    {
        if (board.turnPlayer)
        {
            topTurn.SwitchTurn();
        }
        else
        {
            board.botTurn.SwitchTurn();
        }
    }

    #endregion

    #region Highlights
    public void ResetCell()
    {
        buttonImage.sprite = board.emptyCell;
        buttonImage.color = Color.white;
        state.occupation = 0;
    }


    private void HighlightSelection()
    {

        this.buttonImage.color = Color.yellow; 

        // highlight all posible moves
        foreach (Cell cell in moveTable)
        {
            cell.buttonImage.color = Color.green; 
        }
    }




    public void ClearHighlights()
    {
        // reset cell color
        this.buttonImage.color = Color.white;

        // reset all cells color
        foreach (Cell cell in this.moveTable)
        {
            if (cell != null && cell.buttonImage != null)
            {
                cell.buttonImage.color = Color.white;
            }
        }
        this.moveTable.Clear();
    }

    public void ClearAllDeleteHighlights()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            cell.buttonImage.color = Color.white;
        }
    }

    public void ClearAllMoveHighlights()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            cell.buttonImage.color = Color.white;
        }
    }

    public void ClearAllHighlights()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            cell.buttonImage.color = Color.white;
            if (cell.buttonImage.transform.childCount > 0)
                cell.buttonImage.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ClearHighlightsArtTer()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            cell.buttonImage.color = Color.white;
        }
    }
    public void ResetTerrainAlpha()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            if (cell.state.occupation == 3) // Terren
            {
                // Ponastavi na polno prekrivnost
                Color cellColor = cell.buttonImage.color;
                cellColor.a = 1f;
                cell.buttonImage.color = cellColor;
            }
        }
    }
}
#endregion
