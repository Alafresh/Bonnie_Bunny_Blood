using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private enum State
    {
        Aiming,
        Shooting,
        CoolOff,
    }
    private State _state;
    private int _maxShootDistance = 7;
    private float _stateTimer;
    private Unit _targetUnit;
    private bool _canShootBullet;

    public event EventHandler<OnShotEventArgs> OnShoot;
    
    public class OnShotEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }
    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        switch (_state)
        {
            case State.Aiming:
                Vector3 aimDirection = (_targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                break;
            case State.Shooting:
                if (_canShootBullet)
                {
                    Shoot();
                    _canShootBullet = false;
                }
                break;
            case State.CoolOff:
                break;
        }
        
        _stateTimer -= Time.deltaTime;
        if (_stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (_state)
        {
            case State.Aiming:
                _state = State.Shooting;
                float shootingStateTime = 0.1f;
                _stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                _state = State.CoolOff;
                float coolOffStateTime = 0.5f;
                _stateTimer = coolOffStateTime;
                break;
            case State.CoolOff:
                ActionComplete();
                break;
        }
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShotEventArgs
        {
            targetUnit = _targetUnit,
            shootingUnit = unit
        });
        _targetUnit.Damage(40);
    }
    public override string GetActionName()
    {
        return "Shoot";
    }

    public override void TakeAction(GridPosition gridPosition, Action onComplete)
    {
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        _state = State.Aiming;
        float aimingStateTime = 1f;
        _stateTimer = aimingStateTime;
        _canShootBullet = true;
        
        ActionStart(onComplete);
    }

    public override List<GridPosition> GetValidDestinations()
    {
        List<GridPosition> validDestinations = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
        {
            for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
            {
                GridPosition offsetGrdPosition = new GridPosition(x, z);
                GridPosition testGrdPosition = unitGridPosition + offsetGrdPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGrdPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > _maxShootDistance)
                {
                    continue;
                }
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGrdPosition))
                {
                    // Grid Position is empty no unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGrdPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // both units on same team
                    continue;
                }
                
                validDestinations.Add(testGrdPosition);
            }
        }

        return validDestinations;
    }

    public Unit GetTargetUnit()
    {
        return _targetUnit;
    }
}
