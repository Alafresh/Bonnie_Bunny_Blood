using UnityEngine;
using System;

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
    
    public void Spin(Action onComplete)
    {
        onActionComplete = onComplete;
        isActive = true;
        _totalSpinAmount = 0f;
    }

    public override string GetActionName()
    {
        return "Spin";
    }
}
