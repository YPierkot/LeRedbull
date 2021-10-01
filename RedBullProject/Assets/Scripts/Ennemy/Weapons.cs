using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class Weapons : MonoBehaviour
{
    public bool canActivate;
    private float frameCounter;
    public float refreshTime;


    public GameObject ennemyPrefab;

    public List<ActionClass> actionClasses = new List<ActionClass>();
    
    private void FixedUpdate()
    {
        if (canActivate)
        {
            if (frameCounter < refreshTime)
            {
                frameCounter += 0.25f;
            }
            else
            { 
                frameCounter = 0;
            }
        
            for (int i = 0; i < actionClasses.Count; i++)
            {
                if (frameCounter == actionClasses[i].frames)
                {
                    switch (actionClasses[i].actionsEnum)
                    {
                        case ActionsEnum.FIRE_BURST:
                            StartCoroutine(FireBurst(actionClasses[i].fireBurst));
                            break;
                        case ActionsEnum.RANDOM_FIRE:
                            RandomFire(actionClasses[i].randomFire);
                            break;
                        case ActionsEnum.FIRE_STRAIGHT:
                            FireStraight(actionClasses[i].fireStraight);
                            break;
                        case ActionsEnum.FIRE_CORNER:
                            StartCoroutine(FireCorner(actionClasses[i].fireCorner));
                            break;
                    }
                }
            }
        }
        
    }

    IEnumerator FireBurst(FireBurst fireBurst)
    {
        for (int j = 0; j < fireBurst.bulletNumber; j++)
        {
            for (int i = 0; i < fireBurst.burstBullets + 1; i++)
            {
                var angle = -fireBurst.burstAngle +
                            (i * ((fireBurst.burstAngle * 2) / fireBurst.burstBullets));

                angle += fireBurst.burstRotation * j;

                Vector3 rotation = Quaternion.Euler(0, angle, 0) * ennemyPrefab.transform.forward;

                GameObject bullet = EnnemyBulletPoolManager.instance.GetBullet("BurstBullet",
                    ennemyPrefab.transform.position, ennemyPrefab.transform.rotation);
                bullet.transform.localScale = bullet.transform.localScale * (fireBurst.bulletSizeFactor + 1);
                bullet.GetComponent<Rigidbody>().AddForce(rotation * fireBurst.burstSpeed, ForceMode.Impulse);

            }
            
            yield return new WaitForSeconds(fireBurst.timeBetweenShoot);
        }
    }

    void RandomFire(RandomFire randomFire)
    {
        for (int i = 0; i < randomFire.bulletNumber; i++)
        {
            var angle = Random.Range(-randomFire.AngleRandom, randomFire.AngleRandom);
            var shift = Random.Range(-randomFire.positionRandom, randomFire.positionRandom);
            var breakou = Random.Range(-randomFire.speedRandom, randomFire.speedRandom);

            var rotation = Quaternion.Euler(0, angle, 0) * ennemyPrefab.transform.forward;
            var position = ennemyPrefab.transform.position + new Vector3(shift, 0, 0);
            var finalSpeed = randomFire.bulletSpeed + breakou;

            GameObject bullet =
                EnnemyBulletPoolManager.instance.GetBullet("RandomBullet", position, ennemyPrefab.transform.rotation);
            bullet.transform.localScale = bullet.transform.localScale * (randomFire.bulletSizeFactor + 1);
            bullet.GetComponent<Rigidbody>().AddForce(rotation * finalSpeed, ForceMode.Impulse);
        }
    }

    void FireStraight(FireStraight fireStraight)
    {
        var randomNumberY = Random.Range(-fireStraight.bulletMissShoot, fireStraight.bulletMissShoot);

        Vector3 direction = Quaternion.Euler(0, randomNumberY, 0) * Quaternion.Euler(0,fireStraight.orientationAngle,0) * ennemyPrefab.transform.forward;

        GameObject bullet = EnnemyBulletPoolManager.instance.GetBullet("StraightBullet",
            ennemyPrefab.transform.position, ennemyPrefab.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(direction * fireStraight.ballSpeed, ForceMode.Impulse);
    }

    IEnumerator FireCorner(FireCorner fireCorner)
    {
        var forward = ennemyPrefab.transform.forward;
        Vector3 direction1 = Quaternion.Euler(0, 45, 0) * forward;
        Vector3 direction2 = Quaternion.Euler(0, 135, 0) * forward;
        Vector3 direction3 = Quaternion.Euler(0, 225, 0) * forward;
        Vector3 direction4 = Quaternion.Euler(0, 315, 0) * forward;

        var position = ennemyPrefab.transform.position;
        var rotation = ennemyPrefab.transform.rotation;

        for (int i = 0; i < fireCorner.bulletNumber; i++)
        {
            GameObject bullet1 = EnnemyBulletPoolManager.instance.GetBullet("CornerBullet",
                position, rotation);
            bullet1.GetComponent<Rigidbody>().AddForce(direction1 * fireCorner.bulletSpeed, ForceMode.Impulse);

            GameObject bullet2 = EnnemyBulletPoolManager.instance.GetBullet("CornerBullet",
                position, rotation);
            bullet2.GetComponent<Rigidbody>().AddForce(direction2 * fireCorner.bulletSpeed, ForceMode.Impulse);

            GameObject bullet3 = EnnemyBulletPoolManager.instance.GetBullet("CornerBullet",
                position, rotation);
            bullet3.GetComponent<Rigidbody>().AddForce(direction3 * fireCorner.bulletSpeed, ForceMode.Impulse);

            GameObject bullet4 = EnnemyBulletPoolManager.instance.GetBullet("CornerBullet",
                position, rotation);
            bullet4.GetComponent<Rigidbody>().AddForce(direction4 * fireCorner.bulletSpeed, ForceMode.Impulse);

            yield return new WaitForSeconds(fireCorner.timeBetweenShoot);
        }
    }
}

public enum ActionsEnum
{
    FIRE_BURST,
    RANDOM_FIRE,
    FIRE_STRAIGHT,
    FIRE_CORNER,
}


[Serializable]
public class ActionClass
{
    public string name;
    public int frames;
    public ActionsEnum actionsEnum;
    public FireBurst fireBurst;
    public RandomFire randomFire;
    public FireStraight fireStraight;
    public FireCorner fireCorner;
}

[Serializable]
public class FireBurst
{
    public float burstSpeed; 
    public int burstBullets;
    public float burstAngle;
    public int bulletNumber;
    public float timeBetweenShoot;
    public float burstRotation;
    public float bulletSizeFactor;
}

[Serializable]
public class RandomFire
{
    public int bulletNumber;
    public float AngleRandom;
    public float positionRandom;
    public float speedRandom;
    public float bulletSpeed;
    public float bulletSizeFactor;
}

[Serializable]
public class FireStraight
{
    public float ballSpeed;
    public float bulletMissShoot;
    public int orientationAngle;
}

[Serializable]
public class FireCorner
{
    public float bulletSpeed;
    public int bulletNumber;
    public float timeBetweenShoot;
}
