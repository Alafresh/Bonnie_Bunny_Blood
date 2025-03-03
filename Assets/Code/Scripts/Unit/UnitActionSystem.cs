using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance {get; private set;}
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    private BaseAction _selectedAction;
    private bool _isBusy;
    
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;
    [SerializeField] private LayerMask enemyLayerMask;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of UnitActionSystem attached!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Time.timeScale = 1;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        // actually executes an action
        if (_isBusy) return;
        
        // return if its turns enemys
        if (!TurnSystem.Instance.IsPlayerTurn()) return;
        
        // mouse over button
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        // return if it got unit and stop update from running
        if (TryHandleUnitSelection()) return;
        
        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if (InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMouseWorldPosition());
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());

            if (_selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                if (selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction))
                {
                    SetBusy();
                    _selectedAction.TakeAction(mouseGridPosition, ClearBusy);
                    OnActionStarted?.Invoke(this, EventArgs.Empty);
                }
            }
            else if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, enemyLayerMask))
            {
                if (hit.transform.TryGetComponent(out Unit unit))
                {
                    if (unit.IsEnemy())
                    {

                        if (_selectedAction.IsValidActionGridPosition(unit.GetGridPosition()))
                        {
                            if (selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction))
                            {
                                SetBusy();
                                _selectedAction.TakeAction(unit.GetGridPosition(), ClearBusy);
                                OnActionStarted?.Invoke(this, EventArgs.Empty);
                            }
                        }
                    }

                }
            }
        }
    }
    private void SetBusy()
    {
        _isBusy = true;
        OnBusyChanged?.Invoke(this, _isBusy);
    }

    private void ClearBusy()
    {
        _isBusy = false;
        OnBusyChanged?.Invoke(this, _isBusy);
    }
    
    private bool TryHandleUnitSelection()
    {
        if (InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, unitLayerMask))
            {
                // return false or true depends of component
                if (hit.transform.TryGetComponent(out Unit unit))
                {
                    if (unit == selectedUnit)
                    {
                        return false;
                    }

                    if (unit.IsEnemy())
                    {
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetAction<MoveAction>());
        
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction action)
    {
        _selectedAction = action;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return _selectedAction;
    }
}
