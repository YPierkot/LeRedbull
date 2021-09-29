using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Instance

    private static GameManager instance = null;
    public static GameManager Instance => instance;

    /// <summary>
    /// Initialized the instance
    /// </summary>
    private void Awake() {
        instance = instance == null ? this : instance;
    }

    #endregion Instance

    [SerializeField] private bool hasStartContract = false;
    [SerializeField] private WeaponUiData contractStart = null;
    [SerializeField] private List<WeaponUiData> contractGamList = new List<WeaponUiData>();

    [SerializeField] private PlayerManager playerData = null;
    public List<WeaponUiData> ContractGamList => contractGamList;

    [SerializeField] private WeaponUiData actualStat = null;
    
    private void Start() {
        ChangeContractState(true);
        ChangeWeapon(0);
    }

    private void Update() {
        if (Input.GetKey(KeyCode.L)) {
            AddEnnemyKillToContract();
        }

        if (Input.GetKey(KeyCode.M)) {
            foreach (WeaponUiData data in contractGamList) {
                for (int i = 0; i < data.RessourceList.Count; i++) {
                    Ressource ress = data.RessourceList[i];
                    ress.GetComponent<Ressource>().ReduceDurability(1);
                }
            }
        }

        ChangeWeapon();
    }

    /// <summary>
    /// change the weapon
    /// </summary>
    private void ChangeWeapon(int id = -1) {
        if (id != -1) {
            actualStat = contractGamList[id];
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1)&& contractGamList[0].IsActivAtStart) actualStat = contractGamList[0];
        else if (Input.GetKeyDown(KeyCode.Alpha2) && contractGamList[1].IsActivAtStart) actualStat = contractGamList[1];
        else if (Input.GetKeyDown(KeyCode.Alpha3)&& contractGamList[2].IsActivAtStart) actualStat = contractGamList[2];
        else if (Input.GetKeyDown(KeyCode.Alpha4)&& contractGamList[3].IsActivAtStart) actualStat = contractGamList[3];
        else if (Input.GetKeyDown(KeyCode.Alpha5)&& contractGamList[4].IsActivAtStart) actualStat = contractGamList[4];
        else if (Input.GetKeyDown(KeyCode.Alpha6)&& contractGamList[5].IsActivAtStart) actualStat = contractGamList[5];

        playerData.ChangeActualWeapon(actualStat.Weapon);
    }
    
    #region Contract
    /// <summary>
    /// When an Ennemy was killed called this function 
    /// </summary>
    public void AddEnnemyKillToContract() {
        if (!hasStartContract) return;
        contractStart.AddKilledEnnemy();
    }

    /// <summary>
    /// Start a contract
    /// </summary>
    /// <param name="contract"></param>
    public void StartContract(WeaponUiData contract) {
        if (contract != null && CanStartContract()) {
            contractStart = contract;
            hasStartContract = true;
            ChangeContractState(false);
        }
    }

    /// <summary>
    /// When the contract is finished
    /// </summary>
    public void StopContract() {
        hasStartContract = false;
        contractStart = null;
        ChangeContractState(true);
    }
    
    /// <summary>
    /// Can start a new contract
    /// </summary>
    /// <returns></returns>
    private bool CanStartContract() { return !hasStartContract; }

    /// <summary>
    /// Change the contract state for all the gameObject
    /// </summary>
    /// <param name="activ"></param>
    private void ChangeContractState(bool activ) {
        foreach (WeaponUiData contract in contractGamList) {
            if (!contract.IsActivAtStart) {
                contract.IconAnim.SetBool("Activ", activ);
            }
            contract.ContractButton.enabled = activ;
        }
    }
    
    #endregion Contract
    
}