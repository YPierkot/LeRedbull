using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestroy : MonoBehaviour {
    
    /// <summary>
    /// Destroy Bullet if they touch a wall
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("EnnemyBullet") || other.gameObject.CompareTag("PlayerBullet")) {
            EnnemyBulletPoolManager.instance.DestroyBullet(other.GetComponent<BulletPoolBehavior>().bulletName, other.gameObject);
        }
    }
}
