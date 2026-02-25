using UnityEngine;
using UnityEngine.InputSystem;

public class TaskCheckDragging : Node
{
    private GremBT agent;
    private Camera mainCam;
    private Plane dragPlane;
    private bool isBeingDragged;

    public TaskCheckDragging(GremBT agent) { this.agent = agent; mainCam = Camera.main; }

    public override NodeState Evaluate()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == agent.gameObject)
                {
                    isBeingDragged = true;
                    dragPlane = new Plane(Vector3.up, agent.transform.position);
                    agent.isSleeping = false;
                }
            }
        }

        if (isBeingDragged)
        {
            if (!Mouse.current.leftButton.isPressed)
            {
                isBeingDragged = false;
                return NodeState.Failure;
            }

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray dragRay = mainCam.ScreenPointToRay(mousePos);
            if (dragPlane.Raycast(dragRay, out float enter))
            {
                Vector3 hitPoint = dragRay.GetPoint(enter);
                agent.transform.position = Vector3.Lerp(agent.transform.position, hitPoint, Time.deltaTime * 25f);
            }
            return NodeState.Running;
        }
        return NodeState.Failure;
    }
}