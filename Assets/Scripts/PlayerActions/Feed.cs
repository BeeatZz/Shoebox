using UnityEngine;
using UnityEngine.InputSystem;

public class Feed : MonoBehaviour
{
    public GameObject marshmallowPrefab;
    public float spawnHeight = 2f;
    public LayerMask floorLayer;
    public LayerMask gremLayer; // Add this in the Inspector (select the layer your Grems are on)
    public float castRange = 100f;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SpawnAtMouse();
        }
    }

    void SpawnAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        // Combine layers so the ray only "sees" Grems and the Floor, ignoring walls
        LayerMask combinedMask = floorLayer | gremLayer;

        if (Physics.Raycast(ray, out hit, castRange, combinedMask))
        {
            // If we hit a Grem, stop everything (don't spawn food)
            if (((1 << hit.collider.gameObject.layer) & gremLayer) != 0)
            {
                return;
            }

            // If we hit the floor, spawn the food
            if (((1 << hit.collider.gameObject.layer) & floorLayer) != 0)
            {
                Vector3 spawnPos = hit.point + Vector3.up * spawnHeight;
                Instantiate(marshmallowPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}