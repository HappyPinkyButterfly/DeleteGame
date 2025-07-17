
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BotTurn : MonoBehaviour
{
    [Header("Dependencies")]
    public Board board;
    public BotEndStep botEndStep { get; set; }

    [Header("UI Elements")]
    public Image imageBotTurn { get; set; }
    
    public Sprite greenButton;
    public Sprite yellowButton;
    

    private void Awake()
    {
        imageBotTurn = GetComponent<Image>();
        botEndStep = board.GetComponentInChildren<BotEndStep>();
    }

    public void Update()
    {
        UpdateTurnIndicator();
        CheckForAutoTurnSwitch();
    }

    /// <summary>
    /// Updates the visual indicator for bot's turn state
    /// </summary>
    private void UpdateTurnIndicator()
    {
        if (!board.turnPlayer)
        {
            imageBotTurn.color = Color.clear;
            return;
        }

        imageBotTurn.sprite = board.startStep ? greenButton : yellowButton;
        imageBotTurn.color = Color.white;
    }

    /// <summary>
    /// Checks conditions for automatic turn switching
    /// </summary>
    private void CheckForAutoTurnSwitch()
    {
        if (board.boardType && 
            !board.startStep &&
            botEndStep.AreCurrentPlayerActionsUsed() &&
            !board.moveProccess &&
            !board.healProccess &&
            !board.artTerProcess &&
            board.turnPlayer)
        {
            SwitchTurn();
        }
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
