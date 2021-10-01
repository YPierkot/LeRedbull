using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyText : MonoBehaviour
{
    /// <summary>
    /// Destroy the parent of the text
    /// </summary>
    public void DestroyGameObject() {
        EnnemyBulletPoolManager.instance.DestroyBullet("DamageCanvas", transform.parent.gameObject);
    }
}
