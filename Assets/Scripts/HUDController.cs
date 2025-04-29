using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance { get; private set; }
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;
    public WeaponManager weaponManager;

    private void Awake()
    {
        Instance = this;
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
}
