using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Поиск пути
/// </summary>
public class FindPath
{
    /// <summary>
    /// Поиск
    /// </summary>
    /// <param name="AllVertexs">Полный списко вершин</param>
    /// <param name="startVertex">Стартовая вершина</param>
    /// <param name="endVertex">Конечная вершина</param>
    /// <param name="IsCustomSetWeight">Ручна установка веса</param>
    /// <returns></returns>
    public static List<Vertex> Find(List<Vertex> AllVertexs, Vertex startVertex, Vertex endVertex, bool IsCustomSetWeight)
    {
        List<Vertex> NotVerified = new List<Vertex>(AllVertexs);
        Dictionary<Vertex, WayPoint> path = new Dictionary<Vertex, WayPoint>();

        path.Add(startVertex, new WayPoint() { prevPoint = null, Weight = 0 });

        while (true)
        {
            Vertex currentPoint = null;
            float bestWeight = float.MaxValue;

            for (int i = 0; i < NotVerified.Count; i++)
            {
                Vertex vert = NotVerified[i];
                if (!IsCustomSetWeight)
                {
                    if (currentPoint != null)
                    {
                        vert.Weight = Mathf.Abs(Vector2.Distance(currentPoint.Posititon, vert.Posititon));
                    }
                    if (path.ContainsKey(vert) && path[vert].Weight < bestWeight)
                    {

                        currentPoint = vert;
                        bestWeight = path[vert].Weight;
                    }
                }
            }

            if (currentPoint == null)
            {
                return null;
            }

            if (currentPoint == endVertex)
            {
                break;
            }

            for (int i = 0; i < currentPoint.ContactPoint.Count; i++)
            {
                Vertex nextVer = currentPoint.ContactPoint[i];
                float tempWeight = path[currentPoint].Weight + nextVer.Weight;
                if (!path.ContainsKey(nextVer) || path[nextVer].Weight > tempWeight)
                {
                    path[nextVer] = new WayPoint() { prevPoint = currentPoint, Weight = tempWeight };
                }

                if (nextVer != startVertex)
                {
                    nextVer.Weight = tempWeight;
                }
            }

            NotVerified.Remove(currentPoint);
        }

        List<Vertex> ret = new List<Vertex>();
        Vertex end = endVertex;
        while (end != null)
        {
            ret.Add(end);
            end = path[end].prevPoint;
        }
        return ret;
    }
}