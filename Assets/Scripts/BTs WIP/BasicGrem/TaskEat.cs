using UnityEngine;

public class TaskEat : Node
{
    private GremBT agent;
    private float timer;
    private Transform currentTarget;

    public TaskEat(GremBT agent) { this.agent = agent; }

    public override NodeState Evaluate()
    {
        if (agent.targetFood == null)
        {
            timer = 0;
            currentTarget = null;
            return NodeState.Failure;
        }

        if (currentTarget != agent.targetFood)
        {
            currentTarget = agent.targetFood;
            timer = 0;
        }

        timer += Time.deltaTime;

        if (timer < 2.0f) 
        {
            return NodeState.Running;
        }

        if (agent.targetFood != null)
        {
            Object.Destroy(agent.targetFood.gameObject);
        }

        agent.hunger = 1f;
        timer = 0;
        currentTarget = null;
        return NodeState.Success;
    }
}
