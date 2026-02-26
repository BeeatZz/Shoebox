using UnityEngine;
using System.Collections.Generic;

// this script controls the grems ai using a behavior tree

[RequireComponent(typeof(Rigidbody))]
public class GremBT : MonoBehaviour
{
    public GremData stats;
    public float hunger = 1f;
    public float energy = 1f;
    public bool isSleeping = false;
    public bool isBeingDragged = false;
    public Vector3 dragTargetPosition;

    // How long the Grem idles after being dropped before resuming normal behaviour
    [Tooltip("How long (seconds) the Grem pauses after being dropped.")]
    public float postDragIdleDuration = 1f;
    [HideInInspector] public float postDragIdleTimer = 0f;

    [HideInInspector] public Transform targetFood;
    public GameObject zzzPrefab;

    [Tooltip("Globally enable/disable squish and squash effect for all Grems.")]
    [SerializeField] private bool _enableSquashAndSquish = true;
    public static bool EnableSquashAndSquish { get; private set; } = true;

    [Tooltip("The layer(s) the Grem's collider is on. Used for raycasting during dragging.")]
    public LayerMask gremLayer;

    private Node _root;
    private Transform spriteTransform;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody _rigidbody;

    public Rigidbody GremRigidbody => _rigidbody;

    void Awake()
    {
        GremBT.EnableSquashAndSquish = _enableSquashAndSquish;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
        {
            spriteTransform = _spriteRenderer.transform;
        }
        else
        {
            Debug.LogError("[GremBT.Awake] Could not find SpriteRenderer on the Grem's root GameObject.");
        }

        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("[GremBT.Awake] Rigidbody not found on Grem's root GameObject.");
        }
        else
        {
            _rigidbody.freezeRotation = true;
            // ContinuousDynamic makes Unity sweep-test every physics step,
            // preventing the Grem from tunnelling through walls at high drag speeds.
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
    }

    void Start()
    {
        _root = SetupTree();
    }

    void Update()
    {
        hunger = Mathf.Clamp01(hunger - stats.hungerDepletionRate * Time.deltaTime);

        if (!isSleeping)
            energy = Mathf.Clamp01(energy - stats.energyDepletionRate * Time.deltaTime);

        // Tick down the post-drag idle timer
        if (postDragIdleTimer > 0f)
            postDragIdleTimer -= Time.deltaTime;

        if (_root != null) _root.Evaluate();
    }

    void FixedUpdate()
    {
        if (isBeingDragged && _rigidbody != null)
        {
            Vector3 targetPos = dragTargetPosition;

            if (stats.enableDragStruggle)
            {
                // Create a random offset using Perlin noise for smooth jitter
                float time = Time.time * stats.dragStruggleFrequency;
                // Use a large offset for the second noise sample to ensure they are uncorrelated
                float jitterX = (Mathf.PerlinNoise(time, 0f) - 0.5f) * 2f * stats.dragStruggleAmount;
                float jitterY = (Mathf.PerlinNoise(0f, time) - 0.5f) * 2f * stats.dragStruggleAmount;
                targetPos += new Vector3(jitterX, jitterY, 0f);
            }

            // Drive the Rigidbody by velocity rather than teleporting via MovePosition.
            // Velocity scales with distance so movement is smooth and naturally slows near the cursor.
            // Because the Rigidbody is moving via velocity, ContinuousDynamic collision detection
            // correctly sweeps and stops the Grem at walls instead of passing through them.
            Vector3 toTarget = targetPos - _rigidbody.position;
            _rigidbody.linearVelocity = toTarget * stats.dragFollowSpeed;
        }
    }

    private Node SetupTree()
    {
        TaskCheckDragging dragTask = new TaskCheckDragging(this);

        Sequence hungerSequence = new Sequence(new List<Node> {
            new TaskCheckHunger(this),
            new TaskFindFood(this),
            new TaskMoveToFood(this),
            new TaskEat(this)
        });

        TaskSleep sleepTask = new TaskSleep(this);

        Sequence moveSequence = new Sequence(new List<Node> {
            new TaskWander(this),
            new TaskIdle(this)
        });

        return new Selector(new List<Node> {
            dragTask,
            sleepTask,
            hungerSequence,
            moveSequence
        });
    }

    public void SetSprite(Sprite s)
    {
        if (_spriteRenderer != null && s != null) _spriteRenderer.sprite = s;
    }

    public void ApplySquashAndSquishEffect(float progress, float amount, float speed)
    {
        if (spriteTransform == null) return;
        if (GremBT.EnableSquashAndSquish == false) return;

        float t = (Mathf.Sin(progress * speed) + 1f) * 0.5f;
        float scaleY = 1f - amount + (amount * 2f * t);
        float scaleX = 1f + amount - (amount * t);
        spriteTransform.localScale = new Vector3(scaleX, scaleY, 1f);
    }

    public void ResetSpriteScale()
    {
        if (spriteTransform == null) return;
        spriteTransform.localScale = Vector3.one;
    }

    public void FlipSpriteToTarget(Vector3 targetWorldPosition)
    {
        if (_spriteRenderer == null) return;

        float directionX = targetWorldPosition.x - transform.position.x;

        if (Mathf.Abs(directionX) > 0.01f)
        {
            if (directionX > 0 && _spriteRenderer.flipX == false)
                _spriteRenderer.flipX = true;
            else if (directionX < 0 && _spriteRenderer.flipX == true)
                _spriteRenderer.flipX = false;
        }
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                         Camera.main.transform.rotation * Vector3.up);
    }
}