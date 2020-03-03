using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjManager : MonoBehaviour
{
    public static Action<GameObject> OnCreateVertex;
    public static ObjManager Instance;

    [SerializeField] private GameObject RootVertex;

    public GameObject LinePrefab;
    public GameObject VertexPrefab;
    public GameObject ConnectBtn;
    public GameObject DisconnectBtn;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Создание новоого line renderer
    /// </summary>
    /// <param name="targetTr">Родительский объект</param>
    /// <returns></returns>
    public GameObject CreateLine(Transform targetTr)
    {
        GameObject newLine = Instantiate(LinePrefab, targetTr.position, Quaternion.identity, targetTr);
        return newLine;
    }

    /// <summary>
    /// Создание новой вершины
    /// </summary>
    /// <param name="pos">Позиция</param>
    public void CreateVertex(Vector3 pos)
    {
        GameObject newLine = Instantiate(VertexPrefab, pos, Quaternion.identity, RootVertex.transform);
        OnCreateVertex.Invoke(newLine);
    }

    /// <summary>
    /// Создания кнопки для списка
    /// </summary>
    /// <param name="isConnect">Список соединенных или нет</param>
    /// <returns></returns>
    public GameObject CreateListBtn(bool isConnect)
    {
        GameObject newBtn;
        if (isConnect)
        {
            newBtn = Instantiate(ConnectBtn);
        }
        else
        {
            newBtn = Instantiate(DisconnectBtn);
        }
        return newBtn;
    }
}
