using UnityEngine;

[CreateAssetMenu(fileName = "DefaultCharacterStat", menuName = "Character/Stat/default", order = 0)]
public class StatSo : ScriptableObject
{
    [Header("Info")]
    public string characterName;
    public int level;

    [Header("Stat")]
    public float health;
    public float power;
    public float attackSpeed;
    public float reloadSpeed;
    public float moveSpeed;

    [Header("Attack Data")]
    public float spread;
    public int numberOfProjectilesPerShot;
    public float multipleProjectilesAngle;
    public float duration;
}
