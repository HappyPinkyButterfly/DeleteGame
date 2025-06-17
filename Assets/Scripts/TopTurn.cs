
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TopTurn : MonoBehaviour
{
    public Board board;

    public Image imageTopTurn { get; set; }

    public TopEndStep topEndStep { get; set; }

    public Sprite greenButton;
    public Sprite yellowButton;

    public TextMeshProUGUI timer;
    public TopScoreBoard score;
    public CanvasGroup timerVisible;

    private float timeRemaining;
    private bool isTimerRunning;
    private const int baseTime = 10; // Base 10 seconds
    private const int bonusPerPoint = 5; // 5 seconds per victory point

    private void Awake()
    {
        imageTopTurn = GetComponent<Image>();
        topEndStep = board.GetComponentInChildren<TopEndStep>();
    }

    public void Update()
    {
        if (!board.turnPlayer && !board.startStep)
        {
            imageTopTurn.sprite = yellowButton;
            imageTopTurn.color = Color.white;
        }
        else if (!board.turnPlayer && board.startStep)
        {

            imageTopTurn.sprite = greenButton;
            imageTopTurn.color = Color.white;
        }
        else
        {
            imageTopTurn.color = Color.clear;
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
                SwitchTurn();
            }
        }

        if (board.boardType)
        {
            if (!board.turnPlayer)
            {
                timerVisible.alpha = 1;
                if (isTimerRunning)
                {
                    timeRemaining -= Time.deltaTime;

                    // Only update display when integer second changes
                    if (Mathf.FloorToInt(timeRemaining) != Mathf.FloorToInt(timeRemaining + Time.deltaTime))
                    {
                        UpdateTimerDisplay();
                    }

                    if (timeRemaining <= 0)
                    {
                        // Time's up - switch turns
                        isTimerRunning = false;
                        if (!board.turnPlayer) // Only auto-switch if it's still this player's turn
                        {
                            SwitchTurn();
                        }
                    }
                }

                // Switch turn when time reaches 0
                if (timeRemaining <= 0)
                {
                    timeRemaining = 0;
                    UpdateTimerDisplay();
                    SwitchTurn();
                }

            }
            else
            {
                timerVisible.alpha = 0f;
            }
        }
        else
        {
            timerVisible.alpha = 0f;
        }


    }


    public void ResetTimer()
    {
        // Calculate time based on victory points: 10s + 5s per point
        timeRemaining = baseTime + (score.victoryPoints * bonusPerPoint);
        isTimerRunning = true;
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        timer.text = Mathf.CeilToInt(timeRemaining).ToString();
    }



    private void OnEnable()
    {
        ResetTimer();
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
            SwitchTurn();
        }
    }

    public void SwitchTurn()
    {
        if (board.artTerProcess || board.deleteProccess || board.healProccess || board.moveProccess)
        {
            board.CancelAllProcesses();
        }

        board.turnPlayer = !board.turnPlayer;
        board.startStep = true;
        ResetTimer();
    }
}
