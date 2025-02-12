using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private const float LEVEL_MODIFIER = 0.5f;

    [SerializeField] EnemyInfo[] enemiesData;
    [SerializeField] List<Enemy> currentEnemies;

    void Awake()
    {
        GenerateEnemy("Slime", 1);
    }

    public void GenerateEnemy(string name, int level)
    {
        foreach (var data in enemiesData)
        {
            if (data.Name == name)
            {
                float levelModifier = LEVEL_MODIFIER * level;

                Enemy newEnemy = new()
                {
                    Name = name,
                    Level = level,
                    MaxHealth = Mathf.RoundToInt(data.BaseHealth + data.BaseHealth * levelModifier),
                    CurrentHealth = Mathf.RoundToInt(data.BaseHealth + data.BaseHealth * levelModifier),
                    Strength = Mathf.RoundToInt(data.BaseStrength + data.BaseStrength * levelModifier),
                    Speed = Mathf.RoundToInt(data.BaseSpeed + data.BaseSpeed * levelModifier),
                    BattleVisualPrefab = data.BattleVisualPrefab
                };

                currentEnemies.Add(newEnemy);
            }
        }
    }

    public List<Enemy> GetCurrentEnemies() => currentEnemies;
}

[Serializable]
public class Enemy : IBattler
{
    public string Name;
    public int Level { get; set; }
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
    public GameObject BattleVisualPrefab { get; set; }
    public int Strength;
    public int Speed;
}
