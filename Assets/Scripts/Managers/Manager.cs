using System.Collections;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Handles game state management including win conditions and game over screen
/// </summary>
public class Manager : MonoBehaviour
{
    [Header("Game Configuration")]
    public int pointsToWin = 5;
    [Header("UI References")]
    public CanvasGroup gameOverScreen;
    public Image victoryImage;

    public Board board;

    

    
    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        gameOverScreen.alpha = 0f;
        gameOverScreen.interactable = false;
        gameOverScreen.blocksRaycasts = false;
        board.ResetGame();
        board.enabled = true;
    }

    private void Update()
    {
        CheckForGameEnd();
    }
    
    private void CheckForGameEnd()
    {
        if (CheckVictoryByPoints() || CheckBoardFullCondition())
        {
            return;
        }
    }
    

    private bool CheckVictoryByPoints()
    {
        if (board.topScoreBoard.victoryPoints >= pointsToWin)
        {
            GameOver(true);
            return true;
        }

        if (board.botScoreBoard.victoryPoints >= pointsToWin)
        {
            GameOver(false);
            return true;
        }

        return false;
    }

    private bool CheckBoardFullCondition()
    {
        if (board.cellsInUse < 64) return false;

        if (board.botScoreBoard.victoryPoints != board.topScoreBoard.victoryPoints)
        {
            GameOver(board.botScoreBoard.victoryPoints < board.topScoreBoard.victoryPoints);
            return true;
        }

        HandleDraw();
        return true;
    }

     private void HandleDraw()
    {
        board.enabled = false;
        SetGameOverScreenVisibility(true);
        victoryImage.sprite = board.draw;
    }

    private void SetGameOverScreenVisibility(bool show)
    {
        gameOverScreen.alpha = show ? 1f : 0f;
        gameOverScreen.interactable = show;
        gameOverScreen.blocksRaycasts = show;
    }
    public void GameOver(bool player1Wins)
    {
        board.enabled = false;
        SetGameOverScreenVisibility(true);
        victoryImage.sprite = player1Wins ? board.originSymP2 : board.originSymP1;
    }
}
