using System.Collections.Generic;

public class Sequence : Node
{
    protected List<Node> children = new List<Node>();
    private int currentChildIndex = 0; 
    public Sequence(List<Node> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        for (int i = currentChildIndex; i < children.Count; i++)
        {
            NodeState state = children[i].Evaluate();

            switch (state)
            {
                case NodeState.Running:
                    currentChildIndex = i;
                    return NodeState.Running;
                case NodeState.Success:
                    continue; 
                case NodeState.Failure:
                    currentChildIndex = 0; 
                    return NodeState.Failure;
            }
        }

        currentChildIndex = 0; 
        return NodeState.Success;
    }
}