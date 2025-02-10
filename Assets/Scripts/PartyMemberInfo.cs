using UnityEngine;

[CreateAssetMenu(fileName = "PartyMemberInfo", menuName = "Scriptable Objects/PartyMemberInfo")]
public class PartyMemberInfo : ScriptableObject
{
    public string Name;
    public int StartingLevel;
    public int BaseHealth;
    public int BaseStrength;
    public int BaseSpeed;
    public GameObject BattleVisualPrefab;
    public GameObject OverworldVisualPrefab;
}
