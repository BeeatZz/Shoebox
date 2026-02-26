using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int _currentCurrency = 0;
    public int CurrentCurrency
    {
        get { return _currentCurrency; }
        private set
        {
            _currentCurrency = value;
            Debug.Log($"Current Currency: {_currentCurrency}");
        }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddCurrency(int amount)
    {
        if (amount > 0)
        {
            CurrentCurrency += amount;
        }
    }

    public bool SpendCurrency(int amount)
    {
        if (amount > 0 && CurrentCurrency >= amount)
        {
            CurrentCurrency -= amount;
            return true;
        }
        return false;
    }
}
