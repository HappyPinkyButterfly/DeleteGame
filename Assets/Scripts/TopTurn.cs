using UnityEngine;
using UnityEngine.UI;


public class TopTurn : MonoBehaviour
{
    public Board board;

    public Image imageTopTurn {get;set;}

    private void Awake()
    {
        imageTopTurn = GetComponent<Image>();
    }

    public void Update()
    {
        if(!board.turnPlayer && !board.deleteProccess)
        {
            imageTopTurn.color = Color.green;
        }
        else if (!board.turnPlayer && board.deleteProccess)
        {

           imageTopTurn.color = Color.yellow; 
        }
        else
        {
            ColorUtility.TryParseHtmlString("#A47A6B", out Color novaBarva);
            imageTopTurn.color = novaBarva;
        }
    }
}
