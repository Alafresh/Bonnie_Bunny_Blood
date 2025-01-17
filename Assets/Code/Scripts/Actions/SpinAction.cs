using UnityEngine;
using System;
using System.Collections.Generic;

public class SpinAction : BaseAction
{
    private float _totalSpinAmount;
    

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        float spinAddAmount = 360f  * Time.deltaTime;
        transform.eulerAngles += new Vector3(0f, spinAddAmount, 0f);
        _totalSpinAmount += spinAddAmount;
        if (_totalSpinAmount >= 360)
        {
            ActionComplete();
        }
    }
    
    public override void TakeAction(GridPosition gridPosition, Action onComplete)
    {
        _totalSpinAmount = 0f;
        ActionStart(onComplete);
    }
    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidDestinations()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override int GetActionsCost()
    {
        return 1;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = 0
        };
    }

}
