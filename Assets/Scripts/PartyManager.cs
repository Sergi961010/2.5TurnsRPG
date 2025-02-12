using System;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] PartyMemberInfo[] partyMembersData;
    [SerializeField] List<PartyMember> currentParty;
    [SerializeField] PartyMemberInfo defaultPartyMember;

    void Awake()
    {
        AddMemberToPartyByName(defaultPartyMember.Name);
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

    public List<PartyMember> GetCurrentPartyMembers()
    {
        return currentParty;
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
