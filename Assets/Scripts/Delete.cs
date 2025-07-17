
using UnityEngine;
using UnityEngine.UI;

public class Delete : MonoBehaviour
{
    [Header("References")]
    private Board board;
    [Header("UI")]
    public Button deleteButton;
    public Image deleteButtonImage { get; set; }

    [Header("State")]
    public bool isPlayerDown;
    public bool deleteUsed = false;

    private void Awake()
    {
        board = GetComponentInParent<Board>();
        deleteButton = GetComponent<Button>();
        deleteButtonImage = GetComponent<Image>();
        SetupDeleteButton();
    }
    public void DeleteCell()
    {
        if (CanDeleteCell())
        {
            ExecuteDelete();
        }
    }
    private void SetupDeleteButton()
    {
        isPlayerDown = !IsUpDeleteCell();
        UpdateButtonAppearance();
        deleteButtonImage.color = Color.white;
    }

    private bool CanDeleteCell()
    {
        return !isPlayerDown == board.turnPlayer &&
               board.connectionTable.Count == 0 &&
               !board.deleteProccess &&
               !deleteUsed &&
               !board.topFirstMove &&
               !board.botFirstMove &&
               board.startStep &&
               board.EnemyHasNormalSymbol();
    }

    private void ExecuteDelete()
    {
        board.deleteProccess = true;
        deleteUsed = true;
        deleteButtonImage.color = Color.clear;
        board.MainMenuUndo("Undo");
        HighlightEnemyBasicSymbols();
    }

    public void ResetDelete()
    {
        deleteUsed = false;
        deleteButtonImage.color = Color.white;
        deleteButtonImage.material = null;
        UpdateButtonAppearance();
    }
    
    private void UpdateButtonAppearance()
    {
        deleteButtonImage.sprite = IsUpDeleteCell() ? 
            board.deleteOriginSymP1 : 
            board.deleteOriginSymP2;
    }

    private bool IsUpDeleteCell()
    {
        return !transform.parent.name.Contains("UpDelete");
    }

    private void HighlightEnemyBasicSymbols()
    {
        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Cell cell in allCells)
        {
            if (cell.state.occupation == 1 && cell.state.symbolOwner != board.turnPlayer)
            {
                cell.buttonImage.color = Color.green; // Highlight in green
            }
            else
            {
                cell.buttonImage.color = Color.white; // Reset others
            }
        }
    }
}

