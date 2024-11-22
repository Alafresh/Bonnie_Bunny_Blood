using System;
using UnityEngine;
using Unity.Cinemachine;
public class ScreenShake : MonoBehaviour
{
    private CinemachineImpulseSource _impulseSource;

    public static ScreenShake Instance {get ; private set;}
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of ScreenShake!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void Shake(float intensity)
    {
        _impulseSource.GenerateImpulse(intensity);
    }
}
