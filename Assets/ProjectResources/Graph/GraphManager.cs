using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Менеджер графа
/// </summary>
public class GraphManager : MonoBehaviour
{
    private const string NAME_VERTEX = "Vertex_";
    public static Action<Vertex, Vertex> OnAddConnect;
    public static Action<Vertex, Vertex> OnDeleteConnect;

    public static Action<Vertex> OnSetStartVertex;
    public static Action<Vertex> OnSetEndVertex;

    public static Action<List<Vertex>> onResultPath;

    public static Action OnFindPath;

    [Header("Стартовая вершина")]
    public Vertex StartVertex;

    [Header("Конечная вершина")]
    public Vertex EndVertex;

    [Header("Список всех вершин")]
    public List<Vertex> AllVertexs;
    [Header("Цвет стартовой вершины")]
    public Color startColor;
    [Header("Цвет промежуточной вершины")]
    public Color neutralColor;

    [Header("Цвет конечной вершины")]
    public Color endColor;

    [Header("Ручная установка веса")]
    public bool IsCustomSetWeight;

    private void Start()
    {
        if (StartVertex != null)
        {
            SetVertexColor(StartVertex, startColor);
        }

        if (EndVertex != null)
        {
            SetVertexColor(EndVertex, endColor);
        }
        Subscribe();
        InitVertex();
    }

    private void Subscribe()
    {
        InputController.OnUpdatePosition += EndMoveVertex;
        ObjManager.OnCreateVertex += AddNewVertex;
        OnAddConnect += AddConnect;
        OnDeleteConnect += DeleteConnect;
        InterfaceManager.DeleteVertex += DeleteVertex;
        OnSetStartVertex += SetStartVertex;
        OnSetEndVertex += SetEndVertex;
        OnFindPath += StartFindPath;
    }

    private void UnSubscribe()
    {
        InputController.OnUpdatePosition -= EndMoveVertex;
        ObjManager.OnCreateVertex -= AddNewVertex;
        OnAddConnect -= AddConnect;
        OnDeleteConnect -= DeleteConnect;
        InterfaceManager.DeleteVertex -= DeleteVertex;
        OnSetStartVertex -= SetStartVertex;
        OnSetEndVertex -= SetEndVertex;
        OnFindPath -= StartFindPath;
    }


    private void StartFindPath()
    {
        Drawing.ResetLine(AllVertexs);
        List<Vertex> path = FindPath.Find(AllVertexs, StartVertex, EndVertex, IsCustomSetWeight);
        if (path != null && path.Count > 0)
        {
            Drawing.Draw(path);
        }
        onResultPath?.Invoke(path);
    }

    private void InitVertex()
    {
        for (int i = 0; i < AllVertexs.Count; i++)
        {
            AllVertexs[i].gameObject.name = (NAME_VERTEX + (i + 1)).ToString();
            AllVertexs[i].InitVertex(IsCustomSetWeight);
        }
    }

    private void AddNewVertex(GameObject go)
    {
        AllVertexs.Add(go.GetComponent<Vertex>());
        go.GetComponent<Vertex>().textMesh.text = AllVertexs.Count.ToString();
        go.name = NAME_VERTEX + AllVertexs.Count.ToString();
    }

    private void AddConnect(Vertex select, Vertex addVertex)
    {
        select.AddConnect(addVertex);
        InterfaceManager.UpdateViewSetting?.Invoke();
    }

    private void DeleteConnect(Vertex select, Vertex deleteVert)
    {
        Vertex del = select.ContactPoint.Find(x => x.gameObject.name == deleteVert.gameObject.name);
        select.DeleteConnect(del);
        InterfaceManager.UpdateViewSetting?.Invoke();
    }

    private void DeleteConnect(Vertex deleteVert)
    {
        for (int i = 0; i < deleteVert.allConnect.Count; i++)
        {
            deleteVert.allConnect[i].endPoint.DeleteConnect(deleteVert);
        }
    }

    private void DeleteVertex(Vertex deleteVert)
    {
        DeleteConnect(deleteVert);
        AllVertexs.Remove(deleteVert);
        deleteVert.Delete();
    }

    private void SetStartVertex(Vertex vertex)
    {
        if (StartVertex != null)
        {
            SetVertexColor(StartVertex, neutralColor);
        }

        StartVertex = vertex;
        SetVertexColor(StartVertex, startColor);
    }

    private void SetEndVertex(Vertex vertex)
    {
        if (EndVertex != null)
        {
            SetVertexColor(EndVertex, neutralColor);
        }

        EndVertex = vertex;
        SetVertexColor(EndVertex, endColor);
    }

    private void EndMoveVertex(bool reInit)
    {
        if (reInit)
        {
            for (int i = 0; i < AllVertexs.Count; i++)
            {
                AllVertexs[i].UpdateLine();
            }
        }
    }
    private void SetVertexColor(Vertex vertex, Color color)
    {
        vertex.sprite.color = color;
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }
}