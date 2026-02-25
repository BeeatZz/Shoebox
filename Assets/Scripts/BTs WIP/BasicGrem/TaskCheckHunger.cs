public class TaskCheckHunger : Node
{
    private GremBT agent;
    public TaskCheckHunger(GremBT agent) { this.agent = agent; }
    public override NodeState Evaluate() => agent.hunger < agent.stats.hungerThreshold ? NodeState.Success : NodeState.Failure;
}