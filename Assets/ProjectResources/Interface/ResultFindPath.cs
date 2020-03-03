using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Вывода результата поиска пути
/// </summary>
[RequireComponent(typeof(Text))]
public class ResultFindPath : MonoBehaviour
{
    private const string ARROW = " => ";
    private Text resultText;
    private void Start()
    {
        resultText = GetComponent<Text>();
        GraphManager.onResultPath += FindResult;
    }

    private void FindResult(List<Vertex> path)
    {
        if (path == null || path.Count == 0)
        {
            resultText.text = "Путь не найден";
        }
        else
        {
            resultText.text = "Путь построен: ";

            for (int i = 0; i < path.Count; i++)
            {
                if (i < path.Count - 1)
                {
                    resultText.text += path[i].gameObject.name + ARROW;
                }
                else
                {
                    resultText.text += path[i].gameObject.name;
                }
            }
        }
    }

    private void OnDestroy()
    {
        GraphManager.onResultPath += FindResult;
    }
}
