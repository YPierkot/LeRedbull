using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.LWRP;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
    #region Variables
    [Header("Player Infos")] 
    [SerializeField] private GameObject playerGam = null;
    [SerializeField] private Image lifeSlider = null;
    [SerializeField] private Transform shootPos = null;
    [SerializeField] private int life = 0;
    [SerializeField] private int maxLife = 0;
    [SerializeField] private Animator takeDamageAnim = null;
    
    [Header("Player Movement")] 
    [SerializeField] private float moveSpeed = 0;

    [Header("Player Shoot")] 
    [SerializeField] private BaseWeaponSO actualWeapon;
    
    [Header("Slow Motion")] 
    [SerializeField] private Image slowMotionSlider = null;
    [SerializeField] private Image slowMotionImg = null;
    [SerializeField] private float slowMotionValue = 0f;
    [SerializeField] private float slowMotionLength = 0f;
    [SerializeField] private float slowMotionForce = 0f;
    [SerializeField] private Color slowMotionColorWhenUsed = new Color();
    [SerializeField] private GameObject slowMotionEffect = null;
    [SerializeField] private GameObject fastMotionEffect = null;
    [SerializeField] private bool hasStartSlowMotion = false;

    #region privateVariable
    //Rigidbody
    private Rigidbody playerRig = null;
    
    private Vector3 moveDirection = new Vector3();
    private Vector3 moveDirectionRaw = new Vector3();
    
    //Shoot Data
    private float actualFireRate = 0;
    private bool hasReachEnd;
    
    #endregion PrivateVariable
    
    #endregion Variables
    
    /// <summary>
    /// Initialize rigidbody
    /// </summary>
    private void Start() {
        life = maxLife; 
        playerRig = playerGam.GetComponent<Rigidbody>();
        UpdateSlider();
    }

    private void Update() {
        GetInputs();
        actualFireRate += Time.deltaTime;
        ////Shoot
        if (Input.GetMouseButton(0) && actualFireRate >= GetFireRate(GameManager.Instance.ActualStat.FireRateUpgradeNmb + GameManager.Instance.ShipUIData.FireRateUpgradeNmb)) {
            ShootBullet();
        }

        //Slow Motion
        if (Input.GetMouseButton(1) && hasReachEnd == false) {
            UseSlowMotion(true);
            if (!hasStartSlowMotion) {
                Instantiate(slowMotionEffect);
                hasStartSlowMotion = true;
            }
        }
        else {
            UseSlowMotion(false);
        }

        if (Input.GetMouseButtonUp(1)) {
            hasStartSlowMotion = false;
            if(!hasReachEnd) Instantiate(fastMotionEffect);
        }
    }

    /// <summary>
    /// Move the rigidbody
    /// </summary>
    private void FixedUpdate() {
       if(moveDirectionRaw != Vector3.zero) playerRig.velocity = moveDirection * (moveSpeed + moveSpeed * (GameManager.Instance.ShipUIData.MoveSpeedUpgradeNmb * 10) / 100);
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
        takeDamageAnim.Play("TakeDamage");
        UpdateSlider();
    }

    /// <summary>
    /// UPdate slider value
    /// </summary>
    private void UpdateSlider() {
        lifeSlider.fillAmount = (float)life / maxLife;
    }
    #endregion Life

    #region SlowMotion

    private void UseSlowMotion(bool useSlowMotion) {
        switch (useSlowMotion) {
            case true:
                Time.timeScale = Mathf.Lerp(Time.timeScale, slowMotionValue, Time.deltaTime * slowMotionForce);
                slowMotionSlider.fillAmount = Mathf.Clamp(slowMotionSlider.fillAmount - ((Time.deltaTime / slowMotionLength) * 1 / Time.timeScale), 0 , 1);
                if (slowMotionSlider.fillAmount <= .001f) {
                    hasReachEnd = true;
                    slowMotionImg.color = slowMotionColorWhenUsed;
                    Instantiate(fastMotionEffect);
                }
                break;
            case false:
                Time.timeScale = Mathf.Lerp(Time.timeScale, 1, Time.deltaTime * slowMotionForce);
                slowMotionSlider.fillAmount = Mathf.Clamp(slowMotionSlider.fillAmount + ((Time.deltaTime / slowMotionLength) * 1 / Time.timeScale), 0 , 1);
                if (slowMotionSlider.fillAmount >= .99f) {
                    hasReachEnd = false;
                    hasStartSlowMotion = false;
                    slowMotionImg.color = Color.white;
                }
                break;
        }
    }
    
    #endregion SlowMotion
    
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
