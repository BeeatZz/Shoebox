using UnityEngine;

public class GremCurrencyProducer : MonoBehaviour
{
    private GremBT gremBT;
    private float productionTimer;

    void Awake()
    {
        gremBT = GetComponent<GremBT>();
        productionTimer = 0f;
    }

    void Update()
    {
        if (gremBT == null || gremBT.isSleeping)
        {
            productionTimer = 0f;
            return;
        }

        float currentProductionRate = gremBT.stats.currencyProductionRate;

        if (gremBT.hunger < gremBT.stats.hungerThreshold)
        {
            currentProductionRate *= 1.5f;
        }
        if (gremBT.energy < gremBT.stats.energyThreshold)
        {
            currentProductionRate *= 1.5f;
        }

        productionTimer += Time.deltaTime;

        if (productionTimer >= currentProductionRate)
        {
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.AddCurrency((int)gremBT.stats.currencyProductionValue);
            }
            else
            {
                Debug.LogError("[GremCurrencyProducer] CurrencyManager.Instance is null! Is it set up in the scene?");
            }
            productionTimer = 0f;
        }
    }
}
