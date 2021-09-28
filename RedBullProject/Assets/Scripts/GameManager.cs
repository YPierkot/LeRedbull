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

    [SerializeField] private GameObject StatPanel = null;
    [SerializeField] private WeaponUiData actualStat = null;
    
    private void Start() {
        ChangeContractState(true);
    }

    private void Update() {
        if (Input.GetKey(KeyCode.L)) {
            AddEnnemyKillToContract();
        }
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
    public bool CanStartContract() { return !hasStartContract; }

    /// <summary>
    /// Change the contract state for all the gameObject
    /// </summary>
    /// <param name="activ"></param>
    private void ChangeContractState(bool activ) {
        foreach (WeaponUiData contract in contractGamList) {
            if (!contract.IsActivAtStart) {
                contract.gameObject.GetComponent<Animator>().SetBool("Activ", activ);
            }
            contract.ContractButton.enabled = activ;
        }
    }
    
    #endregion Contract
    
    #region PanelStat

    public void ChangePanelStatState(WeaponUiData data) {
        if(data != actualStat) StatPanel.SetActive(!StatPanel.activeSelf);
        UpdtaStatPanel(data);
    }

    private void UpdtaStatPanel(WeaponUiData weaponUiData) {
        
    }
    #endregion PanelStat
}
    

