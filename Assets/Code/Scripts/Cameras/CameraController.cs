using System;
using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineFollow cameraFollow;
    private const float MaxZoomOffset = 12f;
    private const float MinZoomOffset = 2f;
    private const float MaxTransformZOffset = 94f;
    private const float MinTransformZOffset = -2f;
    private const float MaxTransformXOffset = 90f;
    private const float MinTransformXOffset = 0f;
    
    private Vector3 cameraTarget;

    private void Start()
    {
        cameraTarget = cameraFollow.FollowOffset;
    }

    private void Update()
    {
        CameraMovement();
        CameraRotation();
        CameraZoom();
    }

    private void CameraMovement()
    {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();
        float moveSpeed = 5f;
    
        // Calculate movement
        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        Vector3 newPosition = transform.position + moveVector * (moveSpeed * Time.deltaTime);
    
        // Apply limits of movement
        newPosition.x = Mathf.Clamp(newPosition.x, MinTransformXOffset, MaxTransformXOffset);
        newPosition.z = Mathf.Clamp(newPosition.z, MinTransformZOffset, MaxTransformZOffset);

        // Update camera position
        transform.position = newPosition;
    }

    private void CameraRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();
        
        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * (rotationSpeed * Time.deltaTime);
    }
    private void CameraZoom()
    {
        float zoomIncreaseAmount = 1f;

        cameraTarget.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;
        
        cameraTarget.y = Mathf.Clamp(cameraTarget.y, MinZoomOffset, MaxZoomOffset);
        float zoomSpeed = 50f;
        cameraFollow.FollowOffset = Vector3.Lerp(cameraFollow.FollowOffset, cameraTarget, Time.deltaTime * zoomSpeed);
    }
}
