using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Create Weapon SO/Busrt Weapon")]
public class BurstWeaponSO : BaseWeaponSO {
    [SerializeField] private float burstAngle = 15;
    [SerializeField] private bool randomShot = false;
    [SerializeField] private int nmbBulletPerShot = 1;
    
    #region GameplayFunction
    /// <summary>
    /// Get the rotation based on the angle
    /// </summary>
    /// <param name="burstAngle"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    private static Vector3 GetRotationAngle(float angle, Vector3 line, float distance) {
        Vector3 rotationVector = (Quaternion.Euler(0, angle, 0) * line) * distance;
        return rotationVector;
    }

    /// <summary>
    /// Get a random Direction
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private Vector3 GetRandomDirection(GameObject player) {
        float randomAngle = Random.Range(-burstAngle, burstAngle);
        Vector3 rotationDirection = GetRotationAngle(randomAngle, player.transform.forward, 1);
        return rotationDirection;
    }

    /// <summary>
    /// Override the shoot function
    /// </summary>
    /// <param name="player"></param>
    /// <param name="bulletSpawn"></param>
    /// <param name="bulletContainer"></param>
    public override void ShootBullet(GameObject player, Transform bulletSpawn, int damageUpgradeNmb, int bulletSizeUpgradeNmb, int bulletSpeedUpgradeNmb) {
        if (randomShot) {
            Vector3 direction = GetRandomDirection(player);
            GameObject bulletSpawned = GetBullet(bulletSpawn, player.transform);
            bulletSpawned.transform.localScale = new Vector3(GetBulletSize(bulletSizeUpgradeNmb), GetBulletSize(bulletSizeUpgradeNmb), GetBulletSize(bulletSizeUpgradeNmb));
            bulletSpawned.GetComponent<Rigidbody>().AddForce(direction * GetBulletSpeed(bulletSpeedUpgradeNmb), ForceMode.Impulse);
            Destroy(bulletSpawned, BulletDeathTime);
        }
        else {
            float startAngle = -burstAngle;
            float angleToAdd = (burstAngle * 2) / (nmbBulletPerShot - 1);
            
            for (int i = 0; i < nmbBulletPerShot; i++) {
                Vector3 direction = GetRotationAngle(startAngle + (i * angleToAdd), player.transform.forward, 1);
                GameObject bulletSpawned = GetBullet(bulletSpawn, player.transform);
                bulletSpawned.transform.localScale = new Vector3(GetBulletSize(bulletSizeUpgradeNmb), GetBulletSize(bulletSizeUpgradeNmb), GetBulletSize(bulletSizeUpgradeNmb));
                bulletSpawned.GetComponent<Rigidbody>().AddForce(direction * GetBulletSpeed(bulletSpeedUpgradeNmb), ForceMode.Impulse);
                Destroy(bulletSpawned, BulletDeathTime);
            }
        }
    }
    #endregion GameplayFunction
    
#if UNITY_EDITOR
    /// <summary>
    /// Override the function which is called in the OnDrawGizmos
    /// </summary>
    /// <param name="player"></param>
    public override void DrawCustomGizmos(Transform player, Transform bulletSpawn) {
        if (randomShot) {
            Vector3 rotation = GetRotationAngle(burstAngle, player.transform.forward, 10);
            Vector3 rotationMinus = GetRotationAngle(-burstAngle, player.transform.forward, 10);

            Gizmos.DrawLine(bulletSpawn.position, rotation + bulletSpawn.position);
            Gizmos.DrawLine(bulletSpawn.position, rotationMinus + bulletSpawn.position);
        }
        else {
            float startAngle = -burstAngle;
            float angleToAdd = (burstAngle * 2) / (nmbBulletPerShot - 1);
            
            for (int i = 0; i < nmbBulletPerShot; i++) {
                Vector3 direction = GetRotationAngle(startAngle + (i * angleToAdd), player.transform.forward, 10);
                Gizmos.DrawLine(bulletSpawn.position, direction + bulletSpawn.position);
            }
        }
    }
#endif
}
