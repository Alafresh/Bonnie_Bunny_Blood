using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAction : BaseAction
{
    
    private Vector3 _targetPosition;
    
    [SerializeField] private int maxMoveDistance = 4;

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    protected override void Awake()
    {
        base.Awake();
        _targetPosition = transform.position;
    }

    public override string GetActionName()
    {
        return "Move";
    }
    
    private void Update()
    {
        if (!isActive) return;
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance)
        {
            float moveSpeed = 4f;
            transform.position += moveDirection * (moveSpeed * Time.deltaTime);
            
        }
        else
        {
            OnStopMoving?.Invoke(this, EventArgs.Empty);
            ActionComplete();
        }
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    public override void TakeAction(GridPosition gridPosition, Action onComplete)
    {
        ActionStart(onComplete);
        _targetPosition = LevelGrid.Instance.GetWorldGridPosition(gridPosition);
        OnStartMoving?.Invoke(this, EventArgs.Empty);
    }
    
    public override List<GridPosition>GetValidDestinations()
    {
        List<GridPosition> validDestinations = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGrdPosition = new GridPosition(x, z);
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
                validDestinations.Add(testGrdPosition);
            }
        }

        return validDestinations;
    }
}
