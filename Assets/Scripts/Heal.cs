using UnityEngine;
using UnityEngine.UI;

public class Heal : MonoBehaviour
{
    public bool healProccess;
    public Board board { get; set; }
    public bool isPlayerDown { get; set; }
    public bool healUsed = false;
    public Image healButtonImage { get; set; }

    public void Awake()
    {
        board = GetComponentInParent<Board>();
        isPlayerDown = transform.parent.name.Contains("TopDestroy");
        healButtonImage = GetComponent<Image>();

    }

    public void OnClickHeal()
    {
        Debug.Log(!isPlayerDown + "  /  " + board.turnPlayer);
        if (
            !isPlayerDown == board.turnPlayer &&
            board.connectionTable.Count == 0 &&
            !board.deleteProccess &&
            !board.moveProccess &&
            !board.healProccess &&
            !board.artTerProcess &&
            !healUsed &&
            TerrainOnField()
        )
        {
            board.healProccess = true;
            healUsed = true;
            healButtonImage.color = Color.clear;
        }
    }

    public bool TerrainOnField()
    {
        return true;
     }
}
