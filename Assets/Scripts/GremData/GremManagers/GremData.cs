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

    [Header("Dragging")]
    public float dragPullForce = 50f;
    public float dragFollowSpeed = 8f;
    public bool enableDragStruggle = true;
    public float dragStruggleAmount = 0.5f;
    public float dragStruggleFrequency = 10f;
    public float dragStruggleRotationAmount = 5f;

    [Header("Box Limits")]
    public Vector2 minBounds = new Vector2(-4f, -4f);
    public Vector2 maxBounds = new Vector2(4f, 4f);

    [Header("Vitals")]
    public float hungerDepletionRate = 0.05f;
    public float hungerThreshold = 0.3f;

    [Header("Energy")]
    public float energyDepletionRate = 0.02f;
    public float energyThreshold = 0.2f;

    [Header("Production")]
    public float currencyProductionValue = 1f;
    public float currencyProductionRate = 5f;

    [Header("Visuals")]
    public float squashAmount = 0.1f;
    public float squashSpeed = 10f;
    public float sleepSquashAmount = 0.05f;
    public float sleepSquashSpeed = 1f;
}
