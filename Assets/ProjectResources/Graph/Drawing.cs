using System.Collections.Generic;
using UnityEngine;

public class Drawing
{
    public static void Draw(List<Vertex> pathComplete)
    {
        pathComplete.Reverse();
        for (int i = 0; i < pathComplete.Count - 1; i++)
        {
            ConnectingTunnel line = pathComplete[i].allConnect.Find(x => x.endPoint.gameObject.name == pathComplete[i + 1].gameObject.name);
            line.lineRenderer.startColor = Color.yellow;
            line.lineRenderer.endColor = Color.yellow;
        }
    }

    public static void ResetLine(List<Vertex> vertexs)
    {
        for (int i = 0; i < vertexs.Count; i++)
        {
            vertexs[i].ResetColor();
        }
    }
}