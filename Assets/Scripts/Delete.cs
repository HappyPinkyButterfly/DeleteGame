using UnityEngine;
using UnityEngine.UI;

public class Delete : MonoBehaviour
{
    private Board board;
    public Button deleteButton;

    public Image deleteButtonImage{get;set;}

    public bool deleteTurnPlayer {get;set;}

    private bool isPlayerDown;

    public bool deleteUsed = false;

    private void Awake()
    {
        board = GetComponentInParent<Board>();
        deleteTurnPlayer = false;
        deleteButton = GetComponent<Button>();
        deleteButtonImage = GetComponent<Image>();
        isPlayerDown = transform.parent.name.Contains("UpDeleteCell");
        if(!CheckParent())
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
        if(
        !isPlayerDown == board.turnPlayer 
        && board.connectionTable.Count == 0 
        && !board.deleteProccess
        && !deleteUsed
        && !board.topFirstMove
        && !board.botFirstMove
        && board.EnemyHasNormalSymbol())
        {
        board.deleteProccess = true;
        deleteButtonImage.sprite = board.emptyCell;
        ColorUtility.TryParseHtmlString("#B5A891", out Color novaBarva);
        deleteButtonImage.material = board.material;
        deleteUsed = true;
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
}
