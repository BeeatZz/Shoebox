using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerHook : MonoBehaviour
{
    [Header("Feeding System")]
    [Tooltip("The food prefab to spawn (e.g. Marshmallow).")]
    public GameObject foodPrefab;
    [Tooltip("The layer(s) to spawn food on. Defaults to 'BoxFloor' if not set.")]
    public LayerMask floorLayer = -1; // Default to Everything
    public float spawnHeightOffset = 0.5f;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        
        // Auto-configure layer mask if it's set to Everything (-1) or Nothing (0)
        if (floorLayer.value == -1 || floorLayer.value == 0)
        {
            int boxFloorIndex = LayerMask.NameToLayer("BoxFloor");
            if (boxFloorIndex != -1)
            {
                floorLayer = 1 << boxFloorIndex;
                Debug.Log($"InputManagerHook: Auto-set FloorLayer to 'BoxFloor'");
            }
        }

        if (foodPrefab == null)
        {
            Debug.LogWarning("InputManagerHook: Food Prefab is missing! Assign it in the Inspector to enable feeding.");
        }
    }

    void Update()
    {
        // Handle global toggles (like 'F' for feeding mode)
        InputManager.HandleGlobalInput();

        // Handle Feeding Click
        if (InputManager.IsFeedingEnabled && Mouse.current.leftButton.wasPressedThisFrame)
        {
            TrySpawnFood();
        }
    }

    private void TrySpawnFood()
    {
        if (foodPrefab == null)
        {
            Debug.LogError("Cannot spawn food: No Food Prefab assigned in InputManagerHook!");
            return;
        }

        if (mainCam == null) mainCam = Camera.main;

        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, floorLayer))
        {
            // Don't spawn on top of Grems or existing Food if the layer mask isn't strict enough
            if (hit.collider.CompareTag("Grem") || hit.collider.CompareTag("Food")) return;

            Vector3 spawnPos = hit.point + Vector3.up * spawnHeightOffset;
            Instantiate(foodPrefab, spawnPos, Quaternion.identity);
            
            // Optional: Add a small random rotation for variety
            // Instantiate(foodPrefab, spawnPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
        }
    }
}
