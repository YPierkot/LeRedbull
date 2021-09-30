using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagaValue : MonoBehaviour
{
    public int HP = 1;
    private int dmg = 1;
    public GameObject parent;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PBullet"))
        {
            HP -= dmg;
            Destroy(other.gameObject);
        }
        if (HP <= 0) DestroyEnemy();
    }

    public void DestroyEnemy()
    {
        Destroy(this.parent.gameObject);
    }
}
