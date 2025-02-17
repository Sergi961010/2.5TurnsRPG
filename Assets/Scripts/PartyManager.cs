using System;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] PartyMemberInfo[] partyMembersData;
    [SerializeField] List<PartyMember> currentParty;
    [SerializeField] PartyMemberInfo defaultPartyMember;
    Vector3 playerPosition = Vector3.up;
    static PartyManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            AddMemberToPartyByName(defaultPartyMember.Name);
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void AddMemberToPartyByName(string name)
    {
        foreach (var data in partyMembersData)
        {
            if (data.Name == name)
            {
                PartyMember newMember = new()
                {
                    Name = data.Name,
                    Level = data.StartingLevel,
                    CurrentHealth = data.BaseHealth,
                    MaxHealth = data.BaseHealth,
                    Strength = data.BaseStrength,
                    Speed = data.BaseSpeed,
                    CurrentExperience = 0,
                    MaxExperience = 0,
                    BattleVisualPrefab = data.BattleVisualPrefab,
                    OverworldVisualPrefab = data.OverworldVisualPrefab
                };
                currentParty.Add(newMember);
            }
        }
    }

    public List<PartyMember> GetAliveParty()
    {
        List<PartyMember> activeMembers = new();
        foreach (PartyMember member in currentParty)
        {
            if (member.CurrentHealth > 0)
            {
                activeMembers.Add(member);
            }
        }
        return activeMembers;
    }

    public List<PartyMember> GetCurrentParty() => currentParty;

    public void SaveHealth(int index, int health)
    {
        currentParty[index].CurrentHealth = health;
    }

    public void SetPosition(Vector3 position)
    {
        playerPosition = position;
    }

    public Vector3 GetPosition()
    {
        return playerPosition;
    }
}

[Serializable]
public class PartyMember : IBattler
{
    public string Name;
    public int Level { get; set; }
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
    public int Strength;
    public int Speed;
    public int CurrentExperience;
    public int MaxExperience;
    public GameObject BattleVisualPrefab { get; set; }
    public GameObject OverworldVisualPrefab;
}
