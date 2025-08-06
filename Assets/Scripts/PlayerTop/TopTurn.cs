using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopTurn : MonoBehaviour
{
    [Header("Dependencies")]
    public Board board;
    public TopEndStep topEndStep { get; set; }

    [Header("UI Components")]
    public Image imageTopTurn { get; set; }
    public Sprite greenButton;
    public Sprite yellowButton;

    private void Awake()
    {
        imageTopTurn = GetComponent<Image>();
        topEndStep = board.GetComponentInChildren<TopEndStep>();
    }

    public void Update()
    {
        UpdateTurnIndicator();
        CheckAutoTurnSwitch();
    }
    /// <summary>
    /// Updates the visual indicator for the top player's turn state
    /// </summary>
    private void UpdateTurnIndicator()
    {
        if (board.turnPlayer)
        {
            imageTopTurn.color = Color.clear;
            return;
        }

        imageTopTurn.sprite = board.startStep ? greenButton : yellowButton;
        imageTopTurn.color = Color.white;
    }

    /// <summary>
    /// Checks conditions for automatic turn switching
    /// </summary>
    private void CheckAutoTurnSwitch()
    {
        if (ShouldAutoSwitchTurn())
        {
            SwitchTurn();
        }
    }

    private bool ShouldAutoSwitchTurn()
    {
        return board.boardType && 
               !board.startStep &&
               topEndStep.AreCurrentPlayerActionsUsed() &&
               !board.moveProccess &&
               !board.healProccess &&
               !board.artTerProcess &&
               !board.turnPlayer;
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
    }
}
