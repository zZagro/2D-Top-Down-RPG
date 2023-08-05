using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string EnemyName;
    public Sprite enemySprite;
    public Sprite enemyUISprite;

    [Header("ENEMY STATS")]
    public float Health;

    [Header("ENEMY DROPS")]
    public Dictionary<Equipment, int> EquipmentDrops = new Dictionary<Equipment, int>();
    public Dictionary<Weapon, int> WeaponDrops = new Dictionary<Weapon, int>();

    public void DamagePlayer(PlayableCharacter character, float damage)
    {
        if(character.Health > damage)
        {
            character.Health -= damage;
        }
        else
        {
            character.Health = 0f;
        }
        character.HasTakenDamage = true;
    }

    public void DropEquipment()
    {
        for (int i = 0; i < EquipmentDrops.Count; i++)
        {
            var key = EquipmentDrops.ElementAt(i).Key;
            var value = EquipmentDrops.ElementAt(i).Value;
            int dropPosibility = Random.Range(1, 101);
            if (value <= dropPosibility)
            {
                //drop equipment
            }
        }

        for (int i = 0; i < WeaponDrops.Count; i++)
        {
            var key = WeaponDrops.ElementAt(i).Key;
            var value = WeaponDrops.ElementAt(i).Value;
            int dropPosibility = Random.Range(1, 101);
            if (value <= dropPosibility)
            {
                //drop equipment
            }
        }
    }
}
