using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    [SerializeField] private Animator unitAnimator;
    private Vector3 _targetPosition;
    private GridPosition _gridPosition;

    private void Awake()
    {
        _targetPosition = transform.position;
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
    }

    private void Update()
    {
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * (moveSpeed * Time.deltaTime);
            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            
            unitAnimator.SetBool(IsWalking, true);
        }
        else
        {
            unitAnimator.SetBool(IsWalking, false);
        }
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            // unit changed grid position
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
        }
    }
        
    public void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
