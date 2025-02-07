using System;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] Transform grenadeProjectilePrefb;
    private int _maxThrowDistance = 7;
    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefb, unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.SetUp(gridPosition, OnGrenadeBehaviourComplete);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.throwGranadeClip);
        ActionStart(onActionComplete);
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }

    public override List<GridPosition> GetValidDestinations()
    {
        List<GridPosition> validDestinations = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -_maxThrowDistance; x <= _maxThrowDistance; x++)
        {
            for (int z = -_maxThrowDistance; z <= _maxThrowDistance; z++)
            {
                GridPosition offsetGrdPosition = new GridPosition(x, z, 0);
                GridPosition testGrdPosition = unitGridPosition + offsetGrdPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGrdPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > _maxThrowDistance)
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
        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = 0
        };
    }
}
