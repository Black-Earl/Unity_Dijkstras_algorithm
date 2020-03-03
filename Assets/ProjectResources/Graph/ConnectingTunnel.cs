using UnityEngine;

/// <summary>
/// Хранит данные о соедененных точек
/// </summary>
[System.Serializable]
public class ConnectingTunnel
{
    public Vertex endPoint;

    public LineRenderer lineRenderer;
}