using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.LWRP;

public class PlayerManager : MonoBehaviour {
    #region Variables
    [Header("Player Infos")] 
    [SerializeField] private GameObject playerGam = null;
    [SerializeField] private Transform shootPos = null;
    [SerializeField] private int life = 0;
    [SerializeField] private int maxLife = 0;

    [Header("Player Movement")] 
    [SerializeField] private float moveSpeed = 0;

    [Header("Player Shoot")] [SerializeField]
    private BaseWeaponSO actualWeapon;
    [SerializeField] private Transform bulletContainer = null;

    #region privateVariable
    //Rigidbody
    private Rigidbody playerRig = null;
    
    private Vector3 moveDirection = new Vector3();
    private Vector3 moveDirectionRaw = new Vector3();
    
    //Shoot Data
    private float actualFireRate = 0;
    
    #endregion PrivateVariable
    
    #endregion Variables
    
    /// <summary>
    /// Initialize rigidbody
    /// </summary>
    private void Start() {
        life = maxLife; 
        playerRig = playerGam.GetComponent<Rigidbody>();
    }

    private void Update() {
        GetInputs();
        actualFireRate += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && actualFireRate >= GetFireRate(GameManager.Instance.ActualStat.FireRateUpgradeNmb + GameManager.Instance.ShipUIData.FireRateUpgradeNmb)) {
            ShootBullet();
        }
    }

    /// <summary>
    /// Move the rigidbody
    /// </summary>
    private void FixedUpdate() {
       if(moveDirectionRaw != Vector3.zero) playerRig.velocity = moveDirection * moveSpeed;
    }

    #region Methods
    /// <summary>
    /// Get the inputs for the movement
    /// </summary>
    private void GetInputs() {
        //Set MoveDirection based on the inputs
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        moveDirectionRaw = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }
    
    /// <summary>
    /// Shoot the bullet
    /// </summary>
    private  void ShootBullet() {
        actualFireRate = 0;
        WeaponUiData weapon = GameManager.Instance.ActualStat;
        ShipUIData ship = GameManager.Instance.ShipUIData;
        actualWeapon.ShootBullet(playerGam, shootPos, weapon.DamageUpgradeNmb + ship.DamageUpgradeNmb,
            weapon.BulletSizeUpgreadeNmb + ship.BulletSizeUpgreadeNmb,
            weapon.BulletSpeedUpgradeNmb + ship.BulletSpeedUpgradeNmb);
        GameManager.Instance.UseResourcesFromShoot();
    }

    /// <summary>
    /// Change the weapon of the player
    /// </summary>
    /// <param name="weapon"></param>
    public void ChangeActualWeapon(BaseWeaponSO weapon) {
        actualWeapon = weapon;
    }

    /// <summary>
    /// Get the firerate of the actual weapon
    /// </summary>
    /// <param name="FireRateUpgradeNmb"></param>
    /// <returns></returns>
    private float GetFireRate(int FireRateUpgradeNmb) {
        return actualWeapon.FireRate - (actualWeapon.FireRate * ((FireRateUpgradeNmb * 10f) / 100f));
    }
    
    #endregion Methods
    
    #region Life

    /// <summary>
    /// When the player take damage
    /// </summary>
    /// <param name="damagePerBullet"></param>
    public void TakeDamage(int damagePerBullet) {
        life -= damagePerBullet;
    }
    #endregion Life

#if UNITY_EDITOR
    /// <summary>
    /// Draw data in the scene
    /// </summary>
    private void OnDrawGizmos() {
        if (playerGam == null || actualWeapon == null) return;
        actualWeapon.DrawCustomGizmos(playerGam.transform, shootPos);
    }
#endif
}
