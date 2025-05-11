using UnityEngine;

public class Row : MonoBehaviour
{
    public CellForPrefab[] cells {get; set;}

    private void Awake()
    {
        cells = GetComponentsInChildren<CellForPrefab>();   
    }
}
