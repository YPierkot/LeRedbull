using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class Weapons : MonoBehaviour
{
    private int frameCounter;
    public float refreshTime;

    public GameObject ennemyPrefab;
    
    public List<ActionClass> actionClasses = new List<ActionClass>();

    private void Update()
    {
        if (frameCounter < refreshTime)
        {
            frameCounter++;
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
                        FireBurst(actionClasses[i].fireBurst);
                        break;
                    case ActionsEnum.FIRE_STRAIGHT:
                        FireStraight(actionClasses[i].fireStraight);
                        break;
                    case ActionsEnum.FIRE_CORNER:
                        FireCorner(actionClasses[i].fireCorner);
                        break;
                }
            }
        }
    }

    void FireBurst(FireBurst fireBurst)
    {
        for (int i = 0; i < fireBurst.burstBullets + 1; i++)
        {
            
            var angle = -fireBurst.burstAngle +
                    (i * ((fireBurst.burstAngle * 2) / fireBurst.burstBullets));
            
            Vector3 rotation = Quaternion.Euler(0, angle, 0) * ennemyPrefab.transform.forward;
            
            GameObject bullet = EnnemyBulletPoolManager.instance.GetBullet("BurstBullet", ennemyPrefab.transform.position, ennemyPrefab.transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(rotation * fireBurst.burstSpeed, ForceMode.Impulse);
        }
    }

    void FireStraight(FireStraight fireStraight)
    {
        var randomNumberY = Random.Range(-fireStraight.bulletMissShoot, fireStraight.bulletMissShoot);

        Vector3 direction = Quaternion.Euler(0, randomNumberY, 0) * ennemyPrefab.transform.forward;

        GameObject bullet = EnnemyBulletPoolManager.instance.GetBullet("StraightBullet",
            ennemyPrefab.transform.position, ennemyPrefab.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(direction * fireStraight.ballSpeed, ForceMode.Impulse);
    }

    void FireCorner(FireCorner fireCorner)
    {
        var forward = ennemyPrefab.transform.forward;
        Vector3 direction1 = Quaternion.Euler(0, 45, 0) * forward; 
        Vector3 direction2 = Quaternion.Euler(0, 135, 0) * forward; 
        Vector3 direction3 = Quaternion.Euler(0, 225, 0) * forward; 
        Vector3 direction4 = Quaternion.Euler(0, 315, 0) * forward; 
            
        for (int i = 0; i < fireCorner.bulletNumber; i++)
        {
            var position = ennemyPrefab.transform.position;
            var rotation = ennemyPrefab.transform.rotation;

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
        }
    }
}

public enum ActionsEnum
{
    FIRE_BURST,
    FIRE_STRAIGHT,
    FIRE_CORNER,
}


[Serializable]
public class ActionClass
{
    public int frames;
    public ActionsEnum actionsEnum;
    public FireBurst fireBurst;
    public FireStraight fireStraight;
    public FireCorner fireCorner;
}

[Serializable]
public class FireBurst
{
    public float burstSpeed; 
    public int burstBullets;
    public float burstAngle;
}

[Serializable]
public class FireStraight
{
    public float ballSpeed;
    public float bulletMissShoot;
}

[Serializable]
public class FireCorner
{
    public float bulletSpeed;
    public int bulletNumber;
}
