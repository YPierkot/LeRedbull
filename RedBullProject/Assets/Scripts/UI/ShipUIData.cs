using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipUIData : MonoBehaviour
{
#region Variables
    [Header("Weapon Stat Upgrade")]
    [SerializeField] private GameObject statPanel = null;
    public GameObject StatPanel => statPanel;
    
    [Space]
    [SerializeField] private int damageUpgradeNmb = 0;
    [SerializeField] private int fireRateUpgradeNmb = 0;
    [SerializeField] private int bulletSpeedUpgradeNmb = 0;
    [SerializeField] private int bulletSizeUpgreadeNmb = 0;
    [SerializeField] private int lifeUpgradeNmb = 0;
    [SerializeField] private int moveSpeedUpgradeNmb = 0;
    public int DamageUpgradeNmb => damageUpgradeNmb;
    public int FireRateUpgradeNmb => fireRateUpgradeNmb;
    public int BulletSpeedUpgradeNmb => bulletSpeedUpgradeNmb;
    public int BulletSizeUpgreadeNmb => bulletSizeUpgreadeNmb;
    public int LifeUpgradeNmb => lifeUpgradeNmb;
    public int MoveSpeedUpgradeNmb => moveSpeedUpgradeNmb;

    [Space]
    [SerializeField] private GameObject ressourceGam = null;
    [SerializeField] private GameObject openStatButton = null;
    [SerializeField] private List<Transform> upgradeRessContainer = new List<Transform>();
    [SerializeField] private List<Button> addRessourceBtn = new List<Button>();
    [SerializeField] private List<TextMeshProUGUI> upgradeText = new List<TextMeshProUGUI>();
    [SerializeField] private Animator cameraAnim;
    #endregion Variables

    private void Start() {
        StatPanel.SetActive(false);
        upgradeText[0].text = "+" + damageUpgradeNmb * 10 + "%";
        upgradeText[1].text = "+" + fireRateUpgradeNmb * 10 + "%";
        upgradeText[2].text = "+" + bulletSpeedUpgradeNmb * 10 + "%";
        upgradeText[3].text = "+" + bulletSizeUpgreadeNmb * 10 + "%";
        upgradeText[4].text = "+" + lifeUpgradeNmb * 10 + "%";
        upgradeText[5].text = "+" + moveSpeedUpgradeNmb * 10 + "%";
    }
    
    /// <summary>
    /// Open or Close the upgradePanel
    /// </summary>
    public void ChangePanelStatState() {
        bool wasOpen = false;
        foreach (WeaponUiData weapon in GameManager.Instance.ContractGamList) {
            if (weapon.StatPanel.activeSelf) {
                weapon.ClosePanel();
                wasOpen = true;
            }
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
            case 4:
                if (lifeUpgradeNmb >= 10) return;
                lifeUpgradeNmb++;
                value = lifeUpgradeNmb;
                break;
            case 5:
                if (moveSpeedUpgradeNmb >= 10) return;
                moveSpeedUpgradeNmb++;
                value = moveSpeedUpgradeNmb;
                break;
        }
        
        GameObject ress = Instantiate(ressourceGam, upgradeRessContainer[upgradeID]);
        upgradeText[upgradeID].text = "+" + value*10 + "%";
        GameManager.Instance.UseRessource(1, false);
        
        UpdateButtonRessource(GameManager.Instance.ComplexRessourceNumber);
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
        
        if (lifeUpgradeNmb >= 10 || ressourceNumber == 0) addRessourceBtn[4].interactable = false;
        else if(lifeUpgradeNmb < 10 && ressourceNumber > 0) addRessourceBtn[4].interactable = true;
        
        if (moveSpeedUpgradeNmb >= 10 || ressourceNumber == 0) addRessourceBtn[5].interactable = false;
        else if(moveSpeedUpgradeNmb < 10 && ressourceNumber > 0) addRessourceBtn[5].interactable = true;
    }
    #endregion Resource
}

