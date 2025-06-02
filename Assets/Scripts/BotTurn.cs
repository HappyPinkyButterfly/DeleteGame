
using UnityEngine;
using UnityEngine.UI;
public class BotTurn : MonoBehaviour
{
    public Board board;

    public Image imageBotTurn {get;set;}
    public BotEndStep botEndStep {get;set;}
    public Sprite greenButton;
    public Sprite yellowButton;

    private void Awake()
    {
        imageBotTurn = GetComponent<Image>();
        botEndStep = board.GetComponentInChildren<BotEndStep>();
    }

    public void Update()
    {
        if (board.turnPlayer && !board.startStep)
        {
            imageBotTurn.sprite = yellowButton;
            imageBotTurn.color = Color.white;
        }
        else if (board.turnPlayer && board.startStep)
        {
            imageBotTurn.sprite = greenButton;
            imageBotTurn.color = Color.white;
        }
        else
        {
            imageBotTurn.color = Color.clear;
        }
        if (board.boardType)
        {
            if (!board.startStep
            && botEndStep.AreCurrentPlayerActionsUsed()
            && !board.moveProccess
            && !board.healProccess
            && !board.artTerProcess
            && board.turnPlayer
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
