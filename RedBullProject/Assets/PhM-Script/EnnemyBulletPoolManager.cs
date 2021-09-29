using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBulletPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Bullet
    {
        public string name;
        public GameObject prefab;
        public int size;
    }
    
    [Serializable]
    public class BulletPrefab
    {
        public string name;
        public GameObject prefab;
        public Transform prefabParent;

        public BulletPrefab(string name, GameObject prefab, Transform prefabParent)
        {
            this.name = name;
            this.prefab = prefab;
            this.prefabParent = prefabParent;
        }
    }

    public static EnnemyBulletPoolManager instance;
    
    public List<Bullet> bullets;
    public static Dictionary<string, Queue<GameObject>> poolDictionary;
    public List<BulletPrefab> prefabList = new List<BulletPrefab>(); 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Bullet bullet in bullets)
        {
            GameObject bulletParent = new GameObject();
            bulletParent.name = bullet.name + "Pool"; 
            GameObject.DontDestroyOnLoad(bulletParent);
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < bullet.size; i++)
            {
                GameObject obj = Instantiate(bullet.prefab, bulletParent.transform);
                obj.GetComponent<BulletPoolBehavior>().bulletName = bullet.name;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(bullet.name, objectPool);
            prefabList.Add(new BulletPrefab(bullet.name, bullet.prefab, bulletParent.transform));
        }
    }
    
    public GameObject GetBullet(string bulletName, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary.ContainsKey(bulletName))
        {
            if (poolDictionary[bulletName].Count > 0)
            {
                GameObject obj = poolDictionary[bulletName].Dequeue();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
            else
            {
                for (int i = 0; i < prefabList.Count; i++)
                {
                    if (bulletName == prefabList[i].name)
                    {
                        GameObject obj = Instantiate(prefabList[i].prefab, prefabList[i].prefabParent);
                        obj.GetComponent<BulletPoolBehavior>().bulletName = bulletName;
                        poolDictionary[bulletName].Enqueue(obj);


                        poolDictionary[bulletName].Dequeue();
                        obj.transform.position = position;
                        obj.transform.rotation = rotation;
                        return obj;
                    }
                }
            }
        }
        return null;
    }
    
    public void DestroyBullet(string bulletName, GameObject poolObject)
    {
        if (poolDictionary.ContainsKey(bulletName))
        {
            poolDictionary[bulletName].Enqueue(poolObject);
            poolObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            poolObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            poolObject.SetActive(false);
        }
    }
}
