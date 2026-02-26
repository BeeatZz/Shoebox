using UnityEngine;

public class TaskMoveToFood : Node
{
    private GremBT agent;
    public TaskMoveToFood(GremBT agent) { this.agent = agent; }

    public override NodeState Evaluate()
    {
        if (agent.targetFood == null)
        {
            agent.ResetSpriteScale();
            return NodeState.Failure;
        }

        Vector3 destination = new Vector3(agent.targetFood.position.x, agent.transform.position.y, agent.targetFood.position.z);
        float currentSpeed = agent.stats.moveSpeed;

        if (agent.hunger < agent.stats.hungerThreshold)
        {
            currentSpeed *= 0.5f; 
        }

        agent.transform.position = Vector3.MoveTowards(
            agent.transform.position,
            destination,
            currentSpeed * Time.deltaTime
        );
        
        agent.FlipSpriteToTarget(destination);

        agent.ApplySquashAndSquishEffect(Time.time, agent.stats.squashAmount, agent.stats.squashSpeed);

        if (Vector3.Distance(agent.transform.position, destination) < 0.2f)
        {
            agent.ResetSpriteScale();
            return NodeState.Success;
        }

        return NodeState.Running;
    }
}
