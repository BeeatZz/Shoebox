using UnityEngine;

public class WanderBehavior : GremBehavior
{
    private Vector3 targetPos;
    private float waitTimer;
    private bool isWaiting = false;
    public float pauseTime = 2f;

    public override void EnterBehavior()
    {
        controller.SetSprite(controller.stats.visualSprite);
        SetNewTarget();
    }

    public override void UpdateBehavior()
    {
        if (controller.hunger < controller.stats.hungerThreshold)
        {
            controller.ChangeBehavior(GetComponent<ScentBehavior>());
            return;
        }

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= pauseTime)
            {
                isWaiting = false;
                waitTimer = 0;
                SetNewTarget();
            }
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, controller.GetCurrentSpeed() * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            isWaiting = true;
        }
    }

    public override void ExitBehavior()
    {
        isWaiting = false;
        waitTimer = 0;
    }

    private void SetNewTarget()
    {
        float randomX = Random.Range(controller.stats.minBounds.x, controller.stats.maxBounds.x);
        float randomZ = Random.Range(controller.stats.minBounds.y, controller.stats.maxBounds.y);
        targetPos = new Vector3(randomX, transform.position.y, randomZ);
    }
}