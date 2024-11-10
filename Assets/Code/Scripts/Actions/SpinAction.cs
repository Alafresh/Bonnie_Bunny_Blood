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
            isActive = false;
            onActionComplete();
        }
    }
    
    public override void TakeAction(GridPosition gridPosition, Action onComplete)
    {
        onActionComplete = onComplete;
        isActive = true;
        _totalSpinAmount = 0f;
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
        return 2;
    }
}
