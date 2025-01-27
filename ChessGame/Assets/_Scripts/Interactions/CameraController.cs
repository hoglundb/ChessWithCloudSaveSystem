using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 20f;

    [SerializeField] private InputActionReference activateRotateInput;
    [SerializeField] private InputActionReference lookRotationInput;
    [SerializeField] private InputActionReference zoomInput;

    private Vector2 rotationInput;
    private float currentZoom = 10f;
    private bool isRotating = false;

    private void OnEnable()
    {
        activateRotateInput.action.Enable();
        lookRotationInput.action.Enable();
        zoomInput.action.Enable();
    }

    private void OnDisable()
    {
        activateRotateInput.action.Disable();
        lookRotationInput.action.Disable();
        zoomInput.action.Disable();
    }

    private void Update()
    {
        HandleRotation();
        HandleZoom();
    }

    private void HandleRotation()
    {
        // Check if the activateRotateInput is being held down
        isRotating = activateRotateInput.action.ReadValue<float>() > 0.5f;

        if (isRotating)
        {
            // Read look rotation input only when the rotate button is held down
            rotationInput = lookRotationInput.action.ReadValue<Vector2>();

            // Rotate around the target based on input
            if (rotationInput != Vector2.zero)
            {
                float horizontal = rotationInput.x * rotationSpeed * Time.deltaTime;
                float vertical = rotationInput.y * rotationSpeed * Time.deltaTime;

                transform.RotateAround(target.position, Vector3.up, horizontal);
                transform.RotateAround(target.position, transform.right, -vertical);
            }
        }
    }

    private void HandleZoom()
    {
        float zoomDelta = zoomInput.action.ReadValue<float>();
        currentZoom -= zoomDelta * zoomSpeed * Time.deltaTime;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        Vector3 direction = (transform.position - target.position).normalized;
        transform.position = target.position + direction * currentZoom;
    }
}
