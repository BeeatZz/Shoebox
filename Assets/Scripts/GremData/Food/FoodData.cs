using UnityEngine;

public enum FoodEffect { None, SpeedBoost }

[CreateAssetMenu(fileName = "NewFood", menuName = "GremSystem/FoodData")]
public class FoodData : ScriptableObject
{
    public string foodName;
    public float hungerRestoreValue = 0.5f;
    public FoodEffect specialEffect = FoodEffect.None;
    public float effectDuration = 5f;
}
