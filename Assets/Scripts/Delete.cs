using UnityEngine;
using UnityEngine.UI;

public class Delete : MonoBehaviour
{
    public Board board;
    public Button deleteButton;



    public Image deleteButtonImage{get;set;}

    public bool deleteTurnPlayer {get;set;}

    private bool isPlayerDown;

    public bool deleteUsed = false;

    private void Awake()
    {
        deleteTurnPlayer = false;
        deleteButton = GetComponent<Button>();
        deleteButtonImage = GetComponent<Image>();
        isPlayerDown = transform.parent.name.Contains("UpDelete");
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
        && board.field.EnemyHasNormalSymbol())
        {
        board.deleteProccess = true;
        deleteButtonImage.sprite = board.cover;
        ColorUtility.TryParseHtmlString("#DBC8AA", out Color novaBarva);
        deleteButtonImage.color = novaBarva;
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
