using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInformation : MonoBehaviour
{
    public List<PlayableCharacter> AllCharacters = new List<PlayableCharacter>();
    public List<PlayableCharacter> OwnedCharacters = new List<PlayableCharacter>();
    public List<PlayableCharacter> PartyCharacters = new List<PlayableCharacter>();

    public void SetOwnedCharacters()
    {
        OwnedCharacters.Clear();
        foreach (PlayableCharacter character in AllCharacters)
        {
            if (character.IsOwned)
            {
                OwnedCharacters.Add(character);
            }
        }
    }

    public void SetPartyCharacters()
    {
        PartyCharacters.Clear();
        foreach (PlayableCharacter character in OwnedCharacters)
        {
            if (character.IsInParty)
            {
                PartyCharacters.Add(character);
            }
        }
    }

    public void AddPartyCharacters()
    {
        foreach (PlayableCharacter character in PartyCharacters)
        {
            if (!character.IsInParty)
            {
                character.IsInParty = true;
            }
        }
        SetPartyCharacters();
    }

    public void AddOwnedCharacter(PlayableCharacter character)
    {
        OwnedCharacters.Add(character);
    }

    public void RemoveOwnedCharacter(PlayableCharacter character)
    {
        OwnedCharacters.Remove(character);
    }

    public void AddPartyCharacter(PlayableCharacter character)
    {
        PartyCharacters.Add(character);
    }

    public void RemovePartyCharacter(PlayableCharacter character)
    {
        PartyCharacters.Remove(character);
    }
}
