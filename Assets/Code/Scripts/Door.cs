using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool isOpen;
    private GridPosition _gridPosition;
    private Animator _animator;
    private Action _onInteractComplete;
    private float _timer;
    private bool _isActive;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetDoorAtGridPosition(_gridPosition, this);
        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _isActive = false;
            _onInteractComplete();
        }
    }

    public void Interact(Action onInteractComplete)
    {
        _onInteractComplete = onInteractComplete;
        _isActive = true;
        _timer = 0.5f;
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        _animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGriPosition(_gridPosition,true);
    }

    private void CloseDoor()
    {
        isOpen = false;
        _animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGriPosition(_gridPosition,false);
    }
}
