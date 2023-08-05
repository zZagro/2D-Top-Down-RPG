using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInformation : MonoBehaviour
{

    public List<GameObject> SpawnedEnemies = new List<GameObject>();
    public List<GameObject> OverworldEnemies = new List<GameObject>();
    public List<GameObject> HiddenEnemies = new List<GameObject>();

    void Start()
    {
        
    }

    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            AddEnemyOne();
        }*/
        if (Input.GetMouseButtonDown(1))
        {
            AddEnemyTwo();
        }
    }

    private void AddEnemyOne()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var enemyInstance = Instantiate(OverworldEnemies[0], mousePos, Quaternion.identity, transform);
        SpawnedEnemies.Add(enemyInstance);
    }

    private void AddEnemyTwo()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var enemyInstance = Instantiate(OverworldEnemies[1], mousePos, Quaternion.identity, transform);
        SpawnedEnemies.Add(enemyInstance);
    }
}
