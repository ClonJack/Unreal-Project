using UnityEngine;

namespace UnrealTeam.SB.Configs.AI
{
    [CreateAssetMenu(menuName = "Configs/AI/Hunger")]
    public class GoapHungerConfig : ScriptableObject
    {
        [field: SerializeField] public LayerMask FoodLayer { get; private set; }
        [field: SerializeField] public int MaxFoodsOnRadar { get; private set; } = 5;
        [field: SerializeField] public float FoodSearchDistance { get; private set; } = 10f;
        [field: SerializeField] public float FoodEatingDistance { get; private set; } = 1f;
        [field: SerializeField] public float FoodEatingRate { get; private set; } = 5f;
        [field: SerializeField] public float HungerDepletionRate { get; private set; } = 0.25f;
        [field: SerializeField] public float MaxHunger { get; private set; } = 100f;
        [field: SerializeField] public float AcceptableHunger { get; private set; } = 60f;
        [field: SerializeField] public int BaseCost { get; private set; } = 7;
    }
}