using UnityEngine;

[CreateAssetMenu(fileName = "NewGremData", menuName = "GremSystem/GremData")]
public class GremData : ScriptableObject
{
    [Header("Identity")]
    public string gremName;
    public Sprite visualSprite;
    public Sprite sleepSprite;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Box Limits")]
    public Vector2 minBounds = new Vector2(-4f, -4f);
    public Vector2 maxBounds = new Vector2(4f, 4f);

    [Header("Vitals")]
    public float hungerDepletionRate = 0.05f;
    public float hungerThreshold = 0.3f;
}