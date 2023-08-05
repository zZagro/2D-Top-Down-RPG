using System;
using UnityEngine;

[CreateAssetMenu]

public class Weapon : ScriptableObject
{
    [Header("GENERAL INFO")]
    public string WeaponName;
    public WeaponType WeaponType;
    public int BaseAttack;

    [Header("WAND")]
    public float radius;

    [Header("SUBSTAT")]
    public bool HasSubstat;
    public Stats Substat;
    public StatsType SubstatType;
    public float SubstatValue;

    [Header("ITEM INFORMATION")]
    public Rarity Rarity;
    public int ItemQuality;

    [Header("ATTACK TIME")]
    public float Cooldown;
    public float ChargeTime;

    [Header("VISUALS")]
    public Sprite Sprite;
    public AnimationClip AnimationClip;

    [Header("LEVELING")]
    public int Level;
    public int CurrentXp;

    [Header("EXTRAS")]
    public bool HasWeaponSkill;
    public WeaponSkill Skill;
}
