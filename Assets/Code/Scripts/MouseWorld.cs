using System;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [SerializeField] private LayerMask mousePlaneLayerMask;
    private static MouseWorld _instance;

    private void Awake()
    {
        _instance = this;
    }
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _instance.mousePlaneLayerMask);
        return hit.point;
    }
}
