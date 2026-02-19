using UnityEngine;

public class SpeedBoostAbility : GremAbility
{
    public float multiplier = 2f;
    public float duration = 10f;
    private float timer;

    public override void Initialize(GremController ctrl)
    {
        base.Initialize(ctrl);
        timer = duration;
        controller.SetSpeedMultiplier(multiplier);
    }

    public override void ExecuteAbility()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            controller.SetSpeedMultiplier(1f);
            controller.RemoveAbility<SpeedBoostAbility>();
        }
    }
}