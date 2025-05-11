using UnityEngine;

public class UpDelete : MonoBehaviour
{
    public Delete[] upDeletesAvailable;
    public Board board;

    private void Awake()
    {
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
            delete.deleteUsed = false;
            return;
           }
        }
    }
}
