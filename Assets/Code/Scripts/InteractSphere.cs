using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractSphere : MonoBehaviour, IInteractable
{
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private List<GameObject> waves;
    [SerializeField] private GameObject cameraWave;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject phase;
    [SerializeField] private float value;

    public bool lastContainer;
    private Action _onInteractComplete;
    private float _timer;
    private bool _isActive;
    private GridPosition _gridPosition;
    private bool _isGreen = false;
    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(_gridPosition, this);
        Pathfinding.Instance.SetIsWalkableGriPosition(_gridPosition,false);
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
            cameraWave.SetActive(false);
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
        slider.value = value;
        phase.SetActive(true);
        _isGreen = false;
        meshRenderer.material = redMaterial;
        if (lastContainer)
        {
            SceneManager.LoadScene(0);
            return;
        }
        cameraWave.SetActive(true);
        foreach (GameObject wave in waves)
        {
            wave.SetActive(true);
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        _onInteractComplete = onInteractionComplete;
        _isActive = true;
        _timer = 4f;
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
