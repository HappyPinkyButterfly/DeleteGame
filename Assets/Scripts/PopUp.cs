using UnityEngine;

public class PopUp : MonoBehaviour
{
    // Originalne spremenljivke (ohranjene)
    public Board board;
    public CanvasGroup popUp;
    public CanvasGroup popUpBackGround;

    private void Awake()
    {
        HidePopUp();
    }

    private void HidePopUp()
    {
        SetPopUpVisibility(false);
        SetBackgroundVisibility(false);
    }
    private void ShowPopUp()
    {
        SetPopUpVisibility(true);
        SetBackgroundVisibility(true);
    }

    private void SetPopUpVisibility(bool show)
    {
        popUp.alpha = show ? 1f : 0f;
        popUp.blocksRaycasts = show;
        popUp.interactable = show;
    }

    private void SetBackgroundVisibility(bool show)
    {
        popUpBackGround.alpha = show ? 1f : 0f;
        popUpBackGround.blocksRaycasts = show;
        popUpBackGround.interactable = show;
    }

    public void MainMenuClick()
    {
        if (board.menuButtonState)
        {
            ShowMainMenu();
        }
        else
        {
            HideMainMenu();
        }
    }

    private void ShowMainMenu()
    {
        board.enabled = true;
        ShowPopUp();
    }

    private void HideMainMenu()
    {
        board.CancelAllProcesses();
        board.MainMenuUndo("MainMenu");
    }

    public void NoClick()
    {
        board.enabled = false;
        HidePopUp();
    }
}