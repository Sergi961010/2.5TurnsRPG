using System;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] PartyMemberInfo[] partyMembers;
    [SerializeField] List<PartyMember> currentParty;
    [SerializeField] PartyMemberInfo defaultPartyMember;

    void Awake()
    {
        AddMemberToPartyByName(defaultPartyMember.Name);
    }

    public void AddMemberToPartyByName(string name)
    {
        foreach (var member in partyMembers)
        {
            if (member.Name == name)
            {
                PartyMember newMember = new()
                {
                    Name = member.Name,
                    Level = member.StartingLevel,
                    CurrentHealth = member.BaseHealth,
                    MaxHealth = member.BaseHealth,
                    Strength = member.BaseStrength,
                    Speed = member.BaseSpeed,
                    CurrentExperience = 0,
                    MaxExperience = 0,
                    BattleVisualPrefab = member.BattleVisualPrefab,
                    OverworldVisualPrefab = member.OverworldVisualPrefab
                };
                currentParty.Add(newMember);
            }
        }
    }
}

[Serializable]
public class PartyMember
{
    public string Name;
    public int Level;
    public int CurrentHealth;
    public int MaxHealth;
    public int Strength;
    public int Speed;
    public int CurrentExperience;
    public int MaxExperience;
    public GameObject BattleVisualPrefab;
    public GameObject OverworldVisualPrefab;
}
