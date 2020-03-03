using UnityEngine;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// Управление
/// </summary>
public class InputController : MonoBehaviour
{
    public static Action<bool> OnUpdatePosition;
    private RaycastHit2D hit;
    private void Update()
    {
        if (!InterfaceManager.IsOpenPanel && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null)
                {
                    hit.collider.gameObject.transform.localPosition = new Vector3(ray.origin.x, ray.origin.y, 0);
                }
                else
                {
                    OnUpdatePosition.Invoke(true);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnUpdatePosition.Invoke(hit.collider != null);
            }


            if (Input.GetMouseButtonUp(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null)
                {
                    InterfaceManager.SelectVertex(hit.collider.gameObject.GetComponent<Vertex>());
                }
                else
                {
                    InterfaceManager.OnCreateVertex(ray.origin);
                }
            }
        }
    }
}