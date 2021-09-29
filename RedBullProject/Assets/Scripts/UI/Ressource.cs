using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ressource : MonoBehaviour
{
    [SerializeField] private int durabilityMax = 0;
    [SerializeField] private int actualDurability = 0;
    [SerializeField] private int upgradeID = 0;
    [SerializeField] private Image durabilitySlider = null;
    [SerializeField] private WeaponUiData weaponBase = null;

    /// <summary>
    /// Initialize this ressource
    /// </summary>
    /// <param name="weaponUiData"></param>
    /// <param name="id"></param>
    public void Init(WeaponUiData weaponUiData, int id) {
        weaponBase = weaponUiData;
        upgradeID = id;
        actualDurability = durabilityMax;
        ReduceDurability(0);
    }

    public void ReduceDurability(int value) {
        actualDurability -= value;
        durabilitySlider.fillAmount = (float) actualDurability / durabilityMax;
        if (actualDurability <= 0) DeleteRessource();
    }

    /// <summary>
    /// When the Ressource needs to be destroy
    /// </summary>
    void DeleteRessource() {
        weaponBase.RessourceDestroy(upgradeID, this);
    }
}
