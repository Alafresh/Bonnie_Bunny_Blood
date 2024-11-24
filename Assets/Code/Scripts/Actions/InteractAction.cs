using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    private int _maxInteractDistance = 1;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }

    public override string GetActionName()
    {
        return "Interact";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Door door = LevelGrid.Instance.GetDoorAtGridPosition(gridPosition);
        door.Interact(OnInteractComplete);
        Debug.Log("InteractAction");
        ActionStart(onActionComplete);
    }

    private void OnInteractComplete()
    {
        ActionComplete();
    }
    public override List<GridPosition> GetValidDestinations()
    {
        List<GridPosition> validDestinations = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -_maxInteractDistance; x <= _maxInteractDistance; x++)
        {
            for (int z = -_maxInteractDistance; z <= _maxInteractDistance; z++)
            {
                GridPosition offsetGrdPosition = new GridPosition(x, z);
                GridPosition testGrdPosition = unitGridPosition + offsetGrdPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGrdPosition))
                {
                    continue;
                }

                Door door = LevelGrid.Instance.GetDoorAtGridPosition(testGrdPosition);
                if (door == null)
                {
                    // no door on this gridposition
                    continue;
                }
                validDestinations.Add(testGrdPosition);
            }
        }

        return validDestinations;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction()
        {
            GridPosition = gridPosition,
            ActionValue = 0
        };
    }
}
