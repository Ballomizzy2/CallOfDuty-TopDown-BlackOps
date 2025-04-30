using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GrenadeUIController : MonoBehaviour
{
    public List<Image> grenadeIcons = new List<Image>();
    private WeaponManager weaponManager;

    void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>();
    }

    void Update()
    {
        UpdateGrenadeIcons();
    }

    private void UpdateGrenadeIcons()
    {
        if (weaponManager == null) return;

        for (int i = 0; i < grenadeIcons.Count; i++)
        {
            if (i < weaponManager.currentGrenades)
            {
                grenadeIcons[i].enabled = true;
            }
            else
            {
                grenadeIcons[i].enabled = false;
            }
        }
    }
}
