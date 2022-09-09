using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Character[] playerTeam;
    public Character[] enemyTeam;

    public List<Character> allCharacters = new List<Character>();

    [Header("Components")]
    public Transform[] playerTeamSpaws;
    public Transform[] enemyTeamSpaws;

    [Header("Data")]
    public PlayerPersistentData playerPersistentData;
    public CharacterSet defaultEnemySet;

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        CreateCharacters(playerPersistentData, defaultEnemySet);
        TurnManager.instance.Begin();
    }

    void CreateCharacters(PlayerPersistentData playerData, CharacterSet enemyTeamSet)
    {
        playerTeam = new Character[playerData.characters.Length];
        enemyTeam = new Character[enemyTeamSet.characters.Length];

        int playerSpawnIndex = 0;

        for(int i = 0; i < playerData.characters.Length; i++)
        {
            if (!playerData.characters[i].isDead)
            {
                Character character = CreateCharacter(playerData.characters[i].characterPrefab, playerTeamSpaws[playerSpawnIndex]);
                character.curHp = playerData.characters[i].health;
                playerTeam[i] = character;
                playerSpawnIndex++;
            }
            else
            {
                playerTeam[i] = null;
            }
        }

        for(int i = 0; i < enemyTeamSet.characters.Length; i++)
        {
            Character character = CreateCharacter(enemyTeamSet.characters[i], enemyTeamSpaws[i]);
            enemyTeam[i] = character;
        }

        allCharacters.AddRange(playerTeam);
        allCharacters.AddRange(enemyTeam);
    }
    Character CreateCharacter(GameObject characterPrefab, Transform spawnPos)
    {
        GameObject obj = Instantiate(characterPrefab, spawnPos.position, spawnPos.rotation);
        return obj.GetComponent<Character>();
    }
    public void OnCharacterKilled(Character character)
    {
        allCharacters.Remove(character);

        int playersRemaining = 0;
        int enemiesRemaining = 0;

        for (int i = 0; i < allCharacters.Count; i++)
        {
            if (allCharacters[i].team == Character.Team.Player)
                playersRemaining++;
            else
                enemiesRemaining++;
        }

        // Did the player team win?
        if (enemiesRemaining == 0)
        {
            PlayerTeamWins();
        }
        // Did the enemy team win?
        else if (playersRemaining == 0)
        {
            EnemyTeamWins();
        }
    }
    void PlayerTeamWins()
    {
        UpdatePlayerPersistentData();
        Invoke(nameof(LoadMapScene), 0.5f);
    }

    // Called when the enemy team wins.
    void EnemyTeamWins()
    {
        playerPersistentData.ResetCharacters();
        Invoke(nameof(LoadMapScene), 0.5f);
    }

    void UpdatePlayerPersistentData()
    {
        for (int i = 0; i < playerTeam.Length; i++)
        {
            if (playerTeam[i] != null)
            {
                playerPersistentData.characters[i].health = playerTeam[i].curHp;
            }
            else
            {
                playerPersistentData.characters[i].isDead = true;
            }
        }
    }

    void LoadMapScene()
    {
        SceneManager.LoadScene("Map");
    }
}
