using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.UI;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }
    
    private List<Unit> _unitList;
    private List<Unit> _friendlyUnitList;
    private List<Unit> _enemyUnitList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of UnitManager");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _unitList = new List<Unit>();
        _friendlyUnitList = new List<Unit>();
        _enemyUnitList = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }
    
    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        
        Debug.Log(unit + "OnAnyUnitSpawned");
        
        _unitList.Add(unit);
        if (unit.IsEnemy())
        {
            _enemyUnitList.Add(unit);
        }
        else
        {
            _friendlyUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        
        Debug.Log(unit + "OnAnyUnitDied");
        
        _unitList.Remove(unit);
        if (unit.IsEnemy())
        {
            _enemyUnitList.Remove(unit);
        }
        else
        {
            _friendlyUnitList.Remove(unit);
        }
    }

    public List<Unit> GetUnitList()
    {
        return _unitList;
    }
    public List<Unit> GetFriendlyUnitList()
    {
        return _friendlyUnitList;
    }
    public List<Unit> GetEnemyUnitList()
    {
        return _enemyUnitList;
    }
}
