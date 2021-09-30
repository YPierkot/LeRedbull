using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolBehavior : MonoBehaviour {
    public string bulletName; 
    public int waitForDestruction;
    [SerializeField] private int damage = 0;
    public int Damage => damage;

    private void OnEnable() {
        StartCoroutine(DestroyPooledObject());
    }

    /// <summary>
    /// Initialize damage for this bullet
    /// </summary>
    /// <param name="dam"></param>
    public void Init(int dam) {
        damage = dam;
    }
    
    IEnumerator DestroyPooledObject() {
        yield return new WaitForSeconds(waitForDestruction);
        EnnemyBulletPoolManager.instance.DestroyBullet(bulletName, this.gameObject);
    }
    
}
