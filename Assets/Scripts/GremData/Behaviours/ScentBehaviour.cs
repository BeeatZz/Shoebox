using UnityEngine;
using UnityEngine.UI;

public class ScentBehavior : GremBehavior
{
    private Transform targetFood;
    public float detectRadius = 10f;
    public float foodConsumptionTime = 1f;
    public float postEatWaitTime = 1.5f;

    private float currentTimer = 0f;
    private bool isEating = false;
    private bool isWaitingAfterEat = false;

    public override void EnterBehavior()
    {
        controller.SetSprite(controller.stats.visualSprite);
        isEating = false;
        isWaitingAfterEat = false;
        currentTimer = 0f;
        FindFood();
    }

    public override void UpdateBehavior()
    {
        if (controller.hunger >= 0.9f && !isEating && !isWaitingAfterEat)
        {
            controller.ChangeBehavior(GetComponent<WanderBehavior>());
            return;
        }

        if (isWaitingAfterEat)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer >= postEatWaitTime)
            {
                if (Random.value > 0.5f)
                    controller.ChangeBehavior(GetComponent<SleepBehavior>());
                else
                    controller.ChangeBehavior(GetComponent<WanderBehavior>());
            }
            return;
        }

        if (isEating)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer >= foodConsumptionTime)
            {
                FinishEating();
            }
            return;
        }

        if (targetFood == null)
        {
            FindFood();
            return;
        }

        Vector3 destination = new Vector3(targetFood.position.x, transform.position.y, targetFood.position.z);
        transform.position = Vector3.MoveTowards(transform.position, destination, controller.GetCurrentSpeed() * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.2f)
        {
            StartEating();
        }
    }

    private void FindFood()
    {
        GameObject[] foodItems = GameObject.FindGameObjectsWithTag("Food");
        float closestDist = detectRadius;

        foreach (GameObject food in foodItems)
        {
            float dist = Vector3.Distance(transform.position, food.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                targetFood = food.transform;
            }
        }
    }

    private void StartEating()
    {
        isEating = true;
        currentTimer = 0f;
    }

    private void FinishEating()
    {
        isEating = false;
        if (targetFood != null)
        {
            FoodItem item = targetFood.GetComponent<FoodItem>();
            if (item != null)
            {
                ApplyFoodData(item.data);
            }
            Destroy(targetFood.gameObject);
        }
        controller.hunger = 1.0f;
        isWaitingAfterEat = true;
        currentTimer = 0f;
    }

    private void ApplyFoodData(FoodData data)
    {
        if (data.specialEffect == FoodEffect.SpeedBoost)
        {
            controller.AddAbility<SpeedBoostAbility>();
        }
    }

    public override void ExitBehavior()
    {
        targetFood = null;
        isEating = false;
        isWaitingAfterEat = false;
    }
}