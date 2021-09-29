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
    
    //Move Data
    private int verticalMove = 0;
    private int horizontalMove = 0;
    private Vector3 moveDirection = new Vector2();
    
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
        if (Input.GetKey(KeyCode.Space) && actualFireRate >= actualWeapon.FireRate)
        {
            ShootBullet();
        }
    }

    /// <summary>
    /// Move the rigidbody
    /// </summary>
    private void FixedUpdate() {
       if(moveDirection != Vector3.zero) playerRig.velocity = moveDirection * moveSpeed;
    }

    #region Methods
    /// <summary>
    /// Get the inputs for the movement
    /// </summary>
    private void GetInputs() {
        //Input Down
        if (Input.GetKeyDown(KeyCode.DownArrow)) verticalMove -= 1;
        if (Input.GetKeyDown(KeyCode.UpArrow)) verticalMove += 1;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) horizontalMove -= 1;
        if (Input.GetKeyDown(KeyCode.RightArrow)) horizontalMove += 1;

        //Input Up
        if (Input.GetKeyUp(KeyCode.DownArrow)) verticalMove += 1;
        if (Input.GetKeyUp(KeyCode.UpArrow)) verticalMove -= 1;
        if (Input.GetKeyUp(KeyCode.LeftArrow)) horizontalMove += 1;
        if (Input.GetKeyUp(KeyCode.RightArrow)) horizontalMove -= 1;
        
        //Set MoveDirection based on the inputs
        moveDirection = new Vector3(horizontalMove, 0, verticalMove).normalized;
    }
    
    /// <summary>
    /// Shoot the bullet
    /// </summary>
    private  void ShootBullet() 
    {
        actualFireRate = 0;
        actualWeapon.ShootBullet(playerGam, shootPos, bulletContainer);
    }

    /// <summary>
    /// Change the weapon of the player
    /// </summary>
    /// <param name="weapon"></param>
    public void ChangeActualWeapon(BaseWeaponSO weapon) {
        actualWeapon = weapon;
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
