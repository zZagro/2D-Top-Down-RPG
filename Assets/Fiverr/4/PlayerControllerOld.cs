using UnityEngine;
using TMPro;
public class PlayerControllerOld : MonoBehaviour {
    [SerializeField] private PlayableCharacter[] _characters;
    private PlayableCharacter _currentCharacter;
    [SerializeField] private TextMeshProUGUI _characterDataTxt;

    private void Start() {
        //PrintData();
        //PlayerPrefs.SetInt("Coins", 100);
    }

    /*public void UpgradeAttack() {
        int costs = 100;
        if(costs <= PlayerPrefs.GetInt("Coins")) {
            _currentCharacter.Attack++;
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - 100);
        }
    }

    private int _currentCharacterIndex = 0;
    public void NextCharacter() {
        _currentCharacterIndex++;
        if(_currentCharacterIndex >= _characters.Length) {
            _currentCharacterIndex = 0;
        }
        _currentCharacter = _characters[_currentCharacterIndex];

        _characterDataTxt.text = _currentCharacter.ToString();
    }
    public void AttackEnemy(EnemyController enemy) {
        int rnd = Random.Range(0, 101);
        float damage = _currentCharacter.Attack;
        if(rnd <= _currentCharacter.CriticalChance) {
            damage *= _currentCharacter.CriticalMultiplyer;
        }

        if(enemy.GetDamage(damage)) {
            _currentCharacter.GainXp(100);
        }
        PrintData();
    }

    public void AddEquipment(Equipment equip) {
        _currentCharacter.AddEquipment(equip);
        PrintData();
    }
    public void RemoveEquipment(Equipment equip) {
        _currentCharacter.RemoveEquipment(equip);
        PrintData();
    }

    private void PrintData() {
        _currentCharacter = _characters[_currentCharacterIndex];
        _characterDataTxt.text = _currentCharacter.ToString();
    }*/
}