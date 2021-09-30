using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagaValue : MonoBehaviour
{
    public int HP = 1;
    private int dmg = 1;
    public GameObject parent;

    /// <summary>
    /// When the ennemy enter in collision with a bullet
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("PlayerBullet")) {
            HP -= other.GetComponent<BulletPoolBehavior>().Damage;
            Destroy(other.gameObject);
        }
        if (HP <= 0) DestroyEnemy();
    }

    /// <summary>
    /// Destroy the Ennemy
    /// </summary>
    private void DestroyEnemy() {
        Destroy(this.parent.gameObject);
    }
}
