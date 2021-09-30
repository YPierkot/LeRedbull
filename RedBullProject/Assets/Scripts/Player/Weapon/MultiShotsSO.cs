using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Weapon SO/MultiShot Weapon")]
public class MultiShotsSO : BaseWeaponSO {
    [SerializeField] private float moveSpawnShot = 0f;
    
    /// <summary>
    /// Override the shoot function
    /// </summary>
    /// <param name="player"></param>
    /// <param name="bulletSpawn"></param>
    /// <param name="bulletContainer"></param>
    public override void ShootBullet(GameObject player, Transform bulletSpawn, int damageUpgradeNmb, int bulletSizeUpgradeNmb, int bulletSpeedUpgradeNmb) {
        GameObject bulletSpawned = GetBullet(bulletSpawn, player.transform);
        bulletSpawned.transform.localScale = new Vector3(GetBulletSize(bulletSizeUpgradeNmb), GetBulletSize(bulletSizeUpgradeNmb), GetBulletSize(bulletSizeUpgradeNmb));
        bulletSpawned.GetComponent<Rigidbody>().AddForce(player.transform.forward * GetBulletSpeed(bulletSpeedUpgradeNmb), ForceMode.Impulse);

        GameObject bulletSpawned2 = GetBullet(bulletSpawn, player.transform);
        bulletSpawned2.transform.localScale = new Vector3(GetBulletSize(bulletSizeUpgradeNmb), GetBulletSize(bulletSizeUpgradeNmb), GetBulletSize(bulletSizeUpgradeNmb));
        bulletSpawned2.GetComponent<Rigidbody>().AddForce(player.transform.forward * GetBulletSpeed(bulletSpeedUpgradeNmb), ForceMode.Impulse);
    }

#if  UNITY_EDITOR
    /// <summary>
    /// This function allow the user to make gizmos in the scene
    /// </summary>
    /// <param name="player"></param>
    /// <param name="bulletSpawn"></param>
    public override void DrawCustomGizmos(Transform player, Transform bulletSpawn) {
        Gizmos.DrawLine(bulletSpawn.position + new Vector3(moveSpawnShot, 0 ,0), player.forward * 10 + bulletSpawn.position + new Vector3(moveSpawnShot, 0 ,0));
        Gizmos.DrawLine(bulletSpawn.position - new Vector3(moveSpawnShot, 0 ,0), player.forward * 10 + bulletSpawn.position - new Vector3(moveSpawnShot, 0 ,0));
    }
#endif
}
