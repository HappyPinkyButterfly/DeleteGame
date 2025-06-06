using UnityEngine;

public class PopUp : MonoBehaviour
{

    public Board board;
    public CanvasGroup popUp;

    public CanvasGroup popUpBackGround;

    public void Awake()
    {
        popUp.alpha = 0f;
        popUp.blocksRaycasts = false;
        popUp.interactable = false;

        popUpBackGround.alpha = 0f;
        popUpBackGround.blocksRaycasts = false;
        popUpBackGround.interactable = false;
    }

    public void MainMenuClick()
    {
        board.enabled = true;
        popUp.alpha = 1f;
        popUp.blocksRaycasts = true;
        popUp.interactable = true;

        popUpBackGround.alpha = 1f;
        popUpBackGround.blocksRaycasts = true;
        popUpBackGround.interactable = true;
    }

    public void NoClick()
    {
        board.enabled = false;
        popUp.alpha = 0f;
        popUp.blocksRaycasts = false;
        popUp.interactable = false;

        popUpBackGround.alpha = 0f;
        popUpBackGround.blocksRaycasts = false;
        popUpBackGround.interactable = false;
    }
}
