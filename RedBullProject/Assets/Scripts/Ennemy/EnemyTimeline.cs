using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyTimeline : MonoBehaviour
{
    public GameObject player;
    
    public float frameCounter;
    public float refreshTime;

    public Transform bossSpawn;
    public Transform leftSpawn;
    public Transform centerSpawn;
    public Transform rightSpawn;
    
    private GameObject boss;
    private bool hasSpawnBoss;
    
    public List<EnemySlot> enemySlots = new List<EnemySlot>();

    [SerializeField] private Animator alertCanvas = null;
    private bool hasSpawnAlert = false;

    private void Start() {
        alertCanvas.gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    IEnumerator waitForAlert(float time) {
        hasSpawnAlert = true;
        yield return new WaitForSeconds(time);
        alertCanvas.Play("GetCanvasAlpha");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return) && hasSpawnBoss && boss == null) {
            alertCanvas.Play("changeCanvasAlpha");
            hasSpawnBoss = false;
            hasSpawnAlert = false;
        }
        
        if (hasSpawnBoss && boss == null && !hasSpawnAlert) StartCoroutine(waitForAlert(1.5f));  
    }

    private void FixedUpdate() {
        if (player == null) return;
        
        if (frameCounter < refreshTime && boss == null && !hasSpawnBoss) {
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
                        if (enemySlots[i].prefab != null) InstantiateBoss(enemySlots[i].prefab, enemySlots[i]);
                        else {
                            StartCoroutine(waitForAlert(0.25f));
                            hasSpawnBoss = true;
                            frameCounter += 0.25f;
                        }
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