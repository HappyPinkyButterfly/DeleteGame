using UnityEngine;

public class TopEndStep : MonoBehaviour
{
    public Board board;
    public MoveCell[] moveCells{get;set;}

    private void Awake()
    {
        moveCells = GetComponentsInChildren<MoveCell>();
        foreach (MoveCell moveCell in moveCells)
        {
            Move move = Instantiate(board.movePrefab, moveCell.transform);
            move.transform.position = moveCell.transform.position;
        }
    }
}
