using UnityEngine;

public class Equipment : ScriptableObject
{
    public string EquipmentName;
    public Sprite EquipmentSprite;

    public Rarity Rarity;
    public int Level;
    public int CurrentXp;

    public int HealthBonus;
    public float HealthBonusPercent;
    public int DefenseBonus;
    public float DefenseBonusPercent;
    public int AttackBonus;
    public float AttackBonusPercent;
    public float AttackSpeedBonus;
    public float CritChanceBonus;
    public float CritDamageBonus;
    public int SpeedBonus;
    public float SpeedBonusPercent;
}
