using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyTimeline : MonoBehaviour
{
    public float frameCounter;
    public float refreshTime;

    public Transform bossSpawn;
    public Transform leftSpawn;
    public Transform centerSpawn;
    public Transform rightSpawn;
    
    private GameObject boss;
    private bool hasSpawnBoss;
    
    public List<EnemySlot> enemySlots = new List<EnemySlot>();

    
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) && hasSpawnBoss) hasSpawnBoss = false;
        
        if (frameCounter < refreshTime && boss == null && !hasSpawnBoss)
        {
            frameCounter += 0.25f;
        }
        else if (frameCounter >= refreshTime && boss == null && !hasSpawnBoss)
        {
            frameCounter = 0;
        }
        
        for (int i = 0; i < enemySlots.Count; i++)
        {
            if (frameCounter == enemySlots[i].frame)
            {
                switch (enemySlots[i].spawnPoint)
                {
                    case SpawnPoint.BossSpawn:
                        InstantiateBoss(enemySlots[i].prefab, enemySlots[i]); 
                        break;
                    case SpawnPoint.LeftSpawn:
                        InstantiateLeft(enemySlots[i].prefab, enemySlots[i]); 
                        break;
                    case SpawnPoint.CenterSpawn:
                        InstantiateCenter(enemySlots[i].prefab, enemySlots[i]); 
                        break;
                    case SpawnPoint.RightSpawn:
                        InstantiateRight(enemySlots[i].prefab, enemySlots[i]); 
                        break;
                }
            }
        }
    }

    void InstantiateBoss(GameObject prefab, EnemySlot enemySlot)
    {
        Quaternion rotation = prefab.transform.rotation;

        if (enemySlot.flip)
            rotation = rotation * Quaternion.Euler(0, 0, 180);

        GameObject newGameObject = Instantiate(prefab, bossSpawn.transform.position, rotation, bossSpawn);
        if (enemySlot.isBoss)
        {
            boss = newGameObject;
            frameCounter++;
            hasSpawnBoss = true;
            Destroy(boss, enemySlot.timeBeforeDestroy);
            return;
        }

        Destroy(newGameObject, enemySlot.timeBeforeDestroy);
    }
    
    void InstantiateLeft(GameObject prefab, EnemySlot enemySlot)
    {
        Quaternion rotation = prefab.transform.rotation;

        if (enemySlot.flip)
            rotation = rotation * Quaternion.Euler(0, 0, 180);

        GameObject newGameObject = Instantiate(prefab, leftSpawn.transform.position, rotation, leftSpawn);
        if (enemySlot.isBoss)
        {
            boss = newGameObject;
            frameCounter++;
            hasSpawnBoss = true;
            Destroy(boss, enemySlot.timeBeforeDestroy);
            return;
        }

        Destroy(newGameObject, enemySlot.timeBeforeDestroy);
    }
    
    void InstantiateCenter(GameObject prefab, EnemySlot enemySlot)
    {
        Quaternion rotation = prefab.transform.rotation;

        if (enemySlot.flip)
            rotation = rotation * Quaternion.Euler(0, 0, 180);

        GameObject newGameObject = Instantiate(prefab, centerSpawn.transform.position, rotation, centerSpawn);
        if (enemySlot.isBoss)
        {
            boss = newGameObject;
            frameCounter++;
            hasSpawnBoss = true;
            Destroy(boss, enemySlot.timeBeforeDestroy);
            return;
        }

        Destroy(newGameObject, enemySlot.timeBeforeDestroy);
    }
    
    void InstantiateRight(GameObject prefab, EnemySlot enemySlot)
    {
        Quaternion rotation = prefab.transform.rotation;

        if (enemySlot.flip)
            rotation = rotation * Quaternion.Euler(0, 0, 180);

        GameObject newGameObject = Instantiate(prefab, rightSpawn.transform.position, rotation, rightSpawn);
        if (enemySlot.isBoss)
        {
            boss = newGameObject;
            frameCounter++;
            hasSpawnBoss = true;
            Destroy(boss, enemySlot.timeBeforeDestroy);
            return;
        }

        Destroy(newGameObject, enemySlot.timeBeforeDestroy);
    }

}

public enum SpawnPoint
{
    BossSpawn,
    RightSpawn,
    CenterSpawn,
    LeftSpawn,
}

[Serializable]
public class EnemySlot
{
    public string name;
    public int frame;
    public GameObject prefab;
    public SpawnPoint spawnPoint;
    public float timeBeforeDestroy;
    public bool flip;
    public bool isBoss;
}