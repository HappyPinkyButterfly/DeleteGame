
using UnityEngine;

public class TopScoreBoard : MonoBehaviour
{
    public Board board;

    private Point[] points;

    private Color victoryPointColor;

    public int victoryPoints{get;set;}

    private void Awake()

    {
        points = GetComponentsInChildren<Point>();
        victoryPointColor = Color.white;
        victoryPoints = 0;
    }

    public void AddVictoryPointTop()
    {
        
        for(int i = 0; i <= victoryPoints; i++)
        {
            points[i].targetImage.color = victoryPointColor;
        }
        victoryPoints++;
    }
    public void ResetPoints()
    {
        victoryPoints = 0;
        Point[] points = GetComponentsInChildren<Point>();
        foreach (Point point in points)
        {
            ColorUtility.TryParseHtmlString("#DBC8AA", out Color novaBarva);
            point.targetImage.color = novaBarva;
        }
    }


}


