using UnityEngine;
using UnityEngine.InputSystem;

// This behavior tree task handles the full dragging functionality for a Grem.
// Movement is velocity-based so ContinuousDynamic collision detection can sweep
// against walls properly - preventing the Grem from tunnelling through them.
// After being dropped, the node keeps returning Running for a short duration to
// block the rest of the tree (preventing instant wandering).
public class TaskCheckDragging : Node
{
    private GremBT agent;
    private Camera mainCam;
    private Plane dragPlane;

    public TaskCheckDragging(GremBT agent)
    {
        this.agent = agent;
        mainCam = Camera.main;
    }

    public override NodeState Evaluate()
    {
        // Dragging is disabled in feeding mode
        if (InputManager.IsFeedingEnabled)
        {
            StopDrag();
            return NodeState.Failure;
        }

        // --- Post-drop idle: block the tree until the timer expires ---
        if (!agent.isBeingDragged && agent.postDragIdleTimer > 0f)
            return NodeState.Running;

        // --- Start Drag ---
        if (!agent.isBeingDragged && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, agent.gremLayer)
                && hit.collider.gameObject == agent.gameObject)
            {
                agent.isBeingDragged = true;
                agent.isSleeping = false;
                agent.postDragIdleTimer = 0f; // Clear any leftover timer if picked up again quickly
                dragPlane = new Plane(Vector3.up, agent.transform.position);

                if (agent.GremRigidbody != null)
                {
                    agent.GremRigidbody.useGravity = false;
                    agent.GremRigidbody.linearVelocity = Vector3.zero;
                }
            }
        }

        // --- Continue or End Drag ---
        if (agent.isBeingDragged)
        {
            if (!Mouse.current.leftButton.isPressed)
            {
                StopDrag();
                return NodeState.Running; // Stay Running so the post-drop timer takes over above
            }

            // Update the target position for FixedUpdate
            Ray dragRay = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (dragPlane.Raycast(dragRay, out float enter))
                agent.dragTargetPosition = dragRay.GetPoint(enter);

            // Struggle / twitch effect
            if (GremBT.EnableSquashAndSquish && agent.stats.enableDragStruggle)
            {
                float t = Time.time * agent.stats.dragStruggleFrequency;
                agent.transform.localRotation = Quaternion.Euler(0, 0,
                    Mathf.Sin(t * 1.2f) * agent.stats.dragStruggleRotationAmount);
            }

            return NodeState.Running;
        }

        return NodeState.Failure;
    }

    private void StopDrag()
    {
        if (!agent.isBeingDragged) return;

        agent.isBeingDragged = false;
        agent.postDragIdleTimer = agent.postDragIdleDuration;
        agent.ResetSpriteScale();
        agent.transform.localRotation = Quaternion.identity;

        if (agent.GremRigidbody != null)
        {
            agent.GremRigidbody.linearVelocity = Vector3.zero;
            agent.GremRigidbody.useGravity = true;
        }
    }
}