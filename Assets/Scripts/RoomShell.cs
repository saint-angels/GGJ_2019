using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomShell : SingletonComponent<RoomShell>
{
    public Color attackableColor;

    [SerializeField] private Room[] roomPrefabs;
    [SerializeField] private Room tutorialRoomPrefab;
    [SerializeField] private Room cellarRoomPrefab;
    [SerializeField] private EnemyBase[] enemyPrefabs;
    [SerializeField] private bool showTutorial = true;
    [SerializeField] private UnityEngine.UI.Slider chargeLevel;
    [SerializeField] private GameObject gameOverUI;

    [Header("Test levels")]
    [SerializeField] private bool testSpecificRoom = false;
    [SerializeField] private int specificRoomIndex = 0;

    Room currentRoom;
    public bool gameOver;

    List<EnemyBase> spawnedEnemies = new List<EnemyBase>();
    private List<Room> unusedRoomPrefabs;

    private bool showedTutorialRoom = false;
    

    public void SetChargeLevel(float chargeLevel)
    {
        this.chargeLevel.value = chargeLevel;
    }

    public void Restart()
    {
        gameOver = false;
        unusedRoomPrefabs = new List<Room>(roomPrefabs);
        foreach (var bullet in FindObjectsOfType<EnemyBullet>())
        {
            Destroy(bullet.gameObject);
        }

        foreach (var enemy in spawnedEnemies)
        {
            Destroy(enemy.gameObject);
        }
        spawnedEnemies.Clear();
        Player.Instance.Init();
        GenerateNewRoom();
    }

    public void GameOver()
    {
        gameOver = true;
        foreach (var enemy in FindObjectsOfType<EnemyBase>())
        {
            enemy.Stop();
        }

        gameOverUI.SetActive(true);
    }

    public void GenerateNewRoom()
    {
        gameOverUI.SetActive(false);
        if (currentRoom != null)
        {
            Destroy(currentRoom.gameObject);
        }

        Room selectedPrefab;
        if (testSpecificRoom)
        {
            selectedPrefab = roomPrefabs[specificRoomIndex];
        }
        else
        {
            if (showedTutorialRoom == false && showTutorial)
            {
                selectedPrefab = tutorialRoomPrefab;
                showedTutorialRoom = true;
            }
            else
            {
                if (unusedRoomPrefabs.Count < 3)
                {
                    selectedPrefab = cellarRoomPrefab;
                }
                else
                {
                    selectedPrefab = unusedRoomPrefabs[Random.Range(0, unusedRoomPrefabs.Count)];
                    unusedRoomPrefabs.Remove(selectedPrefab);
                }
            }
        }
        
        currentRoom = Instantiate(selectedPrefab, transform);
        Player.Instance.transform.position = currentRoom.playerSpawnPoint.position;
    }

    public EnemyBase SpawnRandomEnemy(Vector3 atPoint)
    {
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        EnemyBase selectedPrefab = enemyPrefabs[randomEnemyIndex];
        var newEnemy = Instantiate(selectedPrefab, atPoint, Quaternion.identity);
        spawnedEnemies.Add(newEnemy);
        newEnemy.OnDeath += () => spawnedEnemies.Remove(newEnemy);
        return newEnemy;
    }

    void Start()
    {
        Restart();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }
}
