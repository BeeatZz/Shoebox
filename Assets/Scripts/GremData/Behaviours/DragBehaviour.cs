using UnityEngine;
using UnityEngine.InputSystem;

public class DraggedBehavior : GremBehavior
{
    private Plane dragPlane;
    private Camera mainCam;

    public override void EnterBehavior()
    {
        mainCam = Camera.main;
        controller.SetSprite(controller.stats.visualSprite);
        dragPlane = new Plane(Vector3.up, transform.position);
    }

    public override void UpdateBehavior()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCam.ScreenPointToRay(mousePos);

        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            transform.position = Vector3.Lerp(transform.position, hitPoint, Time.deltaTime * 25f);
        }

        if (!Mouse.current.leftButton.isPressed)
        {
            controller.ChangeBehavior(GetComponent<WanderBehavior>());
        }
    }

    public override void ExitBehavior()
    {
    }
}