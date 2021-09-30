using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class DamagaValue : MonoBehaviour {
    public int life = 1;
    public GameObject parent;

    /// <summary>
    /// When the ennemy enter in collision with a bullet
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("PlayerBullet")) {
            life -= other.GetComponent<BulletPoolBehavior>().Damage;
            GameObject damageCanvas = EnnemyBulletPoolManager.instance.GetBullet("DamageCanvas", transform.position, GameManager.Instance.DamageText.transform.rotation);
            damageCanvas.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
            damageCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = other.GetComponent<BulletPoolBehavior>().Damage.ToString();
            GameObject damageEffect = Instantiate(GameManager.Instance.DamageEffect, other.transform.position - (other.transform.forward) + other.transform.up * .5f, Quaternion.Euler(-90, 0 ,-125));
            Destroy(other.gameObject);
        }
        if (life <= 0) DestroyEnemy();
    }

    /// <summary>
    /// Destroy the Ennemy
    /// </summary>
    private void DestroyEnemy() {
        Destroy(transform.parent.gameObject);
        GameManager.Instance.AddBasicRessource(5);
    }
}
