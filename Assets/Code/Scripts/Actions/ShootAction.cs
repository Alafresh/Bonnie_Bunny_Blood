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
    
    [SerializeField] LayerMask obstacleLayer;
    
    private State _state;
    private int _maxShootDistance = 7;
    private float _stateTimer;
    private Unit _targetUnit;
    private bool _canShootBullet;
    private int _damage;

    public event EventHandler<OnShotEventArgs> OnShoot;
    public static event EventHandler<OnShotEventArgs> OnAnyShoot;
    
    public class OnShotEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private void Start()
    {
        if (unit.IsEnemy())
        {
            _damage = 15;
        }
        else
        {
            _damage = 40;
        }
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
                aimDirection.y = 0;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Slerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
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
        OnAnyShoot?.Invoke(this, new OnShotEventArgs()
        {
            targetUnit = _targetUnit,
            shootingUnit = unit
        });
        OnShoot?.Invoke(this, new OnShotEventArgs
        {
            targetUnit = _targetUnit,
            shootingUnit = unit
        });
        
        _targetUnit.Damage(_damage);
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
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidDestinations(unitGridPosition);
    }
    
    private List<GridPosition> GetValidDestinations(GridPosition unitGridPosition)
    {
        List<GridPosition> validDestinations = new List<GridPosition>();

        for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
        {
            for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
            {
                for (int floor = -_maxShootDistance; floor <= _maxShootDistance; floor++)
                {
                    GridPosition offsetGrdPosition = new GridPosition(x, z, floor);
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
                    
                    Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                    Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                    
                    float unitShoulderHeight = 1.7f;
                    if (Physics.Raycast(
                            unitWorldPosition + Vector3.up * unitShoulderHeight,
                            shootDir,
                            Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                            obstacleLayer))
                    {
                        // blocked by obstacle
                        continue;
                    }
                    validDestinations.Add(testGrdPosition);
                }
            }
        }

        return validDestinations;
    }

    public Unit GetTargetUnit()
    {
        return _targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return _maxShootDistance;
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f)
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidDestinations(gridPosition).Count;
    }
}
