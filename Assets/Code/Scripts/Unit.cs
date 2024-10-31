using UnityEngine;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    [SerializeField] private Animator unitAnimator;
    private Vector3 _targetPosition;

    private void Update()
    {
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * (moveSpeed * Time.deltaTime);
            unitAnimator.SetBool(IsWalking, true);
        }
        else
        {
            unitAnimator.SetBool(IsWalking, false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetMouseWorldPosition());
        }
    }
        
    private void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
