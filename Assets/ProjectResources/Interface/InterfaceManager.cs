using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class InterfaceManager : MonoBehaviour
{
    public static Action<Vertex> SelectVertex;

    public static Action<Vertex> DeleteVertex;
    public static Action<Vector2> OnCreateVertex;

    public static Action UpdateViewSetting;

    private static GameObject openPanel;
    public static bool IsOpenPanel
    {
        get
        {
            return openPanel != null;
        }
    }

    public GameObject SettingVertexPanel;

    public GameObject CreateVertexPanel;

    private Vertex selectVertex;

    public FindPath findPath;

    public GraphManager graphManager;

    private Vector3 createVertexPos;

    [SerializeField] Transform ConnectPanel;
    [SerializeField] Transform DisConnectPanel;

    private List<GameObject> createBtnConnectList = new List<GameObject>();
    private List<GameObject> createBtnDisconnectList = new List<GameObject>();



    private void Start()
    {
        SelectVertex += OpenVertexSetting;
        OnCreateVertex += OpenCreateVertex;
        UpdateViewSetting += ReInitSettingPanel;
        UpdateViewSetting += ReInitSettingPanel;
    }

    /// <summary>
    /// Начало поиска пути
    /// </summary>
    public void OnFindLick()
    {
        GraphManager.OnFindPath?.Invoke();
    }

    private void ReInitSettingPanel()
    {
        LoadConenctVertex();
        LoadDissconnectPanel();
    }

    private void OpenVertexSetting(Vertex vertex)
    {
        selectVertex = vertex;
        LoadConenctVertex();
        LoadDissconnectPanel();
        ShowPanel(SettingVertexPanel);
    }

    private void LoadConenctVertex()
    {
        if (createBtnConnectList.Count > 0)
        {
            for (int i = 0; i < createBtnConnectList.Count; i++)
            {
                Destroy(createBtnConnectList[i]);
            }
            createBtnConnectList.Clear();
        }

        if (selectVertex.allConnect.Count > 0)
        {
            for (int i = 0; i < selectVertex.allConnect.Count; i++)
            {
                GameObject newBtn = ObjManager.Instance.CreateListBtn(true);
                newBtn.GetComponent<ConnectPanel>().SetVertex(selectVertex, selectVertex.allConnect[i].endPoint);
                newBtn.transform.SetParent(ConnectPanel);
                newBtn.transform.localScale = Vector3.one;
                createBtnConnectList.Add(newBtn);
            }
        }
    }

    private void LoadDissconnectPanel()
    {
        if (createBtnDisconnectList.Count > 0)
        {
            for (int i = 0; i < createBtnDisconnectList.Count; i++)
            {
                Destroy(createBtnDisconnectList[i]);
            }
            createBtnDisconnectList.Clear();
        }

        List<Vertex> noConnect = FindNoConnectVertex();

        if (noConnect.Contains(selectVertex))
        {
            noConnect.Remove(selectVertex);
        }

        if (noConnect.Count() > 0)
        {
            for (int i = 0; i < noConnect.Count; i++)
            {
                GameObject newBtn = ObjManager.Instance.CreateListBtn(false);
                newBtn.GetComponent<ConnectPanel>().SetVertex(selectVertex, noConnect[i]);
                newBtn.transform.SetParent(DisConnectPanel);
                newBtn.transform.localScale = Vector3.one;
                createBtnDisconnectList.Add(newBtn);
            }
        }
    }

    private List<Vertex> FindNoConnectVertex()
    {
        return graphManager.AllVertexs.Except(selectVertex.ContactPoint).ToList();
    }

    public void DeleteClick()
    {
        DeleteVertex.Invoke(selectVertex);
        ClosePanel();
    }

    /// <summary>
    /// Установить точку как стартовая
    /// </summary>
    public void OnSelectStartVertex()
    {
        if (selectVertex == graphManager.EndVertex)
        {
            Vertex temp = graphManager.StartVertex;
            graphManager.EndVertex = null;

            GraphManager.OnSetStartVertex.Invoke(selectVertex);
            GraphManager.OnSetEndVertex.Invoke(temp);
            ClosePanel();
            return;
        }
        else
        {
            GraphManager.OnSetStartVertex.Invoke(selectVertex);
        }
        ClosePanel();
    }

    /// <summary>
    /// Установить точку как конечная
    /// </summary>
    public void OnSelectEndVertex()
    {
        if (selectVertex == graphManager.StartVertex)
        {
            Vertex temp = graphManager.EndVertex;
            graphManager.StartVertex = null;
            GraphManager.OnSetEndVertex.Invoke(selectVertex);
            GraphManager.OnSetStartVertex.Invoke(temp);
            ClosePanel();
            return;
        }
        else
        {
            GraphManager.OnSetEndVertex.Invoke(selectVertex);
        }
        ClosePanel();
    }

    private void OpenCreateVertex(Vector2 pos)
    {
        createVertexPos = pos;
        ShowPanel(CreateVertexPanel);
    }

    public void CreateVertex()
    {
        ObjManager.Instance.CreateVertex(createVertexPos);
        ClosePanel();
    }

    private void ShowPanel(GameObject panel)
    {
        openPanel = panel;
        openPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        openPanel.SetActive(false);
        openPanel = null;
    }

    private void OnDestroy()
    {
        SelectVertex -= OpenVertexSetting;
        OnCreateVertex -= OpenCreateVertex;
        UpdateViewSetting -= ReInitSettingPanel;
        UpdateViewSetting -= ReInitSettingPanel;
    }
}