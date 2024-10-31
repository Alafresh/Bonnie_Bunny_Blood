using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private void Update()
    {
        // return if it got unit and stop update from running
        if (TryHandleUnitSelection()) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            selectedUnit.Move(MouseWorld.GetMouseWorldPosition());
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, unitLayerMask))
        {
            // return false or true depends of component
            if (hit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                selectedUnit = unit;
                return true;
            }
        }
        return false;
    }
}
