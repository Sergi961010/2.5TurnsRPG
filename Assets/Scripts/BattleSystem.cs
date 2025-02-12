using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] Transform[] partySpawnPoints;
    [SerializeField] Transform[] enemySpawnPoints;

    [Header("Battlers")]
    [SerializeField] List<BattleEntity> allBattlers = new();
    [SerializeField] List<BattleEntity> enemyBattlers = new();
    [SerializeField] List<BattleEntity> playerBattlers = new();

    PartyManager partyManager;
    EnemyManager enemyManager;
    void Start()
    {
        partyManager = FindFirstObjectByType<PartyManager>();
        enemyManager = FindFirstObjectByType<EnemyManager>();

        CreatePartyEntities();
        CreateEnemyEntities();
    }

    void CreatePartyEntities()
    {
        List<PartyMember> currentParty = partyManager.GetCurrentPartyMembers();
        for (int i = 0; i < currentParty.Count; i++)
        {
            BattleEntity newEntity = new()
            {
                Name = currentParty[i].Name,
                CurrentHealth = currentParty[i].CurrentHealth,
                MaxHealth = currentParty[i].MaxHealth,
                Speed = currentParty[i].Speed,
                Strength = currentParty[i].Strength,
                Level = currentParty[i].Level,
                IsPlayer = true,
                BattleVisual = SpawnVisual(currentParty[i], partySpawnPoints[i])
            };


            allBattlers.Add(newEntity);
            playerBattlers.Add(newEntity);
        }
    }

    private BattleVisuals SpawnVisual(IBattler partyMember, Transform transform)
    {
        BattleVisuals visual = Instantiate(partyMember.BattleVisualPrefab, transform.position, transform.rotation).GetComponent<BattleVisuals>();
        visual.SetStartingValues(partyMember.CurrentHealth, partyMember.MaxHealth, partyMember.Level);

        return visual;
    }

    void CreateEnemyEntities()
    {
        List<Enemy> currentEnemies = enemyManager.GetCurrentEnemies();
        for (int i = 0; i < currentEnemies.Count; i++)
        {
            BattleEntity newEntity = new()
            {
                Name = currentEnemies[i].Name,
                CurrentHealth = currentEnemies[i].CurrentHealth,
                MaxHealth = currentEnemies[i].MaxHealth,
                Speed = currentEnemies[i].Speed,
                Strength = currentEnemies[i].Strength,
                Level = currentEnemies[i].Level,
                IsPlayer = true,
                BattleVisual = SpawnVisual(currentEnemies[i], enemySpawnPoints[i])
            };


            allBattlers.Add(newEntity);
            enemyBattlers.Add(newEntity);
        }
    }
}

[Serializable]
public class BattleEntity
{
    public string Name;
    public int CurrentHealth;
    public int MaxHealth;
    public int Speed;
    public int Strength;
    public int Level;
    public bool IsPlayer;
    public BattleVisuals BattleVisual;
}
