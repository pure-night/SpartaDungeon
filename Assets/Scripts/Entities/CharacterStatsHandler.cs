using System;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStatsHandler : MonoBehaviour
{
    [SerializeField] private CharacterStats baseStats;
    public GameObject statusUI;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI atkText;
    public TextMeshProUGUI spdText;
    
    public CharacterStats CurrentStats { get; set; }
    public List<CharacterStats> statsModifiers = new List<CharacterStats>();

    private void Awake()
    {
        UpdateCharacterStats();
    }

    private void Start()
    {
        statusUI.SetActive(false);
    }

    private void UpdateCharacterStats()
    {
        StatSo statSo = null;
        if (baseStats.statSo != null)
        {
            statSo = Instantiate(baseStats.statSo);
        }

        CurrentStats = new CharacterStats { statSo = statSo };
        UpdateStats((a, b) => b, baseStats);

        foreach (var modifier in statsModifiers.OrderBy(o => o.statsChangeType))
        {
            switch (modifier.statsChangeType)
            {
                case StatsChangeType.Override:
                    UpdateStats((o, o1) => o1, modifier);
                    break;
                case StatsChangeType.Add:
                    UpdateStats((o, o1) => o + o1, modifier);
                    break;
            }
        }
    }
    
    public void AddStatModifier(CharacterStats statModifier)
    {
        statsModifiers.Add(statModifier);
        UpdateCharacterStats();
    }
    
    public void RemoveStatModifier(CharacterStats statModifier)
    {
        statsModifiers.Remove(statModifier);
        UpdateCharacterStats();
    }
    
    private void UpdateStats(Func<float, float, float> operation, CharacterStats newModifier)
    {
        CurrentStats.maxHealth = (int)operation(CurrentStats.maxHealth, newModifier.maxHealth);
        CurrentStats.power = operation(CurrentStats.power, newModifier.power);
        CurrentStats.defense = operation(CurrentStats.defense, newModifier.defense);
        CurrentStats.attackSpeed = operation(CurrentStats.attackSpeed, newModifier.attackSpeed);
        CurrentStats.reloadSpeed = operation(CurrentStats.reloadSpeed, newModifier.reloadSpeed);
        CurrentStats.moveSpeed = operation(CurrentStats.moveSpeed, newModifier.moveSpeed);
        CurrentStats.numberOfProjectilesPerShot = (int)operation(CurrentStats.numberOfProjectilesPerShot, newModifier.numberOfProjectilesPerShot);
    }

    public void OnStatusButton()
    {
        if (!statusUI.activeInHierarchy)
        {
            UpdateStatusUI();
            statusUI.SetActive(true);
        }
        else
        {
            statusUI.SetActive(false);
        }
    }

    public void UpdateStatusUI()
    {
        healthText.text = CurrentStats.maxHealth.ToString();
        atkText.text = CurrentStats.power.ToString();
        defText.text = CurrentStats.defense.ToString();
        spdText.text = CurrentStats.moveSpeed.ToString();
    }
}
