using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    public Image targetImage {get;set;} 
    public void Awake()
    {
        targetImage = GetComponent<Image>();
    }
}
