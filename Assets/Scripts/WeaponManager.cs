using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Gun[] weapons; // Should contain 2 weapons in the array
    private int currentWeaponIndex = 0;

    void Start()
    {
        EquipWeapon(currentWeaponIndex);
    }

    void Update()
    {
        // Switch with Q key
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!weapons[currentWeaponIndex].IsReloading)
            {
                SwitchWeapon();
            }
        }

        // Optionally allow switching via number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TryEquipWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TryEquipWeapon(1);
        }
    }

    void TryEquipWeapon(int index)
    {
        if (index != currentWeaponIndex && index >= 0 && index < weapons.Length)
        {
            if (!weapons[currentWeaponIndex].IsReloading)
            {
                EquipWeapon(index);
            }
        }
    }

    void SwitchWeapon()
    {
        int nextIndex = (currentWeaponIndex + 1) % weapons.Length;
        EquipWeapon(nextIndex);
    }

    void EquipWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        currentWeaponIndex = index;
        Debug.Log("Switched to weapon: " + weapons[currentWeaponIndex].gunData.gunName);
    }

    public Gun GetCurrentWeapon()
    {
        return weapons[currentWeaponIndex];
    }
}

