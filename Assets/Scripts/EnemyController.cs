using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private KeyCode damage;

    private Enemy enemy;
    private EnemyInformation enemyInformation;

    void Start()
    {
        enemyInformation = transform.parent.GetComponent<EnemyInformation>();

        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if (Input.GetKeyDown(damage))
        {
            //DamageEnemy();
        }
    }

    /*private void DamageEnemy()
    {
        enemy.Health -= 5;

        if (enemy.Health <= 0) Destroy(this.gameObject);

        Debug.Log(enemy.EnemyName + " has " + enemy.Health + " health!");
    }*/
}
