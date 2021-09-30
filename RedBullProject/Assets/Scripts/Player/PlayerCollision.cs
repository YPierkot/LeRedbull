using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {
    [SerializeField] private PlayerManager playerData = null;
    [SerializeField] private int damagePerBullet = 0;
    [SerializeField] private bool useAnimation = true;
    [SerializeField] private Animator cameraAnimation = null;
    
    /// <summary>
    /// When the player enter in collision with an ennemy bullet
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("EnnemyBullet")) {
            playerData.TakeDamage(damagePerBullet);
            if(useAnimation) cameraAnimation.Play("TakeDamageCameraShake");
            EnnemyBulletPoolManager.instance.DestroyBullet(other.GetComponent<BulletPoolBehavior>().bulletName, other.gameObject);
        }
    }
}
