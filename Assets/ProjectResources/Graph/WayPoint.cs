using UnityEngine;

/// <summary>
/// Вершина вхадящая в путь до конечной точки
/// </summary>
public class WayPoint : MonoBehaviour
{
    public float Weight;

    public Vertex prevPoint;
}