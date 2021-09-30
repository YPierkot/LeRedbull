using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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

    [Header("Contract")]
    [SerializeField] private bool hasStartContract = false;
    [SerializeField] private WeaponUiData contractStart = null;
    [SerializeField] private List<WeaponUiData> contractGamList = new List<WeaponUiData>();
    public List<WeaponUiData> ContractGamList => contractGamList;
    [SerializeField] private ShipUIData shipData = null;
    public ShipUIData ShipUIData => shipData;
    
    [SerializeField] private PlayerManager playerData = null;
    [SerializeField] private WeaponUiData actualStat = null;
    public WeaponUiData ActualStat => actualStat;
    
    [Header("Ressources")] 
    [SerializeField] private int basicRessourceNumber = 0;
    [SerializeField] private int complexRessourceNumber = 0;
    [SerializeField] private TextMeshProUGUI basicRessourceTxt = null;
    [SerializeField] private TextMeshProUGUI complexRessourceTxt = null;
    public int BasicRessourceNumber => basicRessourceNumber;
    public int ComplexRessourceNumber => complexRessourceNumber;
    
    
    private void Start() {
        ChangeContractState(true);
        ChangeWeapon(0);
        UpdateRessourceValue();
    }

    private void Update() {
        if (Input.GetKey(KeyCode.L)) {
            AddEnnemyKillToContract();
        }

        if (Input.GetKey(KeyCode.M)) {
            UseResourcesFromShoot();
        }

        ChangeWeapon();
    }

    /// <summary>
    /// change the weapon
    /// </summary>
    private void ChangeWeapon(int id = -1) {
        if (id != -1) {
            actualStat = contractGamList[0];
            return;
        }

        if (actualStat != null) {
            actualStat.ChangeWeaponColor(false);
            actualStat.EffectAnim.SetBool("PlayAnim", false);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1)&& contractGamList[0].IsActivAtStart) actualStat = contractGamList[0];
        else if (Input.GetKeyDown(KeyCode.Alpha2) && contractGamList[1].IsActivAtStart) actualStat = contractGamList[1];
        else if (Input.GetKeyDown(KeyCode.Alpha3)&& contractGamList[2].IsActivAtStart) actualStat = contractGamList[2];
        else if (Input.GetKeyDown(KeyCode.Alpha4)&& contractGamList[3].IsActivAtStart) actualStat = contractGamList[3];
        else if (Input.GetKeyDown(KeyCode.Alpha5)&& contractGamList[4].IsActivAtStart) actualStat = contractGamList[4];
        else if (Input.GetKeyDown(KeyCode.Alpha6)&& contractGamList[5].IsActivAtStart) actualStat = contractGamList[5];

        playerData.ChangeActualWeapon(actualStat.Weapon);
        actualStat.ChangeWeaponColor(true);
        actualStat.EffectAnim.SetBool("PlayAnim", true);
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
            contractStart.ButtonAnim.SetBool("PlayAnim", true);
            hasStartContract = true;
            ChangeContractState(false);
        }
    }

    /// <summary>
    /// When the contract is finished
    /// </summary>
    public void StopContract() {
        contractStart.ButtonAnim.SetBool("PlayAnim", false);
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
            if (contract.Length < 1) {
                contract.IconAnim.SetBool("Activ", activ);
                if (contract != contractStart) contract.ContractButton.interactable = activ;
            }
            else {
                contract.ContractButton.enabled = false;
            }

        }
    }
    
    #endregion Contract
    
    #region Ressources

    /// <summary>
    /// Function which is called when the player gain a Resource
    /// </summary>
    /// <param name="value"></param>
    /// <param name="basicRessource"></param>
    public void AddBasicRessource(int value) {
        basicRessourceNumber += value;
        UpdateRessourceValue();
    }
    
    /// <summary>
    /// Add complex Resources to the player when completing a contract
    /// </summary>
    /// <param name="value"></param>
    public void AddComplexResource(int value) {
        complexRessourceNumber += value;
        UpdateRessourceValue();
    }

    /// <summary>
    /// When the player use a Resource
    /// </summary>
    /// <param name="value"></param>
    /// <param name="basicRessource"></param>
    public void UseRessource(int value, bool basicRessource) {
        if (basicRessource) basicRessourceNumber -= value;
        else complexRessourceNumber -= value;
        UpdateRessourceValue();
    }
    
    /// <summary>
    /// Update the text of the Ressources
    /// </summary>
    private void UpdateRessourceValue() {
        basicRessourceTxt.text = "Resource : " + basicRessourceNumber;
        complexRessourceTxt.text = "Complex Resource : " + complexRessourceNumber;
        
        foreach (WeaponUiData weapon in contractGamList) {
            weapon.UpdateButtonRessource(basicRessourceNumber);
        }
        shipData.UpdateButtonRessource(complexRessourceNumber);
    }
    
    /// <summary>
    /// use Resources each time the player shoot
    /// </summary>
    public void UseResourcesFromShoot() {
        foreach (WeaponUiData data in contractGamList) {
            for (int i = 0; i < data.RessourceList.Count; i++) {
                Ressource ress = data.RessourceList[i];
                ress.GetComponent<Ressource>().ReduceDurability(1);
            }
        }
    }
    
    #endregion Ressources
    
}