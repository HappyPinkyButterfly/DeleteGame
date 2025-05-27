
using UnityEngine;
using UnityEngine.UI;

public class Delete : MonoBehaviour
{
    private Board board;
    public Button deleteButton;

    public Image deleteButtonImage { get; set; }

    private bool isPlayerDown;

    public bool deleteUsed = false;

    private void Awake()
    {
        board = GetComponentInParent<Board>();
        deleteButton = GetComponent<Button>();
        deleteButtonImage = GetComponent<Image>();
        isPlayerDown = transform.parent.name.Contains("UpDeleteCell");
        if (!CheckParent())
        {
            deleteButtonImage.sprite = board.deleteOriginSymP1;
        }
        else
        {
            deleteButtonImage.sprite = board.deleteOriginSymP2;
        }
    }
    public void DeleteCell()
    {
        Debug.Log(isPlayerDown + " | " + board.turnPlayer);
        if (
        !isPlayerDown == board.turnPlayer
        && board.connectionTable.Count == 0
        && !board.deleteProccess
        && !deleteUsed
        && !board.topFirstMove
        && !board.botFirstMove
        && board.startStep
        && board.EnemyHasNormalSymbol())
        {
            board.deleteProccess = true;
            deleteUsed = true;
            deleteButtonImage.color = new Color(1, 1, 1, 0);
            HighlightEnemyBasicSymbols();
        }
    }



    private bool CheckParent()
    {
        Transform parent = transform.parent;

        if (parent.name.Contains("UpDelete"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetDelete()
    {
        deleteUsed = false;
        deleteButtonImage.color = Color.white;
        deleteButtonImage.material = null;

        // Ponastavi pravilen sprite glede na igralca
        if (!CheckParent()) // Za spodnjega igralca
        {
            deleteButtonImage.sprite = board.deleteOriginSymP1;
        }
        else // Za zgornjega igralca
        {
            deleteButtonImage.sprite = board.deleteOriginSymP2;
        }
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

