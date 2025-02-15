using UnityEngine;

public class EncounterSystem : MonoBehaviour
{
    [SerializeField] Encounter[] enemiesInScene;
    [SerializeField] int maxNumberOfEnemies = 3;

    EnemyManager enemyManager;

    void Awake()
    {
        enemyManager = FindFirstObjectByType<EnemyManager>();
    }

    void Start()
    {
        enemyManager.GenerateEnemiesByEncounter(enemiesInScene, maxNumberOfEnemies);
    }
}

[System.Serializable]
public class Encounter
{
    public EnemyInfo Enemy;
    public int MinLevel;
    public int MaxLevel;
}
