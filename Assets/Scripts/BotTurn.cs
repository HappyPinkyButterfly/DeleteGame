
using UnityEngine;
using UnityEngine.UI;
public class BotTurn : MonoBehaviour
{
    public Board board;

    public Image imageBotTurn {get;set;}

    private void Awake()
    {
        imageBotTurn = GetComponent<Image>();
    }

    public void Update()
    {
        if(board.turnPlayer)
        {
            imageBotTurn.color = Color.yellow;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#A47A6B", out Color novaBarva);
            imageBotTurn.color = novaBarva;
        }
    }
}
