using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExplode;

    [SerializeField] private Transform grenadeExplosionVfxPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    
    private Vector3 _targetPosition;
    private Action _onGrenadeBehaviourComplete;
    private float _totalDistance;
    private Vector3 _positionXZ;

    private void Update()
    {
        Vector3 moveDir = (_targetPosition - _positionXZ).normalized;
        float moveSpeed = 15f;
        _positionXZ += moveDir * (moveSpeed * Time.deltaTime);
        
        float distance = Vector3.Distance(_positionXZ, _targetPosition);
        float distanceNormalized = 1 - distance / _totalDistance;

        float maxHeight = _totalDistance / 4f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        
        transform.position = new Vector3(_positionXZ.x, positionY, _positionXZ.z);
        
        float reachedTargetDistance = 0.2f;
        if (Vector3.Distance(_positionXZ, _targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4;
            Collider[] colliderArray = Physics.OverlapSphere(_targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                }
                if (collider.TryGetComponent(out DesctructibleCrate destructibleCrate))
                {
                    destructibleCrate.Damage();
                }
            }
            trailRenderer.transform.parent = null;
            OnAnyGrenadeExplode?.Invoke(this, EventArgs.Empty);
            
            Instantiate(grenadeExplosionVfxPrefab, _targetPosition + Vector3.up * 1f, Quaternion.identity);
            
            Destroy(gameObject);
            _onGrenadeBehaviourComplete();
        }
    }

    public void SetUp(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        _onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        _positionXZ = transform.position;
        _positionXZ.y = 0;
        _totalDistance = Vector3.Distance(transform.position, _targetPosition);
    }
}
