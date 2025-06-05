
using UnityEngine;
using UnityEngine.UI;


public class TopTurn : MonoBehaviour
{
    public Board board;

    public Image imageTopTurn { get; set; }

    public TopEndStep topEndStep { get; set; }

    public Sprite greenButton;
    public Sprite yellowButton;

    private void Awake()
    {
        imageTopTurn = GetComponent<Image>();
        topEndStep = board.GetComponentInChildren<TopEndStep>();
    }

    public void Update()
    {
        if (!board.turnPlayer && !board.startStep)
        {
            imageTopTurn.sprite = yellowButton;
            imageTopTurn.color = Color.white;
        }
        else if (!board.turnPlayer && board.startStep)
        {

            imageTopTurn.sprite = greenButton;
            imageTopTurn.color = Color.white;
        }
        else
        {
            imageTopTurn.color = Color.clear;
        }
        if (board.boardType)
        {
            if (!board.startStep
            && topEndStep.AreCurrentPlayerActionsUsed()
            && !board.moveProccess
            && !board.healProccess
            && !board.artTerProcess
            && !board.turnPlayer
            )
            {
                board.turnPlayer = !board.turnPlayer;
                board.startStep = true;
            }
        }

    }

    public void OnClick()
    {
        if (
            !board.deleteProccess &&
            !board.moveProccess &&
            !board.healProccess &&
            !board.artTerProcess &&
            !board.startStep
        )
        {
            board.turnPlayer = !board.turnPlayer;
            board.startStep = true;
        }
        
    }
}
