using UnityEngine;

public class SleepBehavior : GremBehavior
{
    public float sleepDuration = 5f;
    private float sleepTimer;

    public override void EnterBehavior()
    {
        sleepTimer = 0;
        controller.SetSprite(controller.stats.sleepSprite);
    }

    public override void UpdateBehavior()
    {
        sleepTimer += Time.deltaTime;

        if (sleepTimer >= sleepDuration || controller.hunger < controller.stats.hungerThreshold)
        {
            controller.ChangeBehavior(GetComponent<WanderBehavior>());
        }
    }

    public override void ExitBehavior()
    {
        controller.SetSprite(controller.stats.visualSprite);
    }
}