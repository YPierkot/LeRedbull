using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolBehavior : MonoBehaviour {
    public string bulletName; 
    public int waitForDestruction;
    [SerializeField] private float damage = 0;
    public float Damage => damage;

    private void OnEnable() {
        if(waitForDestruction > 0) StartCoroutine(DestroyPooledObject());
    }

    /// <summary>
    /// Initialize damage for this bullet
    /// </summary>
    /// <param name="dam"></param>
    public void Init(float dam) {
        damage = dam;
    }
    
    IEnumerator DestroyPooledObject() {
        yield return new WaitForSeconds(waitForDestruction);
        EnnemyBulletPoolManager.instance.DestroyBullet(bulletName, this.gameObject);
    }
    
}
