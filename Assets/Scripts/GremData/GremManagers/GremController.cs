using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GremController : MonoBehaviour
{
    public GremData stats;
    public GremBehavior currentBehavior;
    public List<GremAbility> activeAbilities = new List<GremAbility>();

    [Range(0, 1)] public float hunger = 1.0f;
    private SpriteRenderer spriteRenderer;
    private float speedMultiplier = 1f;
    private Camera mainCam;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        mainCam = Camera.main;
    }

    void Start()
    {
        if (stats != null) Initialize(stats);

        foreach (var behavior in GetComponents<GremBehavior>())
        {
            behavior.enabled = false;
        }

        WanderBehavior wander = GetComponent<WanderBehavior>();
        if (wander != null) ChangeBehavior(wander);
    }

    public void Initialize(GremData newData)
    {
        stats = newData;
        spriteRenderer.sprite = stats.visualSprite;
        this.name = "Grem_" + stats.gremName;
    }

    void Update()
    {
        HandleInput();

        hunger -= stats.hungerDepletionRate * Time.deltaTime;
        hunger = Mathf.Clamp01(hunger);

        if (currentBehavior != null)
        {
            currentBehavior.UpdateBehavior();
        }

        for (int i = activeAbilities.Count - 1; i >= 0; i--)
        {
            if (activeAbilities[i] != null)
                activeAbilities[i].ExecuteAbility();
        }
    }

    private void HandleInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = mainCam.ScreenPointToRay(mousePos);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform))
                {
                    DraggedBehavior drag = GetComponent<DraggedBehavior>();
                    if (drag != null)
                    {
                        ChangeBehavior(drag);
                        break;
                    }
                }
            }
        }
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward,
                         mainCam.transform.rotation * Vector3.up);
    }

    public void ChangeBehavior(GremBehavior newBehavior)
    {
        if (newBehavior == null || newBehavior == currentBehavior) return;

        if (currentBehavior != null)
        {
            currentBehavior.ExitBehavior();
            currentBehavior.enabled = false;
        }

        currentBehavior = newBehavior;
        currentBehavior.enabled = true;
        currentBehavior.EnterBehavior();
    }

    public void AddAbility<T>() where T : GremAbility
    {
        T existing = GetComponent<T>();
        if (existing != null) return;

        T newAbility = gameObject.AddComponent<T>();
        newAbility.Initialize(this);
        activeAbilities.Add(newAbility);
    }

    public void RemoveAbility<T>() where T : GremAbility
    {
        T ability = GetComponent<T>();
        if (ability != null)
        {
            activeAbilities.Remove(ability);
            Destroy(ability);
        }
    }

    public float GetCurrentSpeed()
    {
        return stats.moveSpeed * speedMultiplier;
    }

    public void SetSpeedMultiplier(float m)
    {
        speedMultiplier = m;
    }

    public void SetSprite(Sprite newSprite)
    {
        if (spriteRenderer != null && newSprite != null)
            spriteRenderer.sprite = newSprite;
    }
}