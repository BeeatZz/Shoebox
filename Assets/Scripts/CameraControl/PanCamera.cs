using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PanCamera : MonoBehaviour
{
    [Header("Rotation")]
    public float rotateSensitivity = 0.5f;
    public float rotationSmoothing = 5f;

    [Header("Zoom")]
    public float zoomSpeed = 0.05f;
    public float zoomSmoothing = 5f;
    public float minZoom = 0.1f;
    public float maxZoom = 1.5f;

    [Header("Panning")]
    public float panSpeed = 0.005f;
    public Vector2 boxLimits = new Vector2(0.2f, 0.15f);

    [Header("Freedom Settings")]
    public float pitchSensitivity = 0.5f;
    public float minPitch = 10f;
    public float maxPitch = 80f;

    private float targetPitch;
    private float currentPitch;

    private Transform camTransform;
    private float targetZoom;
    private float currentZoom;
    private float targetRotationY;
    private float currentRotationY;
    private Vector3 targetPivotPos;

    void Start()
    {
        camTransform = GetComponentInChildren<Camera>().transform;
        targetZoom = -camTransform.localPosition.z;
        currentZoom = targetZoom;
        targetRotationY = transform.eulerAngles.y;
        currentRotationY = targetRotationY;
        targetPivotPos = transform.position;

        targetPitch = 30f;
        currentPitch = targetPitch;
    }

    void Update()
    {
        HandleInput();
        ApplySmoothingAndMovement();
    }

    void HandleInput()
    {
        bool isRightBtn = Mouse.current.rightButton.isPressed;
        bool isMiddleBtn = Mouse.current.middleButton.isPressed;
        bool isShift = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
        Vector2 delta = Mouse.current.delta.ReadValue();

        if (isRightBtn && !isShift)
        {
            targetRotationY += delta.x * rotateSensitivity;
            targetPitch -= delta.y * pitchSensitivity;
            targetPitch = Mathf.Clamp(targetPitch, minPitch, maxPitch);
        }

        if (isMiddleBtn || (isRightBtn && isShift))
        {
            Vector3 forward = Vector3.ProjectOnPlane(camTransform.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(camTransform.right, Vector3.up).normalized;
            targetPivotPos += (right * -delta.x + forward * -delta.y) * panSpeed;

            targetPivotPos.x = Mathf.Clamp(targetPivotPos.x, -boxLimits.x, boxLimits.x);
            targetPivotPos.z = Mathf.Clamp(targetPivotPos.z, -boxLimits.y, boxLimits.y);
        }

        Vector2 scroll = Mouse.current.scroll.ReadValue();
        if (scroll.y != 0)
        {
            targetZoom -= (scroll.y > 0 ? 1 : -1) * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
    }

    void ApplySmoothingAndMovement()
    {
        currentRotationY = Mathf.LerpAngle(currentRotationY, targetRotationY, Time.deltaTime * rotationSmoothing);
        currentPitch = Mathf.LerpAngle(currentPitch, targetPitch, Time.deltaTime * rotationSmoothing);
        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSmoothing);
        transform.position = Vector3.Lerp(transform.position, targetPivotPos, Time.deltaTime * rotationSmoothing);

        transform.rotation = Quaternion.Euler(currentPitch, currentRotationY, 0);

        camTransform.localPosition = new Vector3(0, 0, -currentZoom);
    }
}
