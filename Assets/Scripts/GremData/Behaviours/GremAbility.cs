using UnityEngine;

public abstract class GremAbility : MonoBehaviour
{
    protected GremController controller;

    public virtual void Initialize(GremController ctrl)
    {
        controller = ctrl;
    }

    public abstract void ExecuteAbility();
}