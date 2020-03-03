using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Вершина графа
/// </summary>
public class Vertex : MonoBehaviour
{
    public Vector2 Posititon
    {
        get
        {
            return gameObject.transform.localPosition;
        }
    }

    [SerializeField]
    private float customWeight;
    private float weight;
    public float Weight
    {
        get { return weight; }
        set
        {
            weight = value;
            textWeight.text = value.ToString("##.##");
        }
    }

    public bool IsCheckConnect;

    public List<Vertex> ContactPoint;

    public SpriteRenderer sprite;
    public TextMesh textMesh;
    public TextMesh textWeight;

    public List<ConnectingTunnel> allConnect = new List<ConnectingTunnel>();
    private List<Vertex> emptyConnect = new List<Vertex>();

    /// <summary>
    /// Нициализация вершины
    /// </summary>
    public void InitVertex(bool isCustomWeight)
    {
        if (ContactPoint.Count > 0)
        {
            for (int i = 0; i < ContactPoint.Count; i++)
            {
                ConnectingTunnel newConnect = new ConnectingTunnel();
                newConnect.endPoint = ContactPoint[i];
                if (!ContactPoint[i].IsCheckConnect)
                {
                    newConnect.lineRenderer = ObjManager.Instance.CreateLine(gameObject.transform).GetComponent<LineRenderer>();
                    newConnect.lineRenderer.SetPosition(0, Posititon);
                    newConnect.lineRenderer.SetPosition(1, ContactPoint[i].Posititon);
                }
                else
                {
                    ConnectingTunnel tun = ContactPoint[i].allConnect.Find(x => x.endPoint.gameObject.name == gameObject.name);
                    if (tun != null)
                    {
                        newConnect.lineRenderer = tun.lineRenderer;
                    }
                    else
                    {
                        emptyConnect.Add(ContactPoint[i]);
                        newConnect = null;
                    }
                }
                if (newConnect != null)
                {
                    allConnect.Add(newConnect);
                }
            }
        }
        IsCheckConnect = true;
        ClearEmptyContact();
        if (isCustomWeight)
        {
            Weight = customWeight;
        }
    }

    private void ClearEmptyContact()
    {
        if (emptyConnect.Count > 0)
        {
            for (int i = 0; i < emptyConnect.Count; i++)
            {
                ContactPoint.Remove(emptyConnect[i]);
            }
            emptyConnect.Clear();
        }
    }

    /// <summary>
    /// Обновление позиций линий
    /// </summary>
    public void UpdateLine()
    {
        for (int i = 0; i < allConnect.Count; i++)
        {
            allConnect[i].lineRenderer.SetPosition(0, Posititon);
            allConnect[i].lineRenderer.SetPosition(1, allConnect[i].endPoint.Posititon);
        }
    }

    public void DeleteConnect(Vertex vertex)
    {
        ConnectingTunnel deleteConnect = allConnect.Find(x => x.endPoint.gameObject.name == vertex.gameObject.name);
        if (deleteConnect != null)
        {
            ContactPoint.Remove(deleteConnect.endPoint);
            Destroy(deleteConnect.lineRenderer.gameObject);
            allConnect.Remove(deleteConnect);
        }
        else
        {
            Debug.LogError("Ребро не обнаружено");
        }
    }

    /// <summary>
    /// Добавление соединения
    /// </summary>
    /// <param name="vertex">Вершина</param>
    public void AddConnect(Vertex vertex)
    {
        ConnectingTunnel newConnect = new ConnectingTunnel();
        ContactPoint.Add(vertex);
        newConnect.endPoint = vertex;
        newConnect.lineRenderer = ObjManager.Instance.CreateLine(gameObject.transform).GetComponent<LineRenderer>();
        newConnect.lineRenderer.SetPosition(0, Posititon);
        newConnect.lineRenderer.SetPosition(1, vertex.Posititon);
        allConnect.Add(newConnect);
        vertex.ReverseConnect(this, newConnect.lineRenderer);
    }

    /// <summary>
    /// Метод дополнительное соединения без создания линии
    /// </summary>
    /// <param name="vertex">Вершина</param>
    /// <param name="line">Компонетн</param>
    public void ReverseConnect(Vertex vertex, LineRenderer line)
    {
        ConnectingTunnel newConnect = new ConnectingTunnel();
        ContactPoint.Add(vertex);
        newConnect.endPoint = vertex;
        newConnect.lineRenderer = line;
        allConnect.Add(newConnect);
    }

    /// <summary>
    /// Удаление вершины
    /// </summary>
    public void Delete()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Сброс цвета линий
    /// </summary>
    public void ResetColor()
    {
        for (int i = 0; i < allConnect.Count; i++)
        {
            allConnect[i].lineRenderer.startColor = Color.white;
            allConnect[i].lineRenderer.endColor = Color.white;
        }
    }
}