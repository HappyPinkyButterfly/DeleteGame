using UnityEngine;

public class UpDelete : MonoBehaviour
{
    public DeleteForPrefab[] deleteCells{get;set;}
    public Delete[] upDeletesAvailable;
    public Board board;

    private void Awake()
    {
        deleteCells = GetComponentsInChildren<DeleteForPrefab>();
        foreach(DeleteForPrefab deleteCell in deleteCells)
        {
            Delete delete = Instantiate(board.deletePrefab,deleteCell.transform);
            delete.transform.position = deleteCell.transform.position;
            delete.deleteButtonImage.material = board.material;
            
            
        }
        upDeletesAvailable = GetComponentsInChildren<Delete>();
    }

    public void getOneDeleteBack()
    {
        foreach(Delete delete in upDeletesAvailable)
        {
           if(delete.deleteUsed)
           {
            delete.deleteButtonImage.sprite = board.deleteOriginSymP2;
            delete.deleteButtonImage.color = Color.white;
            delete.deleteButtonImage.material = null;
            delete.deleteUsed = false;
            return;
           }
        }
    }
}