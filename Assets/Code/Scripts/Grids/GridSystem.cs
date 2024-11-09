using Unity.Mathematics;
using UnityEngine;

public class GridSystem 
{
    private int _width;
    private int _height;
    private float _cellSize;
    private GridObject[,] _gridObjectArray;
    public GridSystem(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;

        _gridObjectArray = new GridObject[width, height];
        
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                _gridObjectArray[x ,z] = new GridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.X, 0, gridPosition.Z) * _cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / _cellSize),
            Mathf.RoundToInt(worldPosition.z / _cellSize)
        );
    }
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return _gridObjectArray[gridPosition.X, gridPosition.Z];
    }

    public bool IsValidPosition(GridPosition gridPosition)
    {
        return gridPosition.X >= 0 &&
               gridPosition.X < _width &&
               gridPosition.Z >= 0 && 
               gridPosition.Z < _height;
    }
    
    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }
    
}
