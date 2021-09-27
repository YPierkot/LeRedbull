using UnityEngine;

[CreateAssetMenu(menuName = "Create Weapon SO/Base Weapon")]
public class BaseWeaponSO : ScriptableObject {
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

    public virtual void ShootBullet(GameObject player, Transform bulletSpawn, Transform bulletContainer) {
        
    }

    public virtual void InitPulledBullet() {
        
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
