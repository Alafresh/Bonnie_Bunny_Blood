using System;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectPrefab;
    private GridSystem _gridSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gridSystem = new GridSystem(10, 10, 2f);
        _gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
        
    }

    // private void Update()
    // {
    //     Debug.Log(_gridSystem.GetGridPosition(MouseWorld.GetMouseWorldPosition()));
    // }
}
