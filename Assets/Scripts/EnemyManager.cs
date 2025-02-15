using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private const float LEVEL_MODIFIER = 0.5f;

    [SerializeField] EnemyInfo[] enemiesData;
    [SerializeField] List<Enemy> currentEnemies;

    static EnemyManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void GenerateEnemiesByEncounter(Encounter[] encounters, int maxNumberOfEnemies)
    {
        currentEnemies.Clear();
        int numberOfEnemies = UnityEngine.Random.Range(1, maxNumberOfEnemies + 1);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Encounter encounter = encounters[UnityEngine.Random.Range(0, encounters.Length)];
            int level = UnityEngine.Random.Range(encounter.MinLevel, encounter.MaxLevel + 1);
            GenerateEnemyByName(encounter.Enemy.Name, level);
        }
    }

    public void GenerateEnemyByName(string name, int level)
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
