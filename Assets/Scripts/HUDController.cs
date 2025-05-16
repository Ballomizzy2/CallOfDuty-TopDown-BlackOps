using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance { get; private set; }
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;
    public WeaponManager weaponManager;
    [SerializeField] private Transform perkContainer;
    [SerializeField] private Transform perkIconTemplate;

    private void Awake()
    {
        Instance = this;
        perkIconTemplate.gameObject.SetActive(false);
    }
    void Update()
    {
        UpdateAmmoDisplay();
    }

    void UpdateAmmoDisplay()
    {
        Gun currentGun = weaponManager.GetCurrentWeapon();

        if (currentGun != null)
        {
            int current = currentGun.GetCurrentAmmo();
            int reserve = currentGun.GetReserveAmmo();

            ammoText.text = $"{current} / {reserve}";
        }
        else
        {
            ammoText.text = "- / -";
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"{score}";
    }
    
    public void SetPerkIcons(List<PerkSodasSO> perks)
    {
        foreach(Transform child in perkContainer)
        {
            if (child == perkIconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (PerkSodasSO perkSodasSO in perks)
        {
            Transform perkIconTransform = Instantiate(perkIconTemplate, perkContainer);
            perkIconTransform.gameObject.SetActive(true);
            perkIconTransform.GetComponent<Image>().sprite = perkSodasSO.icon;
        }
    }
}
