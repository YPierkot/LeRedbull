using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DamagaValue : MonoBehaviour {
    public float life = 1;
    public GameObject parent;
    [SerializeField] private string AnimName = "TakeDamageEnnemy";

    /// <summary>
    /// When the ennemy enter in collision with a bullet
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("PlayerBullet")) {
            life -= other.GetComponent<BulletPoolBehavior>().Damage;
            GameObject damageCanvas = EnnemyBulletPoolManager.instance.GetBullet("DamageCanvas", other.transform.position, GameManager.Instance.DamageText.transform.rotation);
            damageCanvas.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
            damageCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = other.GetComponent<BulletPoolBehavior>().Damage.ToString();
            Instantiate(GameManager.Instance.DamageEffect, other.transform.position - (other.transform.forward) + other.transform.up * .5f, Quaternion.Euler(-90, 0 ,-125));
            GetComponent<Animator>().Play(AnimName);
            GameManager.Instance.EnnemyTakeDamageSound();
            Destroy(other.gameObject);
        }
        if (life <= 0) DestroyEnemy();
    }

    /// <summary>
    /// Destroy the Ennemy
    /// </summary>
    private void DestroyEnemy() {
        GameManager.Instance.AddBasicRessource();
        GameManager.Instance.AddEnnemyKillToContract();
        Instantiate(GameManager.Instance.DeathEffect, transform.position, GameManager.Instance.DeathEffect.transform.rotation);
        Destroy(transform.parent.gameObject);
    }
}
