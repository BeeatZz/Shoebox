using UnityEngine;
using System.Collections.Generic;

public class GremBT : MonoBehaviour
{
    public GremData stats;
    public float hunger = 1f;
    public float energy = 1f;
    public bool isSleeping = false;

    [HideInInspector] public Transform targetFood;
    private Node _root;

    void Start()
    {
        _root = SetupTree();
    }

    void Update()
    {
        hunger = Mathf.Clamp01(hunger - stats.hungerDepletionRate * Time.deltaTime);

        if (!isSleeping)
            energy = Mathf.Clamp01(energy - stats.energyDepletionRate * Time.deltaTime);

        if (_root != null) _root.Evaluate();
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
        var sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null && s != null) sr.sprite = s;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                         Camera.main.transform.rotation * Vector3.up);
    }
}