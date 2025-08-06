using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    public Board board { get; set; }
    public bool isPlayerDown { get; set; }
    public bool moveUsed = false;
    public Image moveButtonImage { get; set; }

    public void Awake()
    {
        board = GetComponentInParent<Board>();
        isPlayerDown = transform.parent.name.Contains("TopMoveCell");
        moveButtonImage = GetComponent<Image>();

    }
    public void OnMoveClick()
    {
        if (CanActivateMove())
        {
            ActivateMoveMode();
        }
    }

    private bool CanActivateMove()
    {
        return !isPlayerDown == board.turnPlayer &&
               board.connectionTable.Count == 0 &&
               !board.deleteProccess &&
               !board.moveProccess &&
               !board.healProccess &&
               !board.artTerProcess &&
               !moveUsed &&
               !board.startStep &&
               PlayerHasMovableSymbols();
    }

    private void ActivateMoveMode()
    {
        board.moveProccess = true;
        moveUsed = true;
        moveButtonImage.color = Color.clear;
        board.MainMenuUndo("Undo");
        HighlightMovableSymbols();
    }
    private bool PlayerHasMovableSymbols()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            if (IsPlayerSymbol(cell) && HasValidMoves(cell))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsPlayerSymbol(Cell cell)
    {
        return cell.state.symbolOwner == board.turnPlayer && 
               (cell.state.occupation == 1 || cell.state.occupation == 2);
    }


    private bool HasValidMoves(Cell cell)
    {
        cell.FindPossibleMoves();
        return cell.moveTable.Count > 0;
    }


    private void HighlightMovableSymbols()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            UpdateCellHighlight(cell);
        }
    }


    private void UpdateCellHighlight(Cell cell)
    {
        if (IsPlayerSymbol(cell) && HasValidMoves(cell))
        {
            cell.buttonImage.color = Color.cyan;
        }
        else
        {
            cell.buttonImage.color = Color.white;
        }
    }
    public void ResetMove()
    {
        moveUsed = false;
        moveButtonImage.color = Color.white;
    }
}
