
using UnityEngine;
using UnityEngine.UI;
public class BotTurn : MonoBehaviour
{
    public Board board;

    public Image imageBotTurn { get; set; }

    private void Awake()
    {
        imageBotTurn = GetComponent<Image>();
    }

    public void Update()
    {
        if (board.turnPlayer && !board.deleteProccess)
        {
            imageBotTurn.color = Color.green;
        }
        else if (board.turnPlayer && board.deleteProccess)
        {
            imageBotTurn.color = Color.yellow;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#A47A6B", out Color novaBarva);
            imageBotTurn.color = novaBarva;
        }
    }
    public void OnClick()
    {
        board.turnPlayer = !board.turnPlayer;
    }
}
