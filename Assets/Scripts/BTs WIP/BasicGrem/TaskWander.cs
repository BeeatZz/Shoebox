using UnityEngine;

public class TaskWander : Node
{
    private GremBT agent;
    private Vector3 targetPos;
    private bool hasTarget;
    
    public TaskWander(GremBT agent) { this.agent = agent; }

    public override NodeState Evaluate()
    {
        if (!hasTarget)
        {
            float randomX = Random.Range(agent.stats.minBounds.x, agent.stats.maxBounds.x);
            float randomZ = Random.Range(agent.stats.minBounds.y, agent.stats.maxBounds.y);
            targetPos = new Vector3(randomX, agent.transform.position.y, randomZ);
            hasTarget = true;
        }

        float currentSpeed = agent.stats.moveSpeed;

        if (agent.hunger < agent.stats.hungerThreshold)
        {
            currentSpeed *= 0.5f; 
        }

        agent.transform.position = Vector3.MoveTowards(
            agent.transform.position,
            targetPos,
            currentSpeed * Time.deltaTime
        );
        
        agent.FlipSpriteToTarget(targetPos);

        agent.ApplySquashAndSquishEffect(Time.time, agent.stats.squashAmount, agent.stats.squashSpeed);

        if (Vector3.Distance(agent.transform.position, targetPos) < 0.15f)
        {
            hasTarget = false;
            agent.ResetSpriteScale();
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    public void ResetTask()
    {
        hasTarget = false;
        agent.ResetSpriteScale();
    }
}
