using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;


public class TopTurn : MonoBehaviour
{
    public Board board;

    public Image imageTopTurn { get; set; }

    private void Awake()
    {
        imageTopTurn = GetComponent<Image>();
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

    }

    public void OnClick()
    {
        board.turnPlayer = !board.turnPlayer;
        board.startStep = true;
    }
}
