using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class FurniturePlacementManager : MonoBehaviour
{
    public LayerMask floorLayer;
    public GameObject[] furniturePrefabs;
    public float placementHeightOffset = 0.1f;

    private GameObject currentFurnitureInstance;
    private int currentPrefabIndex = -1;
    private bool isPlacing = false;
    private Camera mainCam;


    void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        HandleInput();

        if (isPlacing && currentFurnitureInstance != null)
        {
            MoveCurrentFurnitureWithMouse();

            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                currentFurnitureInstance.transform.Rotate(Vector3.up, 90f);
            }
        }
    }

    void HandleInput()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            if (isPlacing)
            {
                CancelPlacement();
            }
            Debug.Log("Furniture menu key 'B' pressed. (UI interaction expected here)");
        }

        if (isPlacing && Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlaceFurniture();
        }

        if (isPlacing && Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelPlacement();
        }
    }

    public void SelectFurniturePrefab(int index)
    {
        if (index < 0 || index >= furniturePrefabs.Length || furniturePrefabs[index] == null)
        {
            Debug.LogError("Invalid furniture prefab index selected.");
            return;
        }

        if (isPlacing) CancelPlacement();

        currentPrefabIndex = index;
        currentFurnitureInstance = Instantiate(furniturePrefabs[currentPrefabIndex]);
        isPlacing = true;
        Debug.Log($"Selected {furniturePrefabs[currentPrefabIndex].name} for placement.");
    }

    void MoveCurrentFurnitureWithMouse()
    {
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, floorLayer))
        {
            currentFurnitureInstance.transform.position = hit.point + Vector3.up * placementHeightOffset;
        }
    }

    void PlaceFurniture()
    {
        if (currentFurnitureInstance != null)
        {
            Debug.Log($"Placed {currentFurnitureInstance.name}");
            currentFurnitureInstance = null;
            isPlacing = false;
            currentPrefabIndex = -1;
        }
    }

    void CancelPlacement()
    {
        if (currentFurnitureInstance != null)
        {
            Destroy(currentFurnitureInstance);
            Debug.Log("Placement cancelled.");
        }
        currentFurnitureInstance = null;
        isPlacing = false;
        currentPrefabIndex = -1;
    }
}
