using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolBehavior : MonoBehaviour
{
    public string bulletName; 
    public int waitForDestruction;

    private void OnEnable()
    {
        StartCoroutine(DestroyPooledObject());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag($"Destroy"))
        {
            EnnemyBulletPoolManager.instance.DestroyBullet(bulletName, this.gameObject);
        }
    }
    

    IEnumerator DestroyPooledObject()
    {
        yield return new WaitForSeconds(waitForDestruction);
        EnnemyBulletPoolManager.instance.DestroyBullet(bulletName, this.gameObject);
    }
    
}
