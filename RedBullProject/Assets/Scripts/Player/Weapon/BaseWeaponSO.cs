using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Weapon SO/Base Weapon")]
public class BaseWeaponSO : ScriptableObject {
    #region Variables
    [SerializeField] private string weaponName = "new Weapon";
    [SerializeField] private float fireRate = 0;
    [SerializeField] private int damage = 0;
    public string WeaponName => weaponName;
    public float FireRate => fireRate;
    public int Damage => damage;
    
    
    [SerializeField] private float bulletStartSpeed = 0;
    [SerializeField] private float bulletStartSize = 0;
    [SerializeField] private GameObject bulletGam = null;
    [SerializeField] private float bulletDeathTime = 0;
    protected float BulletStartSpeed => bulletStartSpeed;
    protected GameObject BulletGam => bulletGam;
    protected float BulletDeathTime => bulletDeathTime;
    #endregion Variables
    
    /// <summary>
    /// Function when need to shoot a bullet
    /// </summary>
    /// <param name="player"></param>
    /// <param name="bulletSpawn"></param>
    /// <param name="bulletContainer"></param>
    public virtual void ShootBullet(GameObject player, Transform bulletSpawn, int damageUpgradeNmb, int bulletSizeUpgradeNmb, int bulletSpeedUpgradeNmb) {
        GameObject bulletSpawned = GetBullet(bulletSpawn, player.transform, damageUpgradeNmb);
        bulletSpawned.transform.localScale = new Vector3(GetBulletSize(bulletSizeUpgradeNmb), GetBulletSize(bulletSizeUpgradeNmb), GetBulletSize(bulletSizeUpgradeNmb));
        bulletSpawned.GetComponent<Rigidbody>().AddForce(player.transform.forward * GetBulletSpeed(bulletSpeedUpgradeNmb), ForceMode.Impulse);
    }

    /// <summary>
    /// get the bullet from the pullManager
    /// </summary>
    /// <param name="bulletSpawn"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    protected GameObject GetBullet(Transform bulletSpawn, Transform player, int damageUpgradeNmb) {
        GameObject bullet = EnnemyBulletPoolManager.instance.GetBullet("PlayerBullet", bulletSpawn.position, player.rotation);
        bullet.transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
        float damageValue = (float)(damage + damage * ((damageUpgradeNmb * 10) / 100));
        bullet.GetComponent<BulletPoolBehavior>().Init(damageValue);
        return bullet;
    }
    
    /// <summary>
    /// Get the speed of the bullet
    /// </summary>
    /// <param name="bulletSpeedUpgradeNmb"></param>
    /// <returns></returns>
    protected virtual float GetBulletSpeed(int bulletSpeedUpgradeNmb) {
        return (BulletStartSpeed + bulletStartSpeed * ((bulletSpeedUpgradeNmb * 10) / 100));
    }

    /// <summary>
    /// get the size of the bullet
    /// </summary>
    /// <param name="bulletSizeUpgrade"></param>
    /// <returns></returns>
    protected float GetBulletSize(int bulletSizeUpgrade) {
        return (bulletStartSize + bulletStartSize * ((bulletSizeUpgrade * 25) / 100));
    }

#if UNITY_EDITOR
    /// <summary>
    /// Draw custom Gizmos based on the class of the weapon
    /// </summary>
    /// <param name="player"></param>
    /// <param name="bulletSpawn"></param>
    public virtual void DrawCustomGizmos(Transform player, Transform bulletSpawn) {
        Gizmos.DrawLine(bulletSpawn.position, player.forward * 10 + bulletSpawn.position);
    }
#endif
}