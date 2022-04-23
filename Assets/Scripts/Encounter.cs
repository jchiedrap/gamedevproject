using System.Collections;
using System.Collections.Generic;
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
    
    public List<Image> enemyPortraits;
    public List<Image> playerPortraits;

    private void Start()
    {
        InitializeEncounter();
        encounterState = EncounterState.PlayerTurn;
    }

    private void InitializeEncounter()
    {
        enemies = encounterType.enemies;
        playerCharacters = LevelManager.Instance.partyController.partyMembers;
        
        foreach (Entity enemy in enemies) InitializeEnemy(enemy);
        foreach (PlayerCharacter playerCharacter in playerCharacters) InitializeCharacter(playerCharacter);
    }
    
    private void InitializeEnemy(Entity enemy)
    {
        //enemyPortraits.Add(enemy.portrait);
    }
    
    private void InitializeCharacter(PlayerCharacter playerCharacter)
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
