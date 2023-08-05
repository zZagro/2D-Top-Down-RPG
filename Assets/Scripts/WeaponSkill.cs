using UnityEngine;

[CreateAssetMenu]

public class WeaponSkill : ScriptableObject
{
    public string SkillName;
    public string SkillDescription;
    public float Cooldown;
    public Sprite Icon;
    public KeyCode Key;
}
