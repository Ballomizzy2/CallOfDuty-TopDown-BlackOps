using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;
    public WeaponManager weaponManager;

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
        scoreText.text = $"Score: {score}";
    }
}
