using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {
    [SerializeField] private PlayerManager playerData = null;
    [SerializeField] private int damagePerBullet = 0;
    [SerializeField] private Animator cameraAnimation = null;
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("EnnemyBullet")) {
            playerData.TakeDamage(damagePerBullet);
            cameraAnimation.Play("TakeDamageCameraShake");
        }
    }
}
