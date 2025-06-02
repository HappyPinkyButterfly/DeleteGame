
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class TopScoreBoard : MonoBehaviour
{

    private Point[] points;

    public Sprite vp;

    public int victoryPoints { get; set; }

    private void Awake()

    {
        points = GetComponentsInChildren<Point>();
        victoryPoints = 0;
    }

    public void AddVictoryPointTop()
    {

        for (int i = 0; i <= victoryPoints; i++)
        {
            points[i].targetImage.sprite = vp;
            points[i].targetImage.color = Color.white;
        }
        victoryPoints++;
    }
    public void ResetPoints()
    {
        victoryPoints = 0;
        Point[] points = GetComponentsInChildren<Point>();
        foreach (Point point in points)
        {

            point.targetImage.color = Color.clear;
        }
    }


}


