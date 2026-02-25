using UnityEngine;

public class TaskSleep : Node
{
    private GremBT agent;
    public TaskSleep(GremBT agent) { this.agent = agent; }

    public override NodeState Evaluate()
    {
        if (agent.isSleeping)
        {
            agent.energy += Time.deltaTime * 0.2f;
            agent.SetSprite(agent.stats.sleepSprite);

            if (agent.energy >= 1f)
            {
                agent.energy = 1f;
                agent.isSleeping = false;
                agent.SetSprite(agent.stats.visualSprite);
                return NodeState.Success;
            }
            return NodeState.Running;
        }

        if (agent.energy < agent.stats.energyThreshold)
        {
            agent.isSleeping = true;
            return NodeState.Running;
        }

        return NodeState.Failure;
    }
}