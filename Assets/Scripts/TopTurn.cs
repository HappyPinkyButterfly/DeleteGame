using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;


public class TopTurn : MonoBehaviour
{
    public Board board;

    public Image imageTopTurn { get; set; }

    public TopEndStep topEndStep { get; set; }

    private void Awake()
    {
        imageTopTurn = GetComponent<Image>();
        topEndStep = board.GetComponentInChildren<TopEndStep>();
    }

    public void Update()
    {
        if (!board.turnPlayer && !board.startStep)
        {
            imageTopTurn.color = Color.yellow;
        }
        else if (!board.turnPlayer && board.startStep)
        {

            imageTopTurn.color = Color.green;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#A47A6B", out Color novaBarva);
            imageTopTurn.color = novaBarva;
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
