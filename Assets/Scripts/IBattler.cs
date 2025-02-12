using UnityEngine;

public interface IBattler
{
    GameObject BattleVisualPrefab { get; }
    int CurrentHealth { get; }
    int MaxHealth { get; }
    int Level { get; }
}
