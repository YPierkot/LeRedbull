using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUiData : MonoBehaviour {
    #region Variables
    [SerializeField] private bool isActivAtStart = false;
    public bool IsActivAtStart => isActivAtStart;

    [SerializeField] private BaseWeaponSO weapon = null;
    public BaseWeaponSO Weapon => weapon;
    
    [Header("Weapon Data")] 
    [SerializeField] private Sprite lockSprite = null;
    [SerializeField] private Sprite objectSprite = null;
    [SerializeField] private Image iconImage = null;
    [SerializeField] private Animator iconAnim = null;
    [SerializeField] private Image thisImage = null;
    public Animator IconAnim => iconAnim;
    
    [Header("Button data")] 
    [SerializeField] private Button contractButton = null;
    public Button ContractButton => contractButton;
    [SerializeField] private Button openStatButton = null;
    public Button OpenStatButton => openStatButton;

    [Header("Contract Data")] 
    [SerializeField] private int actualEnnemyKill = 0;
    [SerializeField] private int contractEnnemyToKill = 0;
    [SerializeField] private Image contractSlider = null;
    [SerializeField] private Color activContractColor = new Color();
    [SerializeField] private Color notActivContractColor = new Color();

    [Header("Weapon Stat Upgrade")] [SerializeField]
    private GameObject openStatButtonPanel = null;
    [SerializeField] private GameObject statPanel = null;
    public GameObject StatPanel => statPanel;
    
    [SerializeField] private int damageUpgradeNmb = 0;
    [SerializeField] private int fireRateUpgradeNmb = 0;
    [SerializeField] private int bulletSpeedUpgradeNmb = 0;
    [SerializeField] private int bulletSizeUpgreadeNmb = 0;
    public int DamageUpgradeNmb => damageUpgradeNmb;

    [SerializeField] private GameObject ressourceGam = null;
    [SerializeField] private List<Ressource> ressourceList = new List<Ressource>();
    [SerializeField] private List<Transform> upgradeRessContainer = new List<Transform>();
    [SerializeField] private List<Button> addRessourceBtn = new List<Button>();
    [SerializeField] private List<TextMeshProUGUI> upgradeText = new List<TextMeshProUGUI>();
    public List<Ressource> RessourceList => ressourceList;
    
    #endregion Variables

    private void Start() {
        StatPanel.SetActive(false);
        upgradeText[0].text = "+" + damageUpgradeNmb + "%";
        upgradeText[1].text = "+" + fireRateUpgradeNmb + "%";
        upgradeText[2].text = "+" + bulletSpeedUpgradeNmb + "%";
        upgradeText[3].text = "+" + bulletSizeUpgreadeNmb + "%";
        openStatButtonPanel.SetActive(isActivAtStart);
    }

    /// <summary>
    /// When variable change
    /// </summary>
    private void OnValidate() {
        thisImage.color = isActivAtStart ? activContractColor : notActivContractColor;
        if (lockSprite != null && objectSprite != null) {
            iconImage.sprite = !isActivAtStart ? lockSprite : objectSprite;
        }
        UpdateSliderVisual();
    }

    /// <summary>
    /// Update the slider visual
    /// </summary>
    private void UpdateSliderVisual() {
        if (contractSlider == null) return;
        contractSlider.fillAmount = (float) actualEnnemyKill / contractEnnemyToKill;
    }

    /// <summary>
    /// Check the number of the contract
    /// </summary>
    private void CheckSliderValue() {
        if (actualEnnemyKill >= contractEnnemyToKill) {
            if (!isActivAtStart) {
                actualEnnemyKill = 0;
                iconImage.sprite = objectSprite;
            }
            UpdateSliderVisual();
            
            isActivAtStart = true;
            thisImage.color = activContractColor;
            openStatButtonPanel.SetActive(true);
            GameManager.Instance.StopContract();
        }
    }
    
    /// <summary>
    /// Add one to the ennemyKill
    /// </summary>
    public void AddKilledEnnemy() {
        actualEnnemyKill++;
        UpdateSliderVisual();
        CheckSliderValue();
    }
    
    /// <summary>
    /// Open or Close the upgradePanel
    /// </summary>
    public void ChangePanelStatState(bool forceOpen) {
        if (!forceOpen) {
            foreach (WeaponUiData weapon in GameManager.Instance.ContractGamList) {
                if (weapon.statPanel.activeSelf && weapon.statPanel != statPanel) weapon.ChangePanelStatState(true);
            }
        }

        StatPanel.SetActive(!StatPanel.activeSelf);
    }

    /// <summary>
    /// Create a Ressource in the right Panel
    /// </summary>
    /// <param name="upgradeID"></param>
    public void CreateRessource(int upgradeID) {
        int value = 0;
        switch (upgradeID) {
            case 0:
                if (damageUpgradeNmb >= 10) return;
                damageUpgradeNmb++;
                value = damageUpgradeNmb;
                break;
            case 1:
                if (fireRateUpgradeNmb >= 10) return;
                fireRateUpgradeNmb++;
                value = fireRateUpgradeNmb;
                break;
            case 2:
                if (bulletSpeedUpgradeNmb >= 10) return;
                bulletSpeedUpgradeNmb++;
                value = bulletSpeedUpgradeNmb;
                break;
            case 3:
                if (bulletSizeUpgreadeNmb >= 10) return;
                bulletSizeUpgreadeNmb++;
                value = bulletSizeUpgreadeNmb;
                break;
        }

        if (value >= 10) addRessourceBtn[upgradeID].interactable = false;
        
        GameObject ress = Instantiate(ressourceGam, upgradeRessContainer[upgradeID]);
        ress.GetComponent<Ressource>().Init(this, upgradeID);
        upgradeText[upgradeID].text = "+" + value + "%";
        ressourceList.Add(ress.GetComponent<Ressource>());
    }

    /// <summary>
    /// Destroy a Ressource
    /// </summary>
    /// <param name="upgradeID"></param>
    /// <param name="ress"></param>
    public void RessourceDestroy(int upgradeID, Ressource ress) {
        ressourceList.Remove(ress);
        int value = 0;

        switch (upgradeID) {
            case 0:
                damageUpgradeNmb--;
                value = damageUpgradeNmb;
                break;
            case 1:
                fireRateUpgradeNmb--;
                value = fireRateUpgradeNmb;
                break;
            case 2:
                bulletSpeedUpgradeNmb--;
                value = bulletSpeedUpgradeNmb;
                break;
            case 3:
                bulletSizeUpgreadeNmb--;
                value = bulletSizeUpgreadeNmb;
                break;
        }
        if(value < 10) addRessourceBtn[upgradeID].interactable = true;
        upgradeText[upgradeID].text = "+" + value + "%";
        Destroy(ress.gameObject);
    }
}