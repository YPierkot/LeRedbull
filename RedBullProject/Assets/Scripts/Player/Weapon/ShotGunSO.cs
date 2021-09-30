using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Weapon SO/Shotgun Weapon")]
public class ShotGunSO : BaseWeaponSO {
    [SerializeField] private float minStartBulletSpeed = 0f;
    [SerializeField] private float maxStartBulletSpeed = 0f;
    [SerializeField] private int numberOfBulletToSpawn = 0;
    [SerializeField] private float burstAngle = 15;
    
    #region Gameplay
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
    
    public override void ShootBullet(GameObject player, Transform bulletSpawn, int damageUpgradeNmb, int bulletSizeUpgradeNmb, int bulletSpeedUpgradeNmb) {
        for (int i = 0; i < numberOfBulletToSpawn; i++) {
            Vector3 direction = GetRandomDirection(player);
            GameObject bulletSpawned = GetBullet(bulletSpawn, player.transform);
            bulletSpawned.transform.localScale = new Vector3(GetBulletSize(bulletSizeUpgradeNmb), GetBulletSize(bulletSizeUpgradeNmb), GetBulletSize(bulletSizeUpgradeNmb));
            bulletSpawned.GetComponent<Rigidbody>().AddForce(direction * GetBulletSpeed(bulletSpeedUpgradeNmb), ForceMode.Impulse);
        }
    }

    protected override float GetBulletSpeed(int bulletSpeedUpgradeNmb) {
        float bulletStartSpeed = Random.Range(minStartBulletSpeed, maxStartBulletSpeed);
        return (bulletStartSpeed + bulletStartSpeed * ((bulletSpeedUpgradeNmb * 10) / 100));
    }

    #endregion Gameplay
    
#if UNITY_EDITOR
    /// <summary>
    /// Override the function which is called in the OnDrawGizmos
    /// </summary>
    /// <param name="player"></param>
    public override void DrawCustomGizmos(Transform player, Transform bulletSpawn) {
        Vector3 rotation = GetRotationAngle(burstAngle, player.transform.forward, 10);
        Vector3 rotationMinus = GetRotationAngle(-burstAngle, player.transform.forward, 10);

        Gizmos.DrawLine(bulletSpawn.position, rotation + bulletSpawn.position);
        Gizmos.DrawLine(bulletSpawn.position, rotationMinus + bulletSpawn.position);
    }
#endif
}
