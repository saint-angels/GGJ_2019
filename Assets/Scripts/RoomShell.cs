using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomShell : SingletonComponent<RoomShell>
{
    public Color attackableColor;

    [SerializeField] private Room[] roomPrefabs;
    [SerializeField] private Room tutorialRoomPrefab;
    [SerializeField] private EnemyBase[] enemyPrefabs;
    [SerializeField] private bool showTutorial = true;
    [SerializeField] private UnityEngine.UI.Slider chargeLevel;

    Room currentRoom;

    List<EnemyBase> spawnedEnemies = new List<EnemyBase>();

    private bool showedTutorialRoom = false;

    public void SetChargeLevel(float chargeLevel)
    {
        this.chargeLevel.value = chargeLevel;
    }

    public void Restart()
    {
        foreach (var enemy in spawnedEnemies)
        {
            Destroy(enemy.gameObject);
        }
        spawnedEnemies.Clear();
        GenerateNewRoom();
    }

    public void GenerateNewRoom()
    {
        if (currentRoom != null)
        {
            Destroy(currentRoom.gameObject);
        }

        Room selectedPrefab;
        if (showedTutorialRoom == false && showTutorial)
        {
            selectedPrefab = tutorialRoomPrefab;
            showedTutorialRoom = true;
        }
        else
        {
            selectedPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
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
        GenerateNewRoom();    
    }


    void Update()
    {
        
    }
}
