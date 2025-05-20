using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    public bool moveProccess;
    public Board board{get;set;}
    private bool isPlayerDown { get; set; }
    public bool moveUsed = false;
    public Image moveButtonImage{get;set;}

    public void Awake()
    {
        board = GetComponentInParent<Board>();
        isPlayerDown = transform.parent.name.Contains("TopEndStep");
        moveButtonImage = GetComponent<Image>();

    }
    public void OnMoveClick()
    {
        if (
            //!isPlayerDown == board.turnPlayer &&
//board.connectionTable.Count == 0 &&
            !board.deleteProccess &&
            !board.moveProccess &&
            !moveUsed &&
            !board.topFirstMove &&
            !board.botFirstMove &&
            YouHaveSymbol()
        )
        {
            moveProccess = true;
            moveUsed = true;
            Debug.Log("YES");
            moveButtonImage.color = Color.clear;


        }

    }
    public bool YouHaveSymbol()
    {
        return true;
    }
}
