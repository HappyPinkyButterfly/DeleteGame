
using UnityEngine;
using UnityEngine.UI;


public class Cell : MonoBehaviour
{

   private Board board;

   public CellState state {get;  set;}

   public Button button;
   public Image buttonImage{get;set;}
   public Vector2Int location {get; set;}   

    public void Awake()
    {
        board = FindFirstObjectByType<Board>();
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = board.emptyCell;
        state = gameObject.AddComponent<CellState>();
        
    }

    public void CellClick()
    {
        if (!board.deleteProccess)
        {
            if (state.occupation == 0 && board.connectionTable.Count == 0)
            {
                // Prvi potezi za obe strani
                if (board.turnPlayer && board.topFirstMove)
                {
                    buttonImage.sprite = board.originSymP1;
                    state.symbolOwner = true;
                    state.occupation = 2;
                    board.topFirstMove = false;
                    board.turnPlayer = !board.turnPlayer;
                    return;
                }
                else if (!board.turnPlayer && board.botFirstMove)
                {
                    buttonImage.sprite = board.originSymP2;
                    state.symbolOwner = false;
                    state.occupation = 2;
                    board.botFirstMove = false;
                    board.turnPlayer = !board.turnPlayer;
                
                    return;
                }

                // Normalne poteze
                if (board.turnPlayer)
                {
                    buttonImage.sprite = board.basicSymP1;
                    state.symbolOwner = true;
                }
                else
                {
                    buttonImage.sprite = board.basicSymP2;
                    state.symbolOwner = false;
                }
                state.occupation = 1;
                board.turnPlayer = !board.turnPlayer;
            }
            else if ((state.occupation == 1 || state.occupation == 2) && board.turnPlayer == state.symbolOwner)
            {
                if(!board.connectionTable.Contains(this))
                {
                    board.connectionTable.Add(this);
                    buttonImage.color = Color.green;

                    if(board.connectionTable.Count == 3)
                    {
                        if(board.CheckForConnection())
                        {   
                            if(!board.turnPlayer)
                            {
                                board.topScoreBoard.AddVictoryPointTop();
                                board.upDelete.getOneDeleteBack();
                            }
                            else
                            {
                                board.botScoreBoard.AddVictoryPointBot();
                                board.botDelete.getOneDeleteBack();         
                            }
                            
                            board.SuccessfulConnection();
                        }
                        else
                        {
                            board.UnsuccessfulConnection();
                        }
                    }
                }
                else
                {
                    buttonImage.color = Color.white;
                    board.connectionTable.Remove(this);
                }
            }
        }
        else if (state.occupation == 1 && board.turnPlayer != state.symbolOwner && board.connectionTable.Count == 0)
        {
        // Brisanje nasprotnikovega znaka
        if (board.turnPlayer)
        {
            buttonImage.sprite = board.originSymP1;
            board.botDelete.HideUsedDelete();
            
        }
        else
        {
            buttonImage.sprite = board.originSymP2;
            board.upDelete.HideUsedDelete();
            
        }
        state.occupation = 2;
        board.turnPlayer = !board.turnPlayer;
        state.symbolOwner = !state.symbolOwner;
        board.deleteProccess = false;
            
        }
    }
    public void ResetCell()
    {
        buttonImage.sprite = board.emptyCell;
        buttonImage.color = Color.white;
        state.occupation = 0;
    }
}
