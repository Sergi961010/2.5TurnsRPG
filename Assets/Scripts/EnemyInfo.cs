using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyInfo", menuName = "Scriptable Objects/EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    public string Name;
    public int BaseHealth;
    public int BaseStrength;
    public int BaseSpeed;
    public GameObject BattleVisualPrefab;
}
