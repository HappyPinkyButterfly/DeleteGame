
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;


public class Cell : MonoBehaviour
{

    private Board board;

    public CellState state { get; set; }

    public Button button;
    public Image buttonImage { get; set; }
    public Vector2Int location { get; set; }
    public List<Cell> moveTable { get; set; } = new List<Cell>();

    public void Awake()
    {
        board = FindFirstObjectByType<Board>();
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = board.emptyCell;
        state = gameObject.AddComponent<CellState>();

    }

    public void CellClick()
    {
        if (board.healProccess && this.state.occupation == 3 && !board.startStep )
        {
            buttonImage.sprite = board.healedCell;
            buttonImage.color = Color.white;
            this.state.occupation = 5;
            board.healProccess = false;
            board.cellsInUse--;
            board.turnPlayer = !board.turnPlayer;
            board.startStep = true;
            ResetTerrainAlpha();
            return;
        }
        else if (board.artTerProcess && this.state.occupation == 0 && !board.startStep)
        {
            buttonImage.sprite = board.artTer;
            this.state.occupation = 4;
            board.artTerProcess = false;
            board.cellsInUse++;
            board.turnPlayer = !board.turnPlayer;
            board.startStep = true;
            ClearHighlightsArtTer();
            return;
        }
        else if (board.moveProccess && !board.startStep)
        {
            if (board.selectedCellForMove == null)
            {
                if (this.state.symbolOwner == board.turnPlayer &&
                    (this.state.occupation == 1 || this.state.occupation == 2))
                {
                    ClearAllMoveHighlights();
                    board.selectedCellForMove = this;
                    FindPossibleMoves();
                    HighlightSelection();
                }
                return;
            }

            // Faza 2: Izbor ciljne celice
            if (board.selectedCellForMove != null &&
                this != board.selectedCellForMove &&
                board.selectedCellForMove.moveTable != null &&
                board.selectedCellForMove.moveTable.Contains(this))
            {
                MoveSymbolToThisCell();
                board.ResetMoveProcess();
            }

        }


        else if (!board.deleteProccess &&
                !board.moveProccess &&
                !board.artTerProcess &&
                !board.healProccess &&
                board.startStep)
        {
            if ((state.occupation == 0 || state.occupation == 5) && board.connectionTable.Count == 0)
            {
                // Prvi potezi za obe strani
                if (board.turnPlayer && board.topFirstMove)
                {
                    buttonImage.sprite = board.originSymP1;
                    state.symbolOwner = true;
                    state.occupation = 2;
                    board.topFirstMove = false;
                    if (!board.boardType)
                    {
                        board.turnPlayer = !board.turnPlayer;

                    }
                    else
                    {
                        board.startStep = false;
                    }
                    board.cellsInUse++;
                    return;
                }
                else if (!board.turnPlayer && board.botFirstMove)
                {
                    buttonImage.sprite = board.originSymP2;
                    state.symbolOwner = false;
                    state.occupation = 2;
                    board.botFirstMove = false;
                    if (!board.boardType)
                    {
                        board.turnPlayer = !board.turnPlayer;

                    }
                    else
                    {
                        board.startStep = false;
                    }
                    board.cellsInUse++;

                    return;
                }

                // Normalne poteze
                if (board.turnPlayer)
                {
                    buttonImage.sprite = board.basicSymP1;
                    state.symbolOwner = true;
                }
                else
                {
                    buttonImage.sprite = board.basicSymP2;
                    state.symbolOwner = false;
                }
                state.occupation = 1;
                board.cellsInUse++;
                if (!board.boardType)
                {
                    board.turnPlayer = !board.turnPlayer;

                }
                else
                {
                    board.startStep = false;
                }

            }
            else if ((state.occupation == 1 || state.occupation == 2) && board.turnPlayer == state.symbolOwner)
            {
                if (!board.connectionTable.Contains(this))
                {
                    board.connectionTable.Add(this);
                    buttonImage.color = Color.green;

                    if (board.connectionTable.Count == 3)
                    {
                        if (board.CheckForConnection())
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
        }
        else if (state.occupation == 0 && board.deleteProccess && board.startStep)
        {
            // popravi problem kjer ce si kluknil na delete, pocakal malo, 
            // kliknil prazno cell, pocakal malo in kliknil legal target,
            //  se delete ni izbrisal
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }

        else if (state.occupation == 1 &&
                board.turnPlayer != state.symbolOwner &&
                board.connectionTable.Count == 0 &&
                !board.artTerProcess &&
                !board.healProccess &&
                board.startStep)
        {
            if (board.turnPlayer)
            {
                buttonImage.sprite = board.originSymP1;
                //board.botDelete.HideUsedDelete();
            }
            else
            {
                buttonImage.sprite = board.originSymP2;
                //board.upDelete.HideUsedDelete();
            }
            state.occupation = 2;
            if (!board.boardType)
            {
                board.turnPlayer = !board.turnPlayer;

            }
            else
            {
                board.startStep = false;
            }
            state.symbolOwner = !state.symbolOwner;
            board.deleteProccess = false;
            ClearAllDeleteHighlights();
        }
    }

    public void ResetCell()
    {
        buttonImage.sprite = board.emptyCell;
        buttonImage.color = Color.white;
        state.occupation = 0;
    }

    public void FindPossibleMoves()
    {
        moveTable.Clear();

        // Vse možne smeri v 8-smernem gridu
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 0),   // desno
            new Vector2Int(1, 1),   // desno gor
            new Vector2Int(0, 1),    // gor
            new Vector2Int(-1, 1),  // levo gor
            new Vector2Int(-1, 0),  // levo
            new Vector2Int(-1, -1), // levo dol
            new Vector2Int(0, -1),  // dol
            new Vector2Int(1, -1)   // desno dol
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
    private void HighlightSelection()
    {
        // Označi izbrano celico s svetlo zeleno
        this.buttonImage.color = Color.yellow; // Svetlo zelena

        // Označi vse možne cilje s temno zeleno
        foreach (Cell cell in moveTable)
        {
            cell.buttonImage.color = Color.green; // Temno zelena
        }
    }


    private void MoveSymbolToThisCell()
    {
        // Premakni znak
        this.buttonImage.sprite = board.selectedCellForMove.buttonImage.sprite;
        this.state.occupation = board.selectedCellForMove.state.occupation;
        this.state.symbolOwner = board.selectedCellForMove.state.symbolOwner;

        // Počisti staro celico
        board.selectedCellForMove.ResetCell();

        // Ponastavi označbe (brez ponastavitve Move gumba)
        board.selectedCellForMove.ClearHighlights();
        board.moveProccess = false;
        board.selectedCellForMove = null;

        // Move gumb OSTAJE porabljen (moveUsed ostane true)
    }

    public void ClearHighlights()
    {
        // Ponastavi barvo trenutne celice
        this.buttonImage.color = Color.white;

        // Ponastavi barve vseh možnih ciljnih celic
        foreach (Cell cell in this.moveTable)
        {
            if (cell != null && cell.buttonImage != null)
            {
                cell.buttonImage.color = Color.white;
            }
        }

        // Počisti seznam možnih potez
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
