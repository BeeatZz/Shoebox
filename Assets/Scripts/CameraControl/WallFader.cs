using UnityEngine;
using System.Collections.Generic;

public class WallFader : MonoBehaviour
{
    public Transform pivot; // Drag Camera_Pivot here
    public float fadeSpeed = 5f;
    private List<WallTransparency> hitWalls = new List<WallTransparency>();

    void Update()
    {
        // 1. Aim a laser from camera to pivot
        Vector3 direction = pivot.position - transform.position;
        float distance = Vector3.Distance(transform.position, pivot.position);

        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance);

        // 2. Clear old hits
        foreach (var wall in hitWalls) wall.shouldFade = false;
        hitWalls.Clear();

        // 3. Mark new hits
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent<WallTransparency>(out var wall))
            {
                wall.shouldFade = true;
                hitWalls.Add(wall);
            }
        }
    }
}