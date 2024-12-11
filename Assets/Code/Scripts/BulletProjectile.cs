using System;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVfxPrefab;
    private Vector3 _targetPosition;
    public void SetUp(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    private void Update()
    {
        // get direction rest two vectors and normalized
        Vector3 moveDir = (_targetPosition - transform.position).normalized;
        
        float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);
        
        float moveSpeed = 50f;
        transform.position += moveDir * (moveSpeed * Time.deltaTime);
        
        float afterBeforeMoving = Vector3.Distance(transform.position, _targetPosition);
        
        if (distanceBeforeMoving < afterBeforeMoving)
        {
            transform.position = _targetPosition;
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
            
            Instantiate(bulletHitVfxPrefab, _targetPosition, Quaternion.identity);
        }
    }
}
