using UnityEngine;

public class TaskFindFood : Node
{
    private GremBT agent;
    public TaskFindFood(GremBT agent) { this.agent = agent; }

    public override NodeState Evaluate()
    {
        GameObject[] foodItems = GameObject.FindGameObjectsWithTag("Food");
        if (foodItems.Length > 0)
        {
            agent.targetFood = foodItems[0].transform;
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
