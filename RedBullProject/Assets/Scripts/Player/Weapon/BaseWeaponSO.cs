using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Weapon SO/Base Weapon")]
public class BaseWeaponSO : ScriptableObject {
    #region Variables
    [SerializeField] private string weaponName = "new Weapon";
    [SerializeField] private float fireRate = 0;
    [SerializeField] private int damage = 0;

    [SerializeField] private float bulletStartSpeed = 0;
    [SerializeField] private GameObject bulletGam = null;
    [SerializeField] private float bulletDeathTime = 0;
    
    public string WeaponName => weaponName;
    public float FireRate => fireRate;
    public int Damage => damage;
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
    public virtual void ShootBullet(GameObject player, Transform bulletSpawn, Transform bulletContainer) 
    {
        GameObject bulletSpawned = Instantiate(BulletGam, bulletSpawn.position, player.transform.rotation, bulletContainer);
        bulletSpawned.GetComponent<Rigidbody>().AddForce(player.transform.forward * BulletStartSpeed, ForceMode.Impulse);
        Destroy(bulletSpawned, BulletDeathTime);
    }
    
    public virtual void InitPulledBullet() { }
    
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
