using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGrid : MonoBehaviour
{

    public static LevelGrid Instance { get; private set; }

    public const float FLOOR_HEIGHT = 1.36f;

    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] private float cellSize;
    [SerializeField] private int floorAmount;
    [SerializeField] private GameObject gameover;

    private bool _gameoverFlag;
    private float countDown = 5;
    public event EventHandler OnAnyUnitMovedGridPosition;


    [SerializeField] private Transform gridDebugObjectPrefab;

    private List<GridSystem<GridObject>> gridSystemList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gridSystemList = new List<GridSystem<GridObject>>();
        for (int floor = 0; floor < floorAmount; floor++)
        {
            GridSystem<GridObject> gridSystem = new GridSystem<GridObject>(width, height, cellSize, floor, FLOOR_HEIGHT,
                    (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
            //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
            gridSystemList.Add(gridSystem);
        }
    }

    private void Start()
    {
        Pathfinding.Instance.SetUp(width, height, cellSize, floorAmount);
    }

    private void Update()
    {
        if (!_gameoverFlag) return;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverClip);
        gameover.SetActive(true);
        countDown -= Time.deltaTime;
        if (countDown <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void GameOver()
    {
        _gameoverFlag = true;
    }
    private GridSystem<GridObject> GetGridSystem(int floor)
    {
        return gridSystemList[floor];
    }
    
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = GetGridSystem(gridPosition.floor).GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GetGridSystem(gridPosition.floor).GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = GetGridSystem(gridPosition.floor).GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);

        AddUnitAtGridPosition(toGridPosition, unit);

        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public int GetFloor(Vector3 worldPosition)
    {
        return Mathf.RoundToInt(worldPosition.y / FLOOR_HEIGHT);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        int floor = GetFloor(worldPosition);
        return GetGridSystem(floor).GetGridPosition(worldPosition);
    } 
    
    public Vector3 GetWorldPosition(GridPosition gridPosition) => GetGridSystem(gridPosition.floor).GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        if (gridPosition.floor < 0 || gridPosition.floor >= floorAmount)
        {
            return false;
        }
        else
        {
            return GetGridSystem(gridPosition.floor).IsValidGridPosition(gridPosition);
        }
    } 

    public int GetWidth() => GetGridSystem(0).GetWidth();
    
    public int GetHeight() => GetGridSystem(0).GetHeight();
    public int GetFloorAmount() => floorAmount;

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GetGridSystem(gridPosition.floor).GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GetGridSystem(gridPosition.floor).GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GetGridSystem(gridPosition.floor).GetGridObject(gridPosition);
        return gridObject.GetInteractable();
    }
    
    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        GridObject gridObject = GetGridSystem(gridPosition.floor).GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);
    }
}
