using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyStandingEnemy : MonoBehaviour
{
    public int timeBeforeDestroy;
    private void Start()
    {
        StartCoroutine(DestroyEnemy());
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(this.gameObject);
    }
}
