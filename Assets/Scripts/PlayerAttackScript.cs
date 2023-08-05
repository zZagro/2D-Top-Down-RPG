using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackScript : MonoBehaviour
{

    [SerializeField] private LineRenderer circleRenderer;
    [SerializeField] private Transform projectileTransform;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject arrowPrefab;

    [Range(0, 10)]
    [SerializeField] float bowPower;

    private float maxMouseDistance = 1.5f;
    private float addArrowSpeed = 2f;
    private float bowCharge;
    private bool canFire = true;

    Weapon weapon;
    private float damage = 5;
    private float wandRadius = 5;

    PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        /*if (Input.GetMouseButton(0) && canFire)
        {
            ChargeBow();
        } 
        else if (Input.GetMouseButtonUp(0) && canFire)
        {
            FireBow();
        }
        else
        {
            if (bowCharge > 0)
            {
                bowCharge -= 1f * Time.deltaTime;
            }
            else
            {
                bowCharge = 0;
                canFire = true;
            }

            bowPowerSlider.value = bowCharge;
        }*/

        if (playerController._currentCharacter.GetWeapon().WeaponType == WeaponType.BOW)
        {
            SetWeaponPositionBow();
        }
        else
        {
            SetWeaponPosition();
        }

        Attack();
    }

    private void Attack()
    {
        weapon = playerController._currentCharacter.GetWeapon();
        if (weapon.WeaponType == WeaponType.WAND)
        {
            DrawWandRadius(250, wandRadius);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (weapon.WeaponType == WeaponType.GAUNTLET || weapon.WeaponType == WeaponType.SPEAR || weapon.WeaponType == WeaponType.SWORD)
            {
                DamageEnemyMelee(damage);
            }
            else if (weapon.WeaponType == WeaponType.GUN)
            {
                DamageEnemyGun(damage);
            }
            else if (weapon.WeaponType == WeaponType.WAND)
            {
                DamageEnemyWand(damage, wandRadius);
            }
        }

        //bow
        if (Input.GetMouseButton(0))
        {
            if (weapon.WeaponType == WeaponType.BOW)
            {
                ChargeBow();
            }
        }
        else if (Input.GetMouseButtonUp(0) && canFire)
        {
            if (weapon.WeaponType == WeaponType.BOW)  FireBow();
        }
        else
        {
            if (weapon.WeaponType == WeaponType.BOW)
            {
                if (bowCharge > 0)
                {
                    bowCharge -= 1f * Time.deltaTime;
                }
                else
                {
                    bowCharge = 0;
                    canFire = true;
                }

            }  
        }
    }

    private void SetWeaponPosition()
    {
        float radius = 1f;
        float offset = .25f;
        Vector3 weaponPosition;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;

        mousePos = new Vector2(mousePos.x - playerPos.x, mousePos.y - playerPos.y);
        weaponPosition = mousePos.normalized * (radius + offset);
        playerController.weaponObject.transform.localPosition = weaponPosition;

        var angle = Mathf.Atan2(weaponPosition.x, weaponPosition.y);
        playerController.weaponObject.transform.localEulerAngles = new Vector3(0, 0, -angle * Mathf.Rad2Deg);
    }

    private void SetWeaponPositionBow()
    {
        float radius = 1f;
        float offset = .25f;
        Vector3 weaponPosition;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;

        mousePos = new Vector2(mousePos.x - playerPos.x, mousePos.y - playerPos.y);
        weaponPosition = mousePos.normalized * (radius + offset);
        playerController.weaponObject.transform.localPosition = -weaponPosition;

        var angle = Mathf.Atan2(weaponPosition.x, weaponPosition.y);
        playerController.weaponObject.transform.localEulerAngles = new Vector3(0, 0, -angle * Mathf.Rad2Deg);
    }

    private void DamageEnemyMelee(float damage)
    {
        float weaponSize = playerController.weaponObject.transform.localScale.y;
        float radius = weaponSize * 2;

        Vector2 weaponLocation = playerController.weaponObject.transform.position;

        Collider2D[] overlapCircle = Physics2D.OverlapCircleAll(weaponLocation, radius);

        for (int i = 0; i < overlapCircle.Length; i++)
        {
            if (overlapCircle[i].gameObject.GetComponent<Enemy>() != null)
            {
                Enemy enemy = overlapCircle[i].GetComponent<Enemy>();
                enemy.Health -= damage;
                Debug.Log(enemy.EnemyName + " has " + enemy.Health + " health!");
                if (enemy.Health <= 0) Destroy(enemy.gameObject);
            }
        }
    }

    private void DamageEnemyWand(float damage, float radius)
    {
        Vector2 playerPos = playerController.playerObject.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] collide = Physics2D.OverlapPointAll(mousePos);

        if (Math.Abs(playerPos.x + radius) <= mousePos.x || Math.Abs(playerPos.y + radius) <= mousePos.y) return;
        if (collide.Length == 0) return;

        foreach (Collider2D collider in collide)
        {
            if (collider.gameObject.GetComponent<Enemy>() != null)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                enemy.Health -= damage;
                Debug.Log(enemy.EnemyName + " has " + enemy.Health + " health!");
                if (enemy.Health <= 0) Destroy(enemy.gameObject);
            }

        }
    }
    private void DrawWandRadius(int steps, float radius)
    {
        if (circleRenderer.gameObject.activeSelf == false) circleRenderer.gameObject.SetActive(true);
        Vector2 playerPos = playerController.playerObject.transform.position;
        circleRenderer.positionCount = steps;
        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / steps;

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(playerPos.x + x, playerPos.y + y, 0);

            circleRenderer.SetPosition(currentStep, currentPosition);
        }
    }

    private void DamageEnemyGun(float damage)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = playerController.playerObject.transform.position;
        Vector2 weaponPos = playerController.weaponObject.transform.position;

        RaycastHit2D raycast = Physics2D.Raycast(weaponPos, mousePos);

        var bulletSpawned = Instantiate(bullet, weaponPos, Quaternion.identity, projectileTransform);
        bulletSpawned.GetComponent<Rigidbody2D>().AddForce((mousePos - playerPos).normalized * 5, ForceMode2D.Impulse);

        if (raycast.collider == null) return;
        if (!HasRayHitEnemy(raycast)) return;

        Enemy enemy = raycast.collider.gameObject.GetComponent<Enemy>();
        enemy.Health -= damage;
        Debug.Log(enemy.EnemyName + " has " + enemy.Health + " health!");
        if (enemy.Health <= 0) Destroy(enemy.gameObject);
    }

    private void ChargeBow()
    {
        var mouseDistance = DistanceToMouse();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float percentToMaxDistance = ((Math.Abs(mouseDistance.y) + Math.Abs(mouseDistance.x)) / 2) / maxMouseDistance;

        bowCharge = percentToMaxDistance;

        if (bowCharge > maxMouseDistance)
        {
            bowCharge = maxMouseDistance;
        }

        DottedLine.Instance.Delta = (percentToMaxDistance) > 1 ? 2 : (percentToMaxDistance * 2) > 0.1f ? (percentToMaxDistance * 2) : 0.1f;
        DottedLine.Instance.DrawDottedLine(playerController.weaponObject.transform.position, mousePos * 5);
    }

    private void FireBow()
    {
        if (bowCharge > maxMouseDistance) bowCharge = maxMouseDistance;

        float arrowSpeed = bowCharge * bowPower * addArrowSpeed;

        float angle = AngleTowardsMouse();
        Quaternion rot = Quaternion.Euler(new Vector3(0f, 0f, angle - 90));

        var arrowObject = Instantiate(arrowPrefab, playerController.weaponObject.transform.position, rot, projectileTransform);
        Arrow arrow = arrowObject.GetComponent<Arrow>();
        arrow.ArrowVelocity = -arrowSpeed;

        canFire = false;
    }

    private float AngleTowardsMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;

        mousePos = new Vector2(mousePos.x - playerPos.x, mousePos.y - playerPos.y);

        var angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        return angle;
    }

    private Vector2 DistanceToMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        return new Vector2(mousePos.x - playerPos.x, mousePos.y - playerPos.y);
    }

    private bool HasRayHitEnemy(RaycastHit2D hit)
    {
        if (hit.collider.gameObject.GetComponent<Enemy>() == true)
        {
            return true;
        }
        return false;
    }
}
