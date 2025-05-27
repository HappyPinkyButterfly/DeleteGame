
using UnityEngine;
using UnityEngine.UI;
public class BotTurn : MonoBehaviour
{
    public Board board;

    public Image imageBotTurn {get;set;}
    public BotEndStep botEndStep {get;set;}

    private void Awake()
    {
        imageBotTurn = GetComponent<Image>();
        botEndStep = board.GetComponentInChildren<BotEndStep>();
    }

    public void Update()
    {
        if (board.turnPlayer && !board.startStep)
        {
            imageBotTurn.color = Color.yellow;
        }
        else if (board.turnPlayer && board.startStep)
        {

            imageBotTurn.color = Color.green;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#A47A6B", out Color novaBarva);
            imageBotTurn.color = novaBarva;
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
