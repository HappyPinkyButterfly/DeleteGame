using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Board board;

    public Image victoryImage;

    public int pointsToWin = 5;

    public CanvasGroup gameOverScreen;
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
        if (board.topScoreBoard.victoryPoints == pointsToWin)
            GameOver(true);
        else if (board.botScoreBoard.victoryPoints == pointsToWin)
            GameOver(false);
        if(board.cellsInUse == 64)
        {
          if(board.botScoreBoard.victoryPoints < board.topScoreBoard.victoryPoints)
          {
            GameOver(true);
          }
          else if(board.botScoreBoard.victoryPoints > board.topScoreBoard.victoryPoints)
          {
            GameOver(false);
          }
          else
          {
            board.enabled = false;
            gameOverScreen.alpha = 1f;
            gameOverScreen.interactable = true;
            gameOverScreen.blocksRaycasts = true;
            victoryImage.sprite = board.draw;
          }
        }

    }

    public void GameOver(bool player1Wins)
    {
        board.enabled = false;
        gameOverScreen.alpha = 1f;
        gameOverScreen.interactable = true;
        gameOverScreen.blocksRaycasts = true;   
        if(player1Wins)
        {
            victoryImage.sprite = board.originSymP2;
        }
        else
        {
            victoryImage.sprite = board.originSymP1;
        }
        //StartCoroutine(Fade(gameOverScreen,1f,1f));
        
    }

    private IEnumerator Fade(CanvasGroup gameOverScreen, float to, float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;
        float from = gameOverScreen.alpha;

        while (elapsed < duration)
        {
           gameOverScreen.alpha = Mathf.Lerp(from,to,elapsed/duration);
           elapsed += Time.deltaTime;
           yield return null;
        }
        
        gameOverScreen.alpha = to;
    }
}
