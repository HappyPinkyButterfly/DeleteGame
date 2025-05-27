using UnityEngine;

public class TopEndStep : MonoBehaviour
{
    public Board board;
    public MoveCell[] moveCells { get; set; }

    public CreateCell[] createCells { get; set; }
    public DestroyCell[] destroyCells { get; set; }

    private void Awake()
    {

        moveCells = GetComponentsInChildren<MoveCell>();
        Debug.Log($"Found {moveCells.Length} move cells in TopEndStep");
        foreach (MoveCell moveCell in moveCells)
        {
            Move move = Instantiate(board.movePrefab, moveCell.transform);
            move.transform.position = moveCell.transform.position;
        }

        createCells = GetComponentsInChildren<CreateCell>();
        foreach (CreateCell createCell in createCells)
        {
            Heal heal = Instantiate(board.healPrefab, createCell.transform);
            heal.transform.position = createCell.transform.position;
        }

        destroyCells = GetComponentsInChildren<DestroyCell>();
        foreach (DestroyCell destroyCell in destroyCells)
        {
            ArtificialTerrain artTer = Instantiate(board.artificialTerrainPrefab, destroyCell.transform);
            artTer.transform.position = destroyCell.transform.position;
        }
    }
    
    public bool AreCurrentPlayerActionsUsed()
    {
        Move[] allMoves = GetComponentsInChildren<Move>();
        Heal[] allHeals = GetComponentsInChildren<Heal>();
        ArtificialTerrain[] allArtTers = GetComponentsInChildren<ArtificialTerrain>();

        foreach (Move move in allMoves)
        {
            if (!move.moveUsed) 
            {
                return false;
            }
        }

        foreach (Heal heal in allHeals)
        {
            if (!heal.healUsed) 
            {
                return false;
            }
        }

        foreach (ArtificialTerrain artTer in allArtTers)
        {
            if (!artTer.artTerUsed) 
            {
                return false;
            }
        }

        return true; 
    }
}
