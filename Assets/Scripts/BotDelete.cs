using UnityEngine;

public class BotDelete : MonoBehaviour
{
    public Delete[] botDeletesAvailable;
    public Board board;

    private void Awake()
    {
        botDeletesAvailable = GetComponentsInChildren<Delete>();
        
    }

    public void getOneDeleteBack()
    {
        foreach(Delete delete in botDeletesAvailable)
        {
           if(delete.deleteUsed)
           {
            delete.deleteButtonImage.sprite = board.deleteOriginSymP1;
            delete.deleteButtonImage.color = Color.white;
            delete.deleteUsed = false;
            return;
           }
        }
    }
}
