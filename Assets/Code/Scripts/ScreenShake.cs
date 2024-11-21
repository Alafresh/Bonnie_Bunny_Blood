using System;
using UnityEngine;
using Unity.Cinemachine;
public class ScreenShake : MonoBehaviour
{
    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            _impulseSource.GenerateImpulse();
        }
    }
}
