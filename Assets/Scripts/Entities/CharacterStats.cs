using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum StatsChangeType
{
    Add,
    Override,
}

[Serializable]
public class CharacterStats
{
    public StatsChangeType statsChangeType;
    [Range(-100, 100)] public int maxHealth;
    [Range(-10f, 10f)] public float power;
    [Range(-10f, 10f)] public float defense;
    [Range(-10f, 10f)] public float attackSpeed;
    [Range(-10f, 10f)] public float reloadSpeed;
    [Range(-10f, 10f)] public float moveSpeed;
    [Range(0, 5f)] public int numberOfProjectilesPerShot;

    public StatSo statSo;
}
