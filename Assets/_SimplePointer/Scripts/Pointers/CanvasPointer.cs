using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasPointer : MonoBehaviour
{
    public float defaultLength = 5f;
    private LineRenderer lineRenderer = null;

    public EventSystem eventSystem = null;
    public StandaloneInputModule inputModule = null;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateLength();
    }

    private void UpdateLength()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, GetEnd());
    }

    private Vector3 DefaultEnd(float length)
    {
        return transform.position + (transform.forward * length);
    }


    private Vector3 GetEnd()
    {


        return Vector3.zero;
    }


    float GetCanvasDistance()
    {
        return 0f;
    }

    //RaycastResult FindFirstRayCast(List<RaycastResult> results)
    //{
    //    return null;
    //}
}
