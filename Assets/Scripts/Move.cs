using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    public bool moveProccess;
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


        if (
            !isPlayerDown == board.turnPlayer &&
            board.connectionTable.Count == 0 &&
            !board.deleteProccess &&
            !board.moveProccess &&
            !board.healProccess &&
            !board.artTerProcess &&
            !moveUsed &&
            !board.startStep &&
            PlayerHasMovableSymbols()
        )
        {
            board.moveProccess = true;
            moveUsed = true;
            moveButtonImage.color = Color.clear;

            HighlightMovableSymbols();
        }

    }
    private bool PlayerHasMovableSymbols()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            if (cell.state.symbolOwner == board.turnPlayer &&
                (cell.state.occupation == 1 || cell.state.occupation == 2))
            {
                return true;
            }
        }
        return false;
    }
    
    private void HighlightMovableSymbols()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            if (cell.state.symbolOwner == board.turnPlayer &&
                (cell.state.occupation == 1 || cell.state.occupation == 2))
            {
                cell.buttonImage.color = Color.cyan; // Use cyan for movable symbols
            }
            else
            {
                cell.buttonImage.color = Color.white;
            }
        }
    }
}
