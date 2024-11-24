using System;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private MeshRenderer meshRenderer;
    
    private Action _onInteractComplete;
    private float _timer;
    private bool _isActive;
    private GridPosition _gridPosition;
    private bool _isGreen = false;
    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(_gridPosition, this);
        SetColorGreen();
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
    
    private void SetColorGreen()
    {
        _isGreen = true;
        meshRenderer.material = greenMaterial;
    }

    private void SetColorRed()
    {
        _isGreen = false;
        meshRenderer.material = redMaterial;
    }

    public void Interact(Action onInteractionComplete)
    {
        _onInteractComplete = onInteractionComplete;
        _isActive = true;
        _timer = 0.5f;
        if (_isGreen)
        {
            SetColorRed();
        }
        else
        {
            SetColorGreen();
        }
    }
}
