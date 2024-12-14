using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;

public class MoveAction : BaseAction
{
    
    private List<Vector3> positionList;
    private int _currentPositionIndex;
    private bool _isChangingFloor;
    private float _differentFloorsTeleportTimer;
    private float _differentFloorsTeleportTimerMax = .5f;
    
    [SerializeField] private int maxMoveDistance = 4;

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    public event EventHandler<OnChangeFloorsStartedEventArgs> OnChangedFloorsStarted;

    public class OnChangeFloorsStartedEventArgs : EventArgs
    {
        public GridPosition unitGridPosition;
        public GridPosition targetGridPosition;
    }
    
    public override string GetActionName()
    {
        return "Move";
    }
    
    private void Update()
    {
        if (!isActive) return;

        Vector3 targetPosition = positionList[_currentPositionIndex];
        if (_isChangingFloor)
        {
            // Stop and Teleport Logic
            Vector3 targetSameFloorPosition = targetPosition;
            
            targetSameFloorPosition.y = transform.position.y;
            
            Vector3 rotateDirection = (targetSameFloorPosition - transform.position).normalized;
            
            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, rotateDirection, Time.deltaTime * rotateSpeed);
            
            _differentFloorsTeleportTimer -= Time.deltaTime;
            if (_differentFloorsTeleportTimer < 0f)
            {
                _isChangingFloor = false;
                transform.position = targetPosition;
            }
        }
        else
        {
            //  regular move logic
            
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
        
            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            
            float moveSpeed = 4f;
            transform.position += moveDirection * (moveSpeed * Time.deltaTime);
        }
        
        
        
        float stoppingDistance = 0.1f;
        
        if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance)
        {
            _currentPositionIndex++;
            if (_currentPositionIndex >= positionList.Count)
            {
                    OnStopMoving?.Invoke(this, EventArgs.Empty);
                    ActionComplete();
            }
            else
            {
                targetPosition = positionList[_currentPositionIndex];
                GridPosition targetGridPosition = LevelGrid.Instance.GetGridPosition(targetPosition);
                GridPosition unitGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

                if (targetGridPosition.floor != unitGridPosition.floor)
                {
                    //Different floors
                    _isChangingFloor = true;
                    _differentFloorsTeleportTimer = _differentFloorsTeleportTimerMax;
                    OnChangedFloorsStarted?.Invoke(this, new OnChangeFloorsStartedEventArgs
                    {
                        unitGridPosition = unitGridPosition,
                        targetGridPosition = targetGridPosition
                    });
                }
            }
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onComplete)
    {
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);
        
        _currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }
        
        OnStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(onComplete);
    }
    
    public override List<GridPosition>GetValidDestinations()
    {
        List<GridPosition> validDestinations = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                for(int floor = -maxMoveDistance; floor <= maxMoveDistance; floor++)
                {
                    GridPosition offsetGrdPosition = new GridPosition(x, z, floor);
                    GridPosition testGrdPosition = unitGridPosition + offsetGrdPosition;
                    if (!LevelGrid.Instance.IsValidGridPosition(testGrdPosition))
                    {
                        continue;
                    }

                    if (unitGridPosition == testGrdPosition)
                    {
                        // Same Grid Position where the unit is already at
                        continue;
                    }

                    if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGrdPosition))
                    {
                        // Grid Position already occupied with another unit
                        continue;
                    }

                    if (!Pathfinding.Instance.IsWalkableGriPosition(testGrdPosition))
                    {
                        continue;
                    }
                    if (!Pathfinding.Instance.HasPath(unitGridPosition,testGrdPosition))
                    {
                        continue;
                    }

                    int pathfindingDistanceMultiplier = 10;
                    if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGrdPosition) > maxMoveDistance * pathfindingDistanceMultiplier)
                    {
                        // path length is too long
                        continue;
                    }
                    validDestinations.Add(testGrdPosition);
                }
            }
        }

        return validDestinations;
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = targetCountAtGridPosition * 10
        };
    }
}
