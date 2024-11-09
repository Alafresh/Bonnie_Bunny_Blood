using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }
    
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    private GridSystemVisualSingle[,] _gridSystemVisualSinglesArray;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one GridSystemVisual instance");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _gridSystemVisualSinglesArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
        ];
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                
                Transform gridVisualSingleTransform = 
                    Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldGridPosition(gridPosition), Quaternion.identity);
                
                
                _gridSystemVisualSinglesArray[x, z] = gridVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
                //Debug.Log(_gridSystemVisualSinglesArray[x, z]);
            }
        }
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    public void HideAllGridPositions()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                _gridSystemVisualSinglesArray[x, z].Hide();
            }
        }
    }

    public void ShowAllGridPositions(List<GridPosition> gridPositionsList)
    {
        foreach (GridPosition gridPosition in gridPositionsList)
        {
            _gridSystemVisualSinglesArray[gridPosition.X, gridPosition.Z].Show();
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPositions();
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        ShowAllGridPositions(selectedUnit.GetMoveAction().GetValidDestinations());
    }
}
