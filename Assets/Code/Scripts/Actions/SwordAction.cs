using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    private int _maxSwordDistance = 1;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        ActionComplete();
    }

    public override string GetActionName()
    {
        return "Sword";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Debug.Log("Sword Action");
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidDestinations()
    {
        List<GridPosition> validDestinations = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -_maxSwordDistance; x <= _maxSwordDistance; x++)
        {
            for (int z = -_maxSwordDistance; z <= _maxSwordDistance; z++)
            {
                GridPosition offsetGrdPosition = new GridPosition(x, z);
                GridPosition testGrdPosition = unitGridPosition + offsetGrdPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGrdPosition))
                {
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
            ActionValue = 200
        };
    }
}
