using UnityEngine;

public class BotDelete : MonoBehaviour
{
    public DeleteForPrefab[] deleteCells{get;set;}
    public Delete[] deletesAvailable;
    public Board board;

    private void Awake()
    {
        deleteCells = GetComponentsInChildren<DeleteForPrefab>();
        foreach(DeleteForPrefab deleteCell in deleteCells)
        {
            CreateDeleteButton(deleteCell);   
        }
        deletesAvailable = GetComponentsInChildren<Delete>();
    }
    /// <summary>
    /// Creates delete buttons when awake
    /// </summary>
    public void CreateDeleteButton(DeleteForPrefab deleteCell)
    { 
        Delete delete = Instantiate(board.deletePrefab,deleteCell.transform);
        delete.transform.position = deleteCell.transform.position;
        delete.deleteButtonImage.material = board.material;
    }
    /// <summary>
    /// Returns one delete back upon achviving one point
    /// </summary>
    public void getOneDeleteBack()
    {
        foreach (Delete delete in deletesAvailable)
        {
            if (delete.deleteUsed)
            {
                delete.deleteButtonImage.sprite = board.deleteOriginSymP1;
                delete.deleteButtonImage.color = Color.white;
                delete.deleteButtonImage.material = null;
                delete.deleteUsed = false;
                return;
            }
        }
    }


}

