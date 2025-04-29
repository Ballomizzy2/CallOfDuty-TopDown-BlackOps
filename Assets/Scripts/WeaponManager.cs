using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Grenade Settings")]
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private Transform grenadeSpawnPoint;

    [Header("Grenade Count")]
    public int maxGrenades = 4;
    public int currentGrenades;

    public Gun[] weapons; // Should contain 2 weapons in the array
    private int currentWeaponIndex = 0;

    void Start()
    {
        EquipWeapon(currentWeaponIndex);
        currentGrenades = maxGrenades;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowGrenade();
        }

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

    private void ThrowGrenade()
    {
        if (grenadePrefab == null || grenadeSpawnPoint == null) return;

        if (currentGrenades <= 0)
        {
            Debug.Log("No grenades left!");
            return;
        }

        // Create grenade
        GameObject grenade = Instantiate(grenadePrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 throwDirection = (grenadeSpawnPoint.position - (transform.position + Vector3.up * 1.5f)).normalized;
            Vector3 finalThrowDirection = (throwDirection + Vector3.up * 0.5f).normalized;
            rb.AddForce(finalThrowDirection * throwForce, ForceMode.Impulse);
        }

        currentGrenades--; // Reduce grenade count
    }


    public void ReplaceCurrentWeapon(GunData newGunData)
    {
        // First, check if the player already has this weapon
        foreach (Gun gun in weapons)
        {
            if (gun.gunData == newGunData)
            {
                Debug.Log("Already have this weapon! Cannot buy again.");
                return; // Stop, don't replace
            }
        }

        // If not owned, replace the current weapon
        Gun currentGun = weapons[currentWeaponIndex];

        if (currentGun != null)
        {
            currentGun.gunData = newGunData;
            currentGun.ReinitializeWeapon();
        }
    }



}

