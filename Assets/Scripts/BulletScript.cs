using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    void Update()
    {
        if (!GetComponent<SpriteRenderer>().isVisible)
        {
            DestroyBullet();
        }
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyBullet();
    }
}
