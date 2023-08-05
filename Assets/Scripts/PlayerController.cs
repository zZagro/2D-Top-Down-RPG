using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public float runningSpeed = 7;

    private bool canDash = true;
    private float dashSpeed = 5;
    private float dashingCooldown = 1f;
    private Coroutine dashRoutine;

    [SerializeField] private Transform partyUI;
    [SerializeField] private PlayableCharacter character;
    [SerializeField] private GameObject enemy;
    [SerializeField] public GameObject weaponObject;
    [SerializeField] public GameObject playerObject;

    public PlayableCharacter _currentCharacter;
    private int _currentCharacterIndex = 0;
    private CharacterInformation characterInformation;
    private List<PlayableCharacter> characters;

    private EnemyInformation enemyInformation;
    private List<GameObject> spawnedEnemies;

    private bool isInCombat;
    private Coroutine combatStatusRoutine;

    private void Start()
    {
        characterInformation = GetComponent<CharacterInformation>();
        characterInformation.SetOwnedCharacters();
        characterInformation.SetPartyCharacters();
        characters = characterInformation.PartyCharacters;

        enemyInformation = enemy.GetComponent<EnemyInformation>();
        spawnedEnemies = enemyInformation.SpawnedEnemies;

        foreach(PlayableCharacter character in characterInformation.OwnedCharacters)
        {
            character.SetStatsToMax(false);
        }

        _currentCharacter = characters[0];

        ManagePartyUI();
    }

    private void Update()
    {
        ManageMovement();

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchCharacter(0);
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchCharacter(1);
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchCharacter(2);
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GetXP();
        }

        //DrawWandRadius(500, 5);
        if (Input.GetMouseButtonDown(0))
        {
            //DamageEnemyMelee(5);
            //DamageEnemyWand(5, 5);
            //DamageEnemyGun(5);
            //DamageEnemyBow(damage);
        }

        SetCombatStatus();
    }

    public void SwitchCharacter(int index)
    {
        ManagePartyUI();
        if (index >= characters.Count)
        {
            return;
        }
        _currentCharacterIndex = index;
        _currentCharacter = characters[_currentCharacterIndex];
        playerObject.GetComponent<SpriteRenderer>().sprite = _currentCharacter.CharacterSprite;
        characters = characterInformation.PartyCharacters;
    }

    private void ManagePartyUI()
    {
        characterInformation.AddPartyCharacters();
        for (int i = 0; i < characters.Count; i++)
        {
            var child = partyUI.GetChild(i);
            int count = child.childCount;
            for (int k = 0; k < count; k++)
            {
                var text = child.GetChild(k).GetChild(0).GetComponent<TextMeshProUGUI>();
                text.text = characters[k].CharacterName;

                var img = child.GetChild(k).GetChild(1).GetComponent<Image>();
                img.sprite = characters[k].UISprite;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            partyUI.GetChild(i).gameObject.SetActive(false);
        }
        switch (characters.Count)
        {
            case 1:
                partyUI.GetChild(0).gameObject.SetActive(true);
                break;
            case 2:
                partyUI.GetChild(1).gameObject.SetActive(true);
                break;
            case 3:
                partyUI.GetChild(2).gameObject.SetActive(true);
                break;
            default:
                partyUI.GetChild(0).gameObject.SetActive(true);
                break;
        }
    }

    private void GetXP()
    {
        _currentCharacter.GainXp(10000);
    }

    private void SetCombatStatus()
    {
        if (_currentCharacter.HasTakenDamage)
        {
            if (!isInCombat) isInCombat = true;
            if (combatStatusRoutine == null)
            {
                combatStatusRoutine = StartCoroutine(TakenDamageCooldown());
            }
            else
            {
                StopCoroutine(TakenDamageCooldown());
                combatStatusRoutine = StartCoroutine(TakenDamageCooldown());
            }
        }
    }

    private IEnumerator TakenDamageCooldown()
    {
        yield return new WaitForSeconds(4);
        isInCombat = false;
        combatStatusRoutine = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void ManageMovement()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (canDash) dashRoutine = StartCoroutine(Dash());
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Run();
        }
    }

    private void Move()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 newLocation = new Vector3(horizontal, vertical);
        transform.position += newLocation * speed * (_currentCharacter.BaseSpeed/100) * Time.deltaTime;
    }

    private void Run()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 newLocation = new Vector3(horizontal, vertical);
        transform.position += newLocation * runningSpeed * (_currentCharacter.BaseSpeed / 100) * Time.deltaTime;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 newLocation = new Vector3(horizontal, vertical).normalized * dashSpeed;
        transform.position += newLocation;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        dashRoutine = null;
    }
}