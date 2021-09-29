using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyTimeline : MonoBehaviour
{
    public int frameCounter;
    public float refreshTime;

    public Transform rightSpawn;
    public Transform centerSpawn;
    public Transform leftSpawn;
    
    public GameObject boss;
    public bool hasSpawnBoss;
    
    public List<EnemySlot> enemySlots = new List<EnemySlot>();

    
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) && hasSpawnBoss) hasSpawnBoss = false;
        
        if (frameCounter < refreshTime && boss == null && !hasSpawnBoss)
        {
            frameCounter++;
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
                    case SpawnPoint.RightSpawn:
                        InstantiateLeft(enemySlots[i].prefab, enemySlots[i]); 
                        break;
                    case SpawnPoint.CenterSpawn:
                        Instantiate(enemySlots[i].prefab, centerSpawn.transform.position, enemySlots[i].prefab.transform.rotation, centerSpawn);
                        Debug.Log("Enemy Center Spawn");
                        break;
                    case SpawnPoint.LeftSpawn:
                        Instantiate(enemySlots[i].prefab, leftSpawn.transform.position, enemySlots[i].prefab.transform.rotation, leftSpawn);
                        Debug.Log("Enemy Left Spawn");
                        break;
                }
            }
        }
    }

    void InstantiateLeft(GameObject prefab, EnemySlot enemySlot)
    {
        Debug.Log("Enemy Right Spawn");
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