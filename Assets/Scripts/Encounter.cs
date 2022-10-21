using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Encounter : MonoBehaviour
{
    public enum EncounterState
    {
        PlayerTurn,
        EnemyTurn,
        PlayerLoss,
        PlayerWin,
        PlayerEscape
    }
    
    public EncounterType encounterType;
    public EncounterState encounterState;

    public List<Entity> enemies;
    public List<PlayerCharacter> playerCharacters;
    
    public List<Sprite> enemyPortraits;
    public List<Sprite> playerPortraits;
    
    public List<Slider> enemyHealthBars;
    public List<Slider> playerHealthBars;
    public List<Slider> playerMPBars;
    
    public List<int> enemyMaxHealth;
    public List<int> enemyCurrentHealth;
    public List<int> playerMaxHealth;
    public List<int> playerCurrentHealth;
    public List<int> playerMaxMP;
    public List<int> playerCurrentMP;
    
    
    public List<string> enemyNames;
    public List<string> playerNames;
    
    public List<TMP_Text> enemyHealthText;
    public List<TMP_Text> playerHealthText;
    public List<TMP_Text> playerMPText;

    public List<TMP_Text> enemyNameText;
    public List<TMP_Text> playerNameText;


    private void Start()
    {
        InitializeEncounter();
        encounterState = EncounterState.PlayerTurn;
    }

    private void InitializeEncounter()
    {
        enemies = encounterType.enemies;
        playerCharacters = LevelManager.Instance.partyController.partyMembers;
        
        foreach (var enemy in enemies) InitializeEnemy(enemy);
        foreach (var playerCharacter in playerCharacters) InitializeCharacter(playerCharacter);

        //InitializeUI();
    }
    
    private void InitializeEnemy(Entity enemy)
    {
        enemyPortraits.Add(enemy.portrait);
        enemyMaxHealth.Add(enemy.maxHP);
        enemyCurrentHealth.Add(enemy.maxHP);
        enemyNames.Add(enemy.name);
    }
    
    private void InitializeCharacter(PlayerCharacter playerCharacter)
    {
        playerPortraits.Add(playerCharacter.portrait);
        playerMaxHealth.Add(playerCharacter.maxHP);
        playerMaxMP.Add(playerCharacter.maxMP);
        playerNames.Add(playerCharacter.name); 
        playerCurrentHealth.Add(playerCharacter.currentHP);
        playerCurrentMP.Add(playerCharacter.currentMP);
    }
}
