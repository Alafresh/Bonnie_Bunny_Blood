using UnityEngine;

public class SpinAction : BaseAction
{
    private float _totalSpinAmount;
    public delegate void SpinCompleteDelegate();

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
        }
    }
    
    public void Spin(SpinCompleteDelegate onComplete)
    {
        isActive = true;
        _totalSpinAmount = 0f;
    }
}
