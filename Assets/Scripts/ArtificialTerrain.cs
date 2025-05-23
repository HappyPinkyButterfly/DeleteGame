using UnityEngine;
using UnityEngine.UI;

public class ArtificialTerrain : MonoBehaviour
{
    public bool artTerProcess;
    public Board board { get; set; }
    public bool isPlayerDown { get; set; }
    public bool artTerUsed = false;
    public Image artTerButtonImage { get; set; }
    private Color highlightColor = new Color(0.9f, 0.98f, 0.9f, 1f);

    public void Awake()
    {
        board = GetComponentInParent<Board>();
        Transform[] allParents = GetComponentsInParent<Transform>(true);
        isPlayerDown = false;
        foreach (Transform parent in allParents)
        {
            if (parent.name.Contains("TopEndStep"))
            {
                isPlayerDown = true;
                break;
            }
        }
        artTerButtonImage = GetComponent<Image>();

    }

    public void OnClickArtificialTerrain()
    {
        if (
             !isPlayerDown == board.turnPlayer &&
             board.connectionTable.Count == 0 &&
             !board.deleteProccess &&
             !board.moveProccess &&
             !board.healProccess &&
             !board.artTerProcess &&
             !artTerUsed &&
             !board.startStep &&
             EmptyCellAvailable()

         )
        {
            board.artTerProcess = true;
            artTerUsed = true;
            artTerButtonImage.color = Color.clear;
            HighlightEmptyCells();
        }
    }
    public bool EmptyCellAvailable()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            if (cell.state.occupation == 0)
            {
                return true;
            }
        }
        return false;
    }
    private void HighlightEmptyCells()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            if (cell.state.occupation == 0) // Empty cell
            {
                // Apply subtle green highlight
                cell.buttonImage.color = highlightColor;
            }
        }
    }
}
