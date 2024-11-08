using System;
using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineFollow cameraFollow;
    private const float MaxZoomOffset = 12f;
    private const float MinZoomOffset = 2f;
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
        Vector3 inputMoveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x += 1f;
        }

        float moveSpeed = 5f;
        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * (moveSpeed * Time.deltaTime);
    }

    private void CameraRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y += 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y -= 1f;
        }
        
        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * (rotationSpeed * Time.deltaTime);
    }
    private void CameraZoom()
    {
        float zoomAmount = 1f;

        if (Input.mouseScrollDelta.y > 0)
        {
            cameraTarget.y -= zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            cameraTarget.y += zoomAmount;
        }
        cameraTarget.y = Mathf.Clamp(cameraTarget.y, MinZoomOffset, MaxZoomOffset);
        float zoomSpeed = 50f;
        cameraFollow.FollowOffset = Vector3.Lerp(cameraFollow.FollowOffset, cameraTarget, Time.deltaTime * zoomSpeed);
    }
}
