using UnityEngine;

public abstract class GremBehavior : MonoBehaviour
{
    protected GremController controller;

    public virtual void Awake()
    {
        controller = GetComponent<GremController>();
    }

    public abstract void EnterBehavior();
    public abstract void UpdateBehavior();
    public abstract void ExitBehavior();
}
