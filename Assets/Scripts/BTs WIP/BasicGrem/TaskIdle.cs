using UnityEngine;

public class TaskIdle : Node
{
    private GremBT agent;
    private float idleDuration;
    private float timer;
    private bool isIdling;

    public TaskIdle(GremBT agent) { this.agent = agent; }

    public override NodeState Evaluate()
    {
        if (!isIdling)
        {
            idleDuration = Random.Range(2f, 4f);
            timer = 0;
            isIdling = true;
        }

        timer += Time.deltaTime;

        if (timer >= idleDuration)
        {
            isIdling = false; 
            timer = 0;        
            return NodeState.Success;
        }

        return NodeState.Running;
    }
}
