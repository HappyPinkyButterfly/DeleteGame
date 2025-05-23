using UnityEngine;
using UnityEngine.UI;

public class Heal : MonoBehaviour
{
    public bool healProccess;
    public Board board { get; set; }
    public bool isPlayerDown { get; set; }
    public bool healUsed = false;
    public Image healButtonImage { get; set; }
    private float highlightAlpha = 0.965f;

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
        healButtonImage = GetComponent<Image>();

    }

    public void OnClickHeal()
    {
        Debug.Log(!isPlayerDown + "  /  " + board.turnPlayer);
        if (
            !isPlayerDown == board.turnPlayer &&
            board.connectionTable.Count == 0 &&
            !board.deleteProccess &&
            !board.moveProccess &&
            !board.healProccess &&
            !board.artTerProcess &&
            !healUsed &&
            !board.startStep &&
            TerrainOnField()
        )
        {
            board.healProccess = true;
            healUsed = true;
            healButtonImage.color = Color.clear;
            HighlightTerrainCells();
            
        
        }
    }

    public bool TerrainOnField()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
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
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            if (cell.state.occupation == 3) // Terren
            {
                // Nastavi 50% transparentnost
                Color cellColor = cell.buttonImage.color;
                cellColor.a = highlightAlpha;
                cell.buttonImage.color = cellColor;
            }
        }
    }


}
