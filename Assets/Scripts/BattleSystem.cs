using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour
{
    private const string ACTION_MESSAGE = "'s action:";
    private const float TURN_DURATION = 0.5f;
    private const string OVERWORLD_SCENE_NAME = "OverworldScene";

    [SerializeField]
    enum BattleState
    {
        Start,
        Selection,
        Battle,
        Won,
        Lost,
        Run
    }

    [Header("Battle State")]
    [SerializeField] BattleState currentState;

    [Header("Spawn Points")]
    [SerializeField] Transform[] partySpawnPoints;
    [SerializeField] Transform[] enemySpawnPoints;

    [Header("Battlers")]
    [SerializeField] List<BattleEntity> allBattlers = new();
    [SerializeField] List<BattleEntity> enemyBattlers = new();
    [SerializeField] List<BattleEntity> playerBattlers = new();

    [Header("UI")]
    [SerializeField] GameObject[] enemySelectionButtons;
    [SerializeField] GameObject battleMenu;
    [SerializeField] GameObject enemySelectionMenu;
    [SerializeField] TextMeshProUGUI actionText;
    [SerializeField] GameObject bottomPopupText;
    [SerializeField] TextMeshProUGUI bottomText;

    PartyManager partyManager;
    EnemyManager enemyManager;
    int currentPlayer;
    int currentEnemy;

    void Start()
    {
        partyManager = FindFirstObjectByType<PartyManager>();
        enemyManager = FindFirstObjectByType<EnemyManager>();

        CreatePartyEntities();
        CreateEnemyEntities();
        ShowBattleMenu();
        DetermineBattleOrder();
    }

    IEnumerator BattleRoutine()
    {
        enemySelectionMenu.SetActive(false);
        currentState = BattleState.Battle;
        bottomPopupText.SetActive(true);

        for (int i = 0; i < allBattlers.Count; i++)
        {
            if (currentState == BattleState.Battle)
            {
                switch (allBattlers[i].State)
                {
                    case BattleEntity.BattleState.Attacking:
                        yield return StartCoroutine(AttackRoutine(i));
                        break;
                    case BattleEntity.BattleState.Running:
                        yield return StartCoroutine(RunRoutine());
                        break;
                    default:
                        Debug.LogError("Invalid Battle State");
                        break;
                }
            }
        }

        if (currentState == BattleState.Battle)
        {
            bottomPopupText.SetActive(false);
            currentPlayer = 0;
            ShowBattleMenu();
        }

        yield return null;
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
                IsPlayer = false,
                BattleVisual = SpawnVisual(currentEnemies[i], enemySpawnPoints[i])
            };


            allBattlers.Add(newEntity);
            enemyBattlers.Add(newEntity);
        }
    }

    public void ShowBattleMenu()
    {
        battleMenu.SetActive(true);
        enemySelectionMenu.SetActive(false);
        actionText.text = playerBattlers[currentPlayer].Name + ACTION_MESSAGE;
    }

    public void ShowEnemySelectionMenu()
    {
        battleMenu.SetActive(false);
        SetEnemySelectionButtons();
        enemySelectionMenu.SetActive(true);
    }

    public void SelectEnemy(int target)
    {
        BattleEntity player = playerBattlers[currentPlayer];
        player.Target = allBattlers.IndexOf(enemyBattlers[target]);
        player.State = BattleEntity.BattleState.Attacking;
        currentPlayer++;

        if (currentPlayer >= playerBattlers.Count)
        {
            StartCoroutine(BattleRoutine());
        }
        else
        {
            ShowBattleMenu();
        }
    }

    public void SelectRunAction()
    {
        BattleEntity currentPlayerEntity = playerBattlers[currentPlayer];
        currentPlayerEntity.State = BattleEntity.BattleState.Running;
        battleMenu.SetActive(false);
        currentPlayer++;

        if (currentPlayer >= playerBattlers.Count)
        {
            StartCoroutine(BattleRoutine());
        }
        else
        {
            ShowBattleMenu();
        }
    }

    void SetEnemySelectionButtons()
    {
        foreach (GameObject button in enemySelectionButtons)
        {
            button.SetActive(false);
        }

        for (int i = 0; i < enemyBattlers.Count; i++)
        {
            enemySelectionButtons[i].SetActive(true);
            enemySelectionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = enemyBattlers[i].Name;
        }
    }

    void AttackAction(BattleEntity player, BattleEntity target)
    {
        player.BattleVisual.PlayAttackAnimation();
        target.CurrentHealth -= player.Strength;
        target.BattleVisual.PlayHitAnimation();
        target.UpdateUI();
        bottomText.text = string.Format("{0} dealt {1} damage to {2}.", player.Name, player.Strength, target.Name);
        SaveHealth();
    }

    IEnumerator AttackRoutine(int i)
    {
        if (allBattlers[i].IsPlayer)
        {
            BattleEntity player = allBattlers[i];
            if (!allBattlers[player.Target].IsPlayer || player.Target >= allBattlers.Count)
            {
                player.Target = GetRandomEnemy();
            }
            BattleEntity target = allBattlers[player.Target];

            AttackAction(allBattlers[i], allBattlers[allBattlers[i].Target]);
            yield return new WaitForSeconds(TURN_DURATION);

            if (target.CurrentHealth <= 0)
            {
                bottomText.text = string.Format("{0} has been defeated!", target.Name);
                yield return new WaitForSeconds(TURN_DURATION);

                allBattlers.Remove(target);
                enemyBattlers.Remove(target);

                if (enemyBattlers.Count <= 0)
                {
                    currentState = BattleState.Won;
                    bottomText.text = "You won the battle!";
                    SceneManager.LoadScene(OVERWORLD_SCENE_NAME);
                }
            }
        }

        if (i < allBattlers.Count && !allBattlers[i].IsPlayer)
        {

            BattleEntity enemy = allBattlers[i];
            enemy.Target = GetRandomPartyMember();
            BattleEntity target = allBattlers[enemy.Target];

            AttackAction(allBattlers[i], allBattlers[allBattlers[i].Target]);
            yield return new WaitForSeconds(TURN_DURATION);

            if (target.CurrentHealth <= 0)
            {
                bottomText.text = string.Format("{0} has been defeated!", target.Name);
                yield return new WaitForSeconds(TURN_DURATION);
                allBattlers.Remove(target);
                playerBattlers.Remove(target);

                if (playerBattlers.Count <= 0)
                {
                    currentState = BattleState.Lost;
                    bottomText.text = "You lost the battle!";
                    SceneManager.LoadScene(OVERWORLD_SCENE_NAME);
                }
            }
        }

    }

    IEnumerator RunRoutine()
    {
        if (currentState == BattleState.Battle)
        {
            if (UnityEngine.Random.Range(1, 101) >= 50)
            {
                currentState = BattleState.Run;
                allBattlers.Clear();
                bottomText.text = "You ran away!";
                yield return new WaitForSeconds(TURN_DURATION);
                SceneManager.LoadScene(OVERWORLD_SCENE_NAME);
            }
            else
            {
                bottomText.text = "You couldn't run away!";
                yield return new WaitForSeconds(TURN_DURATION);
            }
        }
    }

    int GetRandomPartyMember()
    {
        List<int> availableMembers = new();
        for (int i = 0; i < allBattlers.Count; i++)
        {
            if (allBattlers[i].IsPlayer)
            {
                availableMembers.Add(i);
            }
        }

        return availableMembers[UnityEngine.Random.Range(0, availableMembers.Count)];
    }

    int GetRandomEnemy()
    {
        List<int> availableEnemies = new();
        for (int i = 0; i < allBattlers.Count; i++)
        {
            if (!allBattlers[i].IsPlayer)
            {
                availableEnemies.Add(i);
            }
        }

        return availableEnemies[UnityEngine.Random.Range(0, availableEnemies.Count)];
    }

    void SaveHealth()
    {
        for (int i = 0; i < playerBattlers.Count; i++)
        {
            partyManager.SaveHealth(i, playerBattlers[i].CurrentHealth);
        }
    }

    void DetermineBattleOrder()
    {
        allBattlers.Sort((a, b) => b.Speed.CompareTo(a.Speed));
    }
}

[Serializable]
public class BattleEntity
{
    public enum BattleState
    {
        Attacking,
        Running,
    }
    public BattleState State;

    public string Name;
    public int CurrentHealth;
    public int MaxHealth;
    public int Speed;
    public int Strength;
    public int Level;
    public bool IsPlayer;
    public BattleVisuals BattleVisual;
    public int Target;

    public void UpdateUI()
    {
        BattleVisual.UpdateHealthBar(CurrentHealth);
    }
}
