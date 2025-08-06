using UnityEngine;
using UnityEngine.UI;

public class Heal : MonoBehaviour
{
    [Header("State")]
    
    public bool isPlayerDown { get; set; }
    public bool healUsed = false;
    
    [Header("Settings")]
    [SerializeField] private float highlightAlpha = 0.965f;

    [Header("References")]
    public Board board { get; set; }
    public Image healButtonImage { get; set; }

    public void Awake()
    {
        board = GetComponentInParent<Board>();
        healButtonImage = GetComponent<Image>();
        DeterminePlayerPosition();
    }

    private void DeterminePlayerPosition()
    { 
        isPlayerDown = false;
        foreach (Transform parent in GetComponentsInParent<Transform>(true))
        {
            if (parent.name.Contains("TopEndStep"))
            {
                isPlayerDown = true;
                break;
            }
        }
    }

    public void OnClickHeal()
    {
        if (CanActivateHeal())
        {
            ActivateHeal();


        }
    }

    private bool CanActivateHeal()
    {
        return !isPlayerDown == board.turnPlayer &&
               board.connectionTable.Count == 0 &&
               !board.deleteProccess &&
               !board.moveProccess &&
               !board.healProccess &&
               !board.artTerProcess &&
               !healUsed &&
               !board.startStep &&
               TerrainOnField();
    }
    private void ActivateHeal()
    {
        board.healProccess = true;
        healUsed = true;
        healButtonImage.color = Color.clear;
        board.MainMenuUndo("Undo");
        HighlightTerrainCells();
    }

    

    public bool TerrainOnField()
    {
        foreach (Cell cell in FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (cell.state.occupation == 3)
            {
                return true;
            }
        }
        return false;
    }

    private void HighlightTerrainCells()
    {
        foreach (Cell cell in FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (cell.state.occupation == 3) // Terren
            {
                Color cellColor = cell.buttonImage.color;
                cellColor.a = highlightAlpha;
                cell.buttonImage.color = cellColor;
            }
        }
    }
    public void ResetHeal()
    {
        healUsed = false;
        healButtonImage.color = Color.white;
    }


}
