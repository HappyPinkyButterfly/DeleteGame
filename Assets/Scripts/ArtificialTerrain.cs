using UnityEngine;
using UnityEngine.UI;

public class ArtificialTerrain : MonoBehaviour
{
    // Configuration
    private Color highlightColor = new Color(0.9f, 0.98f, 0.9f, 1f);
    // State
    public bool isPlayerDown { get; set; }
    public bool artTerUsed = false;
    // References
    public Image artTerButtonImage { get; set; }
    public Board board { get; set; }
    

    public void Awake()
    {
        InitializeComponents();
        DeterminePlayerPosition();
    }

    private void InitializeComponents()
    {
        board = GetComponentInParent<Board>();
        artTerButtonImage = GetComponent<Image>();
        
        if (artTerButtonImage == null)
            Debug.LogError("Missing Image component", this);
    }

    private void DeterminePlayerPosition()
    {
        foreach (Transform parent in GetComponentsInParent<Transform>(true))
        {
            if (parent.name.Contains("TopEndStep"))
            {
                isPlayerDown = true;
                break;
            }
        }
    }
    
    /// <summary>
    /// Handles artificial terrain button click
    /// </summary>
    public void OnClickArtificialTerrain()
    {
        if (CanActivate())
        {
            ActivateTerrainMode();
        }
    }

    private void ActivateTerrainMode()
    {
        board.artTerProcess = true;
        artTerUsed = true;
        artTerButtonImage.color = Color.clear;
        board.MainMenuUndo("Undo");
        HighlightEmptyCells();
    }

    private bool CanActivate()
    {
        return !isPlayerDown == board.turnPlayer &&
               board.connectionTable.Count == 0 &&
               !board.deleteProccess &&
               !board.moveProccess &&
               !board.healProccess &&
               !board.artTerProcess &&
               !artTerUsed &&
               !board.startStep &&
               EmptyCellAvailable();
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
    public void ResetArtTer()
    {
        artTerUsed = false;
        artTerButtonImage.color = Color.white;
    }
}
