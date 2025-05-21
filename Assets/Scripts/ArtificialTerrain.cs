using UnityEngine;
using UnityEngine.UI;

public class ArtificialTerrain : MonoBehaviour
{
    public bool artTerProcess;
    public Board board { get; set; }
    public bool isPlayerDown { get; set; }
    public bool artTerUsed = false;
    public Image artTerButtonImage { get; set; }

    public void Awake()
    {
        board = GetComponentInParent<Board>();
        isPlayerDown = transform.parent.name.Contains("TopCreate");
        artTerButtonImage = GetComponent<Image>();

    }

    public void OnClickArtificialTerrain()
    {
        if (
             !isPlayerDown == board.turnPlayer &&
             board.connectionTable.Count == 0 &&
             !board.deleteProccess &&
             !board.moveProccess &&
             !board.healProccess &&
             !board.artTerProcess &&
             !artTerUsed &&
             EmptyCellAvailable()

         )
        {
            board.artTerProcess = true;
            artTerUsed = true;
            artTerButtonImage.color = Color.clear;
        }
    }
    public bool EmptyCellAvailable()
    {
        return true;
    }
}
