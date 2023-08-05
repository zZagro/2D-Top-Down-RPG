using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class PlayableCharacter : ScriptableObject
{
    [Header("CHARACTER INFORMATION")]
    public string CharacterName;
    public Sprite CharacterSprite;
    public Sprite UISprite;
    public bool IsInParty;
    public bool IsOwned;
    public Rarity Rarity;

    [HideInInspector]
    public bool HasTakenDamage;

    [Header("EQUIPMENT")]
    public Weapon EquipedWeapon;
    public List<WeaponType> UsableWeaponTypes = new List<WeaponType>();
    public List<Equipment> CurrentEquipment = new List<Equipment>();

    [Header("LEVELING")]
    public int Level;
    public float CurrentXp;
    public float NextLevelXp;

    [Header("BASE STATS")]
    public float BaseHealth;
    public float BaseDefense;
    public float BaseSpeed;
    public float BaseAttack;
    public float BaseAttackSpeed;
    [Range(0f, 100f)]
    public float BaseCritChance;
    public float BaseCritDamage;

    [Header("CURRENT STATS")]
    public float Health;
    public float Defense;
    public float Speed;
    public float Attack;
    public float AttackSpeed;
    [Range(0f, 100f)]
    public float CritChance;
    public float CritDamage;

    private float bonusHealth;
    private float bonusDefense;
    private float bonusSpeed;
    private float bonusAttack;

    public void GainXp(int gainedXp)
    {
        CurrentXp += gainedXp;
        while (CurrentXp >= NextLevelXp)
        {
            Level++;
            CurrentXp -= NextLevelXp;
            NextLevelXp = Mathf.Round(NextLevelXp * 1.1f);
        }
    }

    public void RecoverHealth(float amount)
    {
        if (Health < BaseHealth)
        {
            Health += Mathf.Round(amount);
        }
        else
        {
            Health = BaseHealth;
        }
    }

    public IEnumerator RecoverHealthAuto(int waitTime)
    {
        while (Health < BaseHealth)
        {
            Health += BaseHealth * 0.05f;
            yield return new WaitForSeconds(waitTime);
        } 
        if (Health >= BaseHealth)
        {
            Health = BaseHealth;
        }
        yield break;
    }

    public Weapon GetWeapon()
    {
        return EquipedWeapon;
    }

    public void SetWeapon(Weapon newWeapon, Weapon oldWeapon, Weapon standardWeapon)
    {
        var health = BaseHealth;
        if (oldWeapon != null)
        {
            RemoveStatsFlat(0, 0, 0, oldWeapon.BaseAttack + newWeapon.ItemQuality / 10, 0, 0, 0);
            RemoveSubStat(oldWeapon);
        }
        if (EquipedWeapon == null)
        {
            EquipedWeapon = standardWeapon;
            AddStatsFlat(0, 0, 0, standardWeapon.BaseAttack, 0, 0, 0);
            AddSubStat(standardWeapon);
        } else
        {
            if (UsableWeaponTypes.Contains(newWeapon.WeaponType))
            {
                EquipedWeapon = newWeapon;
                AddStatsFlat(0, 0, 0, newWeapon.BaseAttack + newWeapon.ItemQuality / 10, 0, 0, 0);
                AddSubStat(newWeapon);
            }
        }
        Health += BaseHealth - health;
    }

    public void AddEquipment(Equipment equip)
    {
        var health = BaseHealth;

        CurrentEquipment.Add(equip);

        AddStatsFlat(equip.HealthBonus, equip.DefenseBonus, equip.SpeedBonus, equip.AttackBonus, equip.AttackSpeedBonus, equip.CritChanceBonus, equip.CritDamageBonus);

        AddStatsPercent(equip.HealthBonusPercent, equip.DefenseBonusPercent, equip.SpeedBonusPercent, equip.AttackBonusPercent);

        RemoveSubStat(EquipedWeapon);
        AddSubStat(EquipedWeapon);

        Health += BaseHealth - health;
    }

    public void RemoveEquipment(Equipment equip)
    {
        if (CurrentEquipment.Contains(equip))
        {
            var health = BaseHealth;

            CurrentEquipment.Remove(equip);

            RemoveStatsFlat(equip.HealthBonus, equip.DefenseBonus, equip.SpeedBonus, equip.AttackBonus, equip.AttackSpeedBonus, equip.CritChanceBonus, equip.CritDamageBonus);

            RemoveStatsPercent(equip.HealthBonusPercent, equip.DefenseBonusPercent, equip.SpeedBonusPercent, equip.AttackBonusPercent);

            RemoveSubStat(EquipedWeapon);
            AddSubStat(EquipedWeapon);

            Health += BaseHealth - health;
        }
    }

    public void AddSubStat(Weapon weapon)
    {
        if (!weapon.HasSubstat) return;
        var health = BaseHealth;
        if (weapon.SubstatType == StatsType.FLAT)
        {
            switch (weapon.Substat)
            {
                case Stats.HEALTH:
                    BaseHealth += weapon.SubstatValue;
                    break;
                case Stats.DEFENSE:
                    BaseDefense += weapon.SubstatValue;
                    break;
                case Stats.SPEED:
                    BaseSpeed += weapon.SubstatValue;
                    break;
                case Stats.ATTACK:
                    BaseAttackSpeed += weapon.SubstatValue;
                    break;
                case Stats.ATTACKSPEED:
                    BaseAttackSpeed += weapon.SubstatValue;
                    break;
                case Stats.CRITCHANCE:
                    BaseCritChance += weapon.SubstatValue;
                    break;
                case Stats.CRITDAMAGE:
                    BaseCritDamage += weapon.SubstatValue;
                    break;
                default:
                    break;
            }
        } else if (weapon.SubstatType == StatsType.PERCENT)
        {
            RemoveStatsBonus();
            switch (weapon.Substat)
            {
                case Stats.HEALTH:
                    bonusHealth += BaseHealth * (1 + weapon.SubstatValue);
                    break;
                case Stats.DEFENSE:
                    bonusDefense += BaseDefense * (1 + weapon.SubstatValue);
                    break;
                case Stats.SPEED:
                    bonusSpeed += BaseSpeed * (1 + weapon.SubstatValue);
                    break;
                case Stats.ATTACK:
                    bonusAttack += BaseAttackSpeed * (1 + weapon.SubstatValue);
                    break;
                default:
                    break;
            }
            AddStatsBonus();
        }
        Health += BaseHealth - health;
    }

    public void RemoveSubStat(Weapon weapon)
    {
        if (!weapon.HasSubstat) return;
        var health = BaseHealth;
        if (weapon.SubstatType == StatsType.FLAT)
        {
            switch (weapon.Substat)
            {
                case Stats.HEALTH:
                    BaseHealth -= weapon.SubstatValue;
                    break;
                case Stats.DEFENSE:
                    BaseDefense -= weapon.SubstatValue;
                    break;
                case Stats.SPEED:
                    BaseSpeed -= weapon.SubstatValue;
                    break;
                case Stats.ATTACK:
                    BaseAttackSpeed -= weapon.SubstatValue;
                    break;
                case Stats.ATTACKSPEED:
                    BaseAttackSpeed -= weapon.SubstatValue;
                    break;
                case Stats.CRITCHANCE:
                    BaseCritChance -= weapon.SubstatValue;
                    break;
                case Stats.CRITDAMAGE:
                    BaseCritDamage -= weapon.SubstatValue;
                    break;
                default:
                    break;
            }
        }
        else if (weapon.SubstatType == StatsType.PERCENT)
        {
            RemoveStatsBonus();
            switch (weapon.Substat)
            {
                case Stats.HEALTH:
                    bonusHealth -= BaseHealth / (1 + weapon.SubstatValue);
                    break;
                case Stats.DEFENSE:
                     bonusDefense -= BaseDefense / (1 + weapon.SubstatValue);
                    break;
                case Stats.SPEED:
                    bonusSpeed -= BaseSpeed / (1 + weapon.SubstatValue);
                    break;
                case Stats.ATTACK:
                    bonusAttack -= BaseAttack / (1 + weapon.SubstatValue);
                    break;
                default:
                    break;
            }
            AddStatsBonus();
        }
        Health += BaseHealth - health;
    }

    public void AddStatsFlat(float health, float defense, float speed, float attack, float attackSpeed, float critChance, float critDamage)
    {
        BaseHealth += health;
        BaseDefense += defense;
        BaseSpeed += speed;
        BaseAttack += attack;
        BaseAttackSpeed += attackSpeed;
        BaseCritChance += critChance;
        BaseCritDamage += critDamage;
    }

    public void AddStatsPercent(float health, float defense, float speed, float attack)
    {
        RemoveStatsBonus();
        bonusHealth += BaseHealth * (1 + health);
        bonusDefense += BaseDefense * (1 + defense);
        bonusSpeed += BaseSpeed * (1 + speed);
        bonusAttack += BaseAttack * (1 + attack);
        AddStatsBonus();
    }

    public void RemoveStatsFlat(float health, float defense, float speed, float attack, float attackSpeed, float critChance, float critDamage)
    {
        BaseHealth -= health;
        BaseDefense -= defense;
        BaseSpeed -= speed;
        BaseAttack -= attack;
        BaseAttackSpeed -= attackSpeed;
        BaseCritChance -= critChance;
        BaseCritDamage -= critDamage;
    }

    public void RemoveStatsPercent(float health, float defense, float speed, float attack)
    {
        RemoveStatsBonus();
        bonusHealth -= BaseHealth / (1 + health);
        bonusDefense -= BaseDefense / (1 + defense);
        bonusSpeed -= BaseSpeed / (1 + speed);
        bonusAttack -= BaseAttack / (1 + attack);
        AddStatsBonus();
    }

    private void AddStatsBonus()
    {
        BaseHealth += bonusHealth;
        BaseDefense += bonusDefense;
        BaseSpeed += bonusSpeed;
        BaseAttack += bonusAttack;
    }

    private void RemoveStatsBonus()
    {
        BaseHealth -= bonusHealth;
        BaseDefense -= bonusDefense;
        BaseSpeed -= bonusSpeed;
        BaseAttack -= bonusAttack;
    }

    public void SetStatsToMax(bool health)
    {
        if (health) Health = BaseHealth;
        Defense = BaseDefense;
        Speed = BaseSpeed;
        Attack = BaseAttack;
        AttackSpeed = BaseAttackSpeed;
        CritChance = BaseCritChance;
        CritDamage = BaseCritDamage;
    }
}
