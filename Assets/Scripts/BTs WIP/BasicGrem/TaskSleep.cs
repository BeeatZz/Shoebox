using UnityEngine;

public class TaskSleep : Node
{
    private GremBT agent;
    private GameObject currentZzzEffect;

    public TaskSleep(GremBT agent) { this.agent = agent; }

    public override NodeState Evaluate()
    {
        if (agent.isSleeping)
        {
            if (currentZzzEffect == null && agent.zzzPrefab != null)
            {
                Vector3 zzzPos = agent.transform.position + Vector3.up * 0.5f;
                currentZzzEffect = GameObject.Instantiate(agent.zzzPrefab, zzzPos, Quaternion.identity, agent.transform);
            }

            agent.ApplySquashAndSquishEffect(Time.time, agent.stats.sleepSquashAmount, agent.stats.sleepSquashSpeed);

            agent.energy += Time.deltaTime * 0.2f;
            agent.SetSprite(agent.stats.sleepSprite);

            if (agent.energy >= 1f)
            {
                agent.energy = 1f;
                agent.isSleeping = false;
                agent.SetSprite(agent.stats.visualSprite);
                agent.ResetSpriteScale();
                
                if (currentZzzEffect != null)
                {
                    GameObject.Destroy(currentZzzEffect);
                    currentZzzEffect = null;
                }
                return NodeState.Success;
            }
            return NodeState.Running;
        }

        if (agent.energy < agent.stats.energyThreshold)
        {
            agent.isSleeping = true;
            return NodeState.Running;
        }

        if (currentZzzEffect != null)
        {
            GameObject.Destroy(currentZzzEffect);
            currentZzzEffect = null;
        }
        agent.ResetSpriteScale();

        return NodeState.Failure;
    }
}
