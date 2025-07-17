using UnityEngine;

public class BotEndStep : MonoBehaviour
{
    public Board board;
    public MoveCell[] moveCells { get; set; }
    public CreateCell[] createCells { get; set; }
    public DestroyCell[] destroyCells { get; set; }

    private void Awake()
    {
        InitializeMoveCells();
        InitializeHealCells();
        InitializeArtTerCells();
    
    }
    private void InitializeMoveCells()
    { 
        moveCells = GetComponentsInChildren<MoveCell>();
        foreach (MoveCell moveCell in moveCells)
        {
            Move move = Instantiate(board.movePrefab, moveCell.transform);
            move.transform.position = moveCell.transform.position;
        }
    }

    private void InitializeHealCells()
    { 
        createCells = GetComponentsInChildren<CreateCell>();
        foreach (CreateCell createCell in createCells)
        {
            
            Heal heal = Instantiate(board.healPrefab, createCell.transform);
            heal.transform.position = createCell.transform.position;
        }
    }

    private void InitializeArtTerCells()
    { 
        destroyCells = GetComponentsInChildren<DestroyCell>();
        foreach (DestroyCell destroyCell in destroyCells)
        {
            ArtificialTerrain artTer = Instantiate(board.artificialTerrainPrefab, destroyCell.transform);
            artTer.transform.position = destroyCell.transform.position;
        }
    }
    
    /// <summary>
    /// Checks if all current player actions (move, heal, artificial terrain) have been used
    /// </summary>
    /// <returns>True if all actions are used, false otherwise</returns>
    public bool AreCurrentPlayerActionsUsed()
    {
        return AreAllMovesUsed() && 
               AreAllHealsUsed() && 
               AreAllArtificialTerrainsUsed();
    }

    private bool AreAllMovesUsed()
    {
        Move[] allMoves = GetComponentsInChildren<Move>();
        foreach (Move move in allMoves)
        {
            if (!move.moveUsed) 
                return false;
        }
        return true;
    }

    private bool AreAllHealsUsed()
    {
        Heal[] allHeals = GetComponentsInChildren<Heal>();
        foreach (Heal heal in allHeals)
        {
            if (!heal.healUsed) 
                return false;
        }
        return true;
    }

    private bool AreAllArtificialTerrainsUsed()
    {
        ArtificialTerrain[] allArtTers = GetComponentsInChildren<ArtificialTerrain>();
        foreach (ArtificialTerrain artTer in allArtTers)
        {
            if (!artTer.artTerUsed) 
                return false;
        }
        return true;
    }
}
