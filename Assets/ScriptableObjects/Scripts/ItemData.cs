using System;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Activated,
    Passive,
    Trinket,
    Consumable,
}

public enum ValueType
{
    Health,
    Power,
    Defense,
    AttackSpeed,
    ReloadSpeed,
    MoveSpeed,
}

[Serializable]
public class AdditionalItemStat
{
    public ValueType type;
    public float value;
}

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemKey;
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    public AdditionalItemStat[] additionalItemStats;

    [Header("Equip")]
    public GameObject equipPrefab;

    public CharacterStats statMultiplier;
}
