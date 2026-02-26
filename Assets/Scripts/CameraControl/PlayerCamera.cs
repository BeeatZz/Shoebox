using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("Rotation")]
    public float sensitivity = 0.5f;
    public float rotationSmoothing = 5f;

    [Header("Zoom")]
    public float zoomSpeed = 0.05f;
    public float zoomSmoothing = 5f;
    public float minZoom = 0.2f;
    public float maxZoom = 1.2f;

    private Transform camTransform;
    private float targetZoom;
    private float currentZoom;
    private float targetRotationY;
    private float currentRotationY;

    void Start()
    {
        camTransform = GetComponentInChildren<Camera>().transform;

        targetZoom = -camTransform.localPosition.z;
        currentZoom = targetZoom;
        targetRotationY = transform.eulerAngles.y;
        currentRotationY = targetRotationY;
    }

    void Update()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            targetRotationY += mouseDelta.x * sensitivity;
        }

        Vector2 scrollValue = Mouse.current.scroll.ReadValue();
        if (scrollValue.y != 0)
        {
            float normalizedScroll = scrollValue.y > 0 ? 1 : -1;
            targetZoom -= normalizedScroll * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }

        currentRotationY = Mathf.LerpAngle(currentRotationY, targetRotationY, Time.deltaTime * rotationSmoothing);
        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSmoothing);

        transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
        camTransform.localPosition = new Vector3(camTransform.localPosition.x, camTransform.localPosition.y, -currentZoom);

        camTransform.LookAt(transform.position);
    }
}
