using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUiData : MonoBehaviour {
    #region Variables
    [SerializeField] private bool isActivAtStart = false;
    public bool IsActivAtStart => isActivAtStart;
    
    [Header("Weapon Data")] 
    [SerializeField] private Sprite lockSprite = null;
    [SerializeField] private Sprite objectSprite = null;
    [SerializeField] private Image iconImage = null;
    [SerializeField] private Image thisImage = null;

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

    [Header("Weapon Stat Upgrade")] 
    [SerializeField] private int damageUpgradeNmb = 0;
    [SerializeField] private int fireRateUpgradeNmb = 0;
    [SerializeField] private int bulletSpeedUpgradeNmb = 0;
    [SerializeField] private int bulletSizeUpgreadeNmb = 0;
    #endregion Variables
    
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
}
