using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CharacterInventory : MonoBehaviour
{
    public KeyCode OpenCharInv = KeyCode.C;

    [SerializeField] private GameObject characterInv;
    [SerializeField] private Transform canvas;
    [SerializeField] private Transform container;
    [SerializeField] private GameObject obj;
    [SerializeField] private GameObject statsButton;
    [SerializeField] private GameObject statsScrollView;
    
    private CharacterInformation characterInformation;
    private bool isActive = false;
    private List<GameObject> characterSprites = new List<GameObject>();
    private float characterSpeed;
    private float characterRunSpeed;

    int index = 0;
    private List<PlayableCharacter> characters = new List<PlayableCharacter>();

    void Start()
    {
        characterInformation = GetComponent<CharacterInformation>();
        characterInv.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(OpenCharInv))
        {
            if (isActive)
            {
                CloseCharacterInventory();
            } else
            {
                OpenCharacterInventory();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isActive)
            {
                CloseCharacterInventory();
            }
        }
    }

    private void OpenCharacterInventory()
    {
        characterInv.SetActive(true);
        isActive = true;
        StopMovement();
        InstantiateCharacters();
    }

    private void CloseCharacterInventory()
    {
        foreach (GameObject gameObj in characterSprites)
        {
            gameObj.GetComponent<DestroyObject>().DestroyGameObject();
        }
        characterSprites.Clear();
        characterInv.SetActive(false);
        ContinueMovement();
        isActive = false;
    }

    private void StopMovement()
    {
        characterSpeed = GetComponent<PlayerController>().speed;
        GetComponent<PlayerController>().speed = 0f;

        characterRunSpeed = GetComponent<PlayerController>().runningSpeed;
        GetComponent<PlayerController>().runningSpeed = 0f;
    }

    private void ContinueMovement()
    {
        GetComponent<PlayerController>().speed = characterSpeed;
        GetComponent<PlayerController>().runningSpeed = characterRunSpeed;
    }

    private void InstantiateCharacters()
    {
        foreach (PlayableCharacter character in characterInformation.PartyCharacters)
        {
            var button = Instantiate(obj);
            button.transform.SetParent(container);
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(140, 140);
            button.transform.localScale = Vector2.one;

            button.GetComponent<UnityEngine.UI.Image>().sprite = character.UISprite;

            characterSprites.Add(button);

            characters.Add(character);
        }
        foreach (PlayableCharacter character in characterInformation.OwnedCharacters)
        {
            if (!character.IsInParty)
            {
                var button = Instantiate(obj);
                button.transform.SetParent(container);
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(140, 140);
                button.transform.localScale = Vector2.one;

                button.GetComponent<UnityEngine.UI.Image>().sprite = character.UISprite;

                characterSprites.Add(button);

                characters.Add(character);
            }
        }
    }

    private void ShowCharacterStats(PlayableCharacter character)
    {
        var sv = Instantiate(statsScrollView).GetComponent<ScrollView>();
        for (int i = 0; i < 7; i++)
        {
            var btn = Instantiate(statsButton);
        }
    }
}
