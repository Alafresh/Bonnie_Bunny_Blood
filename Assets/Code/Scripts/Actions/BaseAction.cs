using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidDestinations();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidDestinations();

    public virtual int GetActionsCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetUnit()
    {
        return unit;
    }

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionsList = new List<EnemyAIAction>();
        
        List<GridPosition> validGridPositionList = GetValidDestinations();

        foreach (GridPosition gridPosition in validGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionsList.Add(enemyAIAction);
        }

        if (enemyAIActionsList.Count > 0)
        {
            // Arrange the largest number first
            enemyAIActionsList.Sort((EnemyAIAction a, EnemyAIAction b) => b.ActionValue - a.ActionValue);
            return enemyAIActionsList[0];
        }
        else
        {
            return null;
        }
    }
    
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
}
