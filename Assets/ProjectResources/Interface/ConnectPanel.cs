using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Панелька с информацией о вершине
/// </summary>
public class ConnectPanel : MonoBehaviour
{
    public Vertex CurrentVertex;
    private Vertex selectVertex;
    public Text NameVertex;

    public void SetVertex(Vertex select, Vertex targetVert)
    {
        selectVertex = select;
        CurrentVertex = targetVert;
        NameVertex.text = CurrentVertex.gameObject.name;
    }

    /// <summary>
    /// Удалить соединение с этой вершиной
    /// </summary>
    public void DeleteConnect()
    {
        GraphManager.OnDeleteConnect(selectVertex, CurrentVertex);
    }

    /// <summary>
    /// Добавить соединенис с этой вершиной
    /// </summary>
    public void AddConnect()
    {
        GraphManager.OnAddConnect(selectVertex, CurrentVertex);
    }
}