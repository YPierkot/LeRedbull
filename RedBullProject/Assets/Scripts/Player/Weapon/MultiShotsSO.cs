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
    public override void ShootBullet(GameObject player, Transform bulletSpawn, Transform bulletContainer) {
        GameObject bulletSpawned = Instantiate(BulletGam, bulletSpawn.position - new Vector3(moveSpawnShot, 0 ,0), player.transform.rotation, bulletContainer);
        bulletSpawned.GetComponent<Rigidbody>().AddForce(player.transform.forward * BulletStartSpeed, ForceMode.Impulse);
        Destroy(bulletSpawned, BulletDeathTime);
        
        GameObject bulletSpawned2 = Instantiate(BulletGam, bulletSpawn.position + new Vector3(moveSpawnShot, 0 ,0), player.transform.rotation, bulletContainer);
        bulletSpawned2.GetComponent<Rigidbody>().AddForce(player.transform.forward * BulletStartSpeed, ForceMode.Impulse);
        Destroy(bulletSpawned2, BulletDeathTime);
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
