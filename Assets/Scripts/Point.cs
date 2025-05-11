using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    public Image targetImage {get;set;} 
    public void Awake()
    {
        targetImage = GetComponent<Image>();
        ColorUtility.TryParseHtmlString("#DBC8AA", out Color novaBarva);
        targetImage.color = novaBarva;
    }
}
