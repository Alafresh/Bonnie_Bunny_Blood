using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private Vector3 _targetPosition;
    private Action _onGrenadeBehaviourComplete;

    private void Update()
    {
        Vector3 moveDir = (_targetPosition - transform.position).normalized;
        float moveSpeed = 15f;
        transform.position += moveDir * (moveSpeed * Time.deltaTime);
        float reachedTargetDistance = 0.2f;
        if (Vector3.Distance(transform.position, _targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4;
            Collider[] colliderArray = Physics.OverlapSphere(_targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                }
            }
            
            Destroy(gameObject);
            _onGrenadeBehaviourComplete();
        }
    }

    public void SetUp(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        _onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }
}
