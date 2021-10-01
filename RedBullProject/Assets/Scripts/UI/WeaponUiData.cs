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
    [SerializeField] private Animator effectAnim = null;
    [SerializeField] private Image thisImage = null;
    public Animator IconAnim => iconAnim;
    public Animator EffectAnim => effectAnim;
    
    [Header("Button data")] 
    [SerializeField] private Button contractButton = null;
    public Button ContractButton => contractButton;
    [SerializeField] private Button openStatButton = null;
    public Button OpenStatButton => openStatButton;

    [Header("Contract Data")] 
    [SerializeField] private Animator buttonAnim = null;
    public Animator ButtonAnim => buttonAnim;
    [SerializeField] private int actualEnnemyKill = 0;
    [SerializeField] private int contractEnnemyToKill = 0;
    [SerializeField] private Image contractSlider = null;
    [SerializeField] private Color activContractColor = new Color();
    [SerializeField] private Color notActivContractColor = new Color();
    [SerializeField] private Color activWeaponColor = new Color();
    public float Length => (float) actualEnnemyKill / contractEnnemyToKill;
    
    [Header("Weapon Stat Upgrade")] [SerializeField]
    private GameObject openStatButtonPanel = null;
    [SerializeField] private GameObject statPanel = null;
    public GameObject StatPanel => statPanel;
    
    [SerializeField] private int damageUpgradeNmb = 0;
    [SerializeField] private int fireRateUpgradeNmb = 0;
    [SerializeField] private int bulletSpeedUpgradeNmb = 0;
    [SerializeField] private int bulletSizeUpgreadeNmb = 0;
    public int DamageUpgradeNmb => damageUpgradeNmb;
    public int FireRateUpgradeNmb => fireRateUpgradeNmb;
    public int BulletSpeedUpgradeNmb => bulletSpeedUpgradeNmb;
    public int BulletSizeUpgreadeNmb => bulletSizeUpgreadeNmb;

    [SerializeField] private GameObject ressourceGam = null;
    [SerializeField] private List<Ressource> ressourceList = new List<Ressource>();
    [SerializeField] private List<Transform> upgradeRessContainer = new List<Transform>();
    [SerializeField] private List<Button> addRessourceBtn = new List<Button>();
    [SerializeField] private List<TextMeshProUGUI> upgradeText = new List<TextMeshProUGUI>();
    public List<Ressource> RessourceList => ressourceList;
    
    [SerializeField] private Animator cameraAnim;
    #endregion Variables

    private void Start() {
        StatPanel.SetActive(false);
        upgradeText[0].text = "+" + damageUpgradeNmb * 10 + "%";
        upgradeText[1].text = "+" + fireRateUpgradeNmb * 10 + "%";
        upgradeText[2].text = "+" + bulletSpeedUpgradeNmb * 10 + "%";
        upgradeText[3].text = "+" + bulletSizeUpgreadeNmb * 10 + "%";
        openStatButtonPanel.SetActive(isActivAtStart);
    }
    
#if UNITY_EDITOR    
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
#endif
    
    #region Contract
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
            else {
                GameManager.Instance.AddComplexResource(1);
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
    public void ChangePanelStatState() {
        bool wasOpen = false; 
        foreach (WeaponUiData weapon in GameManager.Instance.ContractGamList) {
            if (weapon.statPanel.activeSelf && weapon.statPanel != statPanel) {
                weapon.ClosePanel();
                wasOpen = true;
            }
        }

        if (GameManager.Instance.ShipUIData.StatPanel.activeSelf) {
            GameManager.Instance.ShipUIData.ClosePanel();
            wasOpen = true;
        }
        
        switch (wasOpen) {
            case true when statPanel.activeSelf:
            case false when statPanel.activeSelf:
                cameraAnim.Play("MoveCameraForGameplay");
                ClosePanel();
                break;
            case true when !statPanel.activeSelf:
                OpenPanel();
                break;
            case false when !statPanel.activeSelf:
                cameraAnim.Play("MoveCameraForUI");
                OpenPanel();
                break;
        }
    }

    public void ClosePanel() {
        StatPanel.SetActive(false);
        openStatButton.gameObject.transform.localRotation = Quaternion.Euler(0, -180, 180);
    }

    private void OpenPanel() {
        StatPanel.SetActive(true);
        openStatButton.gameObject.transform.localRotation = Quaternion.Euler(0, -180, 0);
    }
    #endregion Contract

    #region Resource
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

        UpdateButtonRessource(GameManager.Instance.BasicRessourceNumber);
        
        GameObject ress = Instantiate(ressourceGam, upgradeRessContainer[upgradeID]);
        ress.GetComponent<Ressource>().Init(this, upgradeID);
        upgradeText[upgradeID].text = "+" + value*10 + "%";
        ressourceList.Add(ress.GetComponent<Ressource>());
        GameManager.Instance.UseRessource(1, true);
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

        UpdateButtonRessource(GameManager.Instance.BasicRessourceNumber);
        upgradeText[upgradeID].text = "+" + value*10 + "%";

        Destroy(ress.gameObject);
    }

    /// <summary>
    /// Update the button state to use Ressource
    /// </summary>
    /// <param name="ressourceNumber"></param>
    public void UpdateButtonRessource(int ressourceNumber) {
        if (damageUpgradeNmb >= 10 || ressourceNumber == 0) addRessourceBtn[0].interactable = false;
        else if(damageUpgradeNmb < 10 && ressourceNumber > 0) addRessourceBtn[0].interactable = true;
        
        if (fireRateUpgradeNmb >= 10 || ressourceNumber == 0) addRessourceBtn[1].interactable = false;
        else if(fireRateUpgradeNmb < 10 && ressourceNumber > 0) addRessourceBtn[1].interactable = true;
        
        if (bulletSpeedUpgradeNmb >= 10 || ressourceNumber == 0) addRessourceBtn[2].interactable = false;
        else if(bulletSpeedUpgradeNmb < 10 && ressourceNumber > 0) addRessourceBtn[2].interactable = true;
        
        if (bulletSizeUpgreadeNmb >= 10 || ressourceNumber == 0) addRessourceBtn[3].interactable = false;
        else if(bulletSizeUpgreadeNmb < 10 && ressourceNumber > 0) addRessourceBtn[3].interactable = true;
    }
    #endregion Resource

    /// <summary>
    /// Change the color of the weapon
    /// </summary>
    /// <param name="isUse"></param>
    public void ChangeWeaponColor(bool isUse)
    {
        thisImage.color = isUse switch {
            true => activWeaponColor,
            false => activContractColor
        };
    }
}