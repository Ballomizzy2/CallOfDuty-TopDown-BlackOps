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
    [Header("Perk Display")]
    [SerializeField] private Transform perkContainer;
    [SerializeField] private Transform perkIconTemplate;

    [Header("Power UP Display")]
    [SerializeField] private Transform powerUpContainer;
    [SerializeField] private Transform powerUpIconTemplate;
    //[SerializeField] private PowerUpUIHandler powerUpUIHandler;
    [SerializeField] private List<Transform> powerUpList;

    private void Awake()
    {
        Instance = this;
        perkIconTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        foreach(Transform t in powerUpList)
        {
            //clear the PowerUp UI
            t.gameObject.SetActive(false);
        }
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

    public void EnablePowerUpUI(PowerUpType powerUpType)
    {
        for (int i = 0; i<powerUpList.Count; i++)
        {
            if(powerUpList[i].GetComponent<PowerUpUIHandler>().GetPowerUpType() == powerUpType)
            {
                powerUpList[i].gameObject.SetActive(true);
                break;
            }
        }
    }
    public void DisablePowerUpUI(PowerUpType powerUpType)
    {
        for (int i = 0; i < powerUpList.Count; i++)
        {
            if (powerUpList[i].GetComponent<PowerUpUIHandler>().GetPowerUpType() == powerUpType)
            {
                powerUpList[i].gameObject.SetActive(false);
                break;
            }
        }
    }

}
