using System;
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
    private bool isCooking = false;
    private float cookTimer = 0f;
    [SerializeField] private float grenadeFuseTime = 3f;
    public event EventHandler<WeaponSwap> OnWeaponSwap;

    public class WeaponSwap : EventArgs
    {
        public Gun currentWeapon;
    }

    public Gun[] weapons; // Should contain 2 weapons in the array
    private int currentWeaponIndex = 0;

    void Start()
    {
        EquipWeapon(currentWeaponIndex);
        currentGrenades = maxGrenades;
    }

    void Update()
    {
        // Start cooking when G is pressed
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (currentGrenades > 0 && !isCooking)
            {
                StartCookingGrenade();
            }
        }

        // Throw grenade when G is released
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (isCooking)
            {
                ThrowCookedGrenade();
            }
        }

        if (isCooking)
        {
            cookTimer += Time.deltaTime;

            if (cookTimer >= grenadeFuseTime)
            {
                Debug.Log("BOOM! Grenade cooked too long.");

                isCooking = false;
                currentGrenades--;

                // Explode at player's position
                Instantiate(grenadePrefab.GetComponent<Grenade>().explosionPrefab, transform.position, Quaternion.identity);
            }
        }

        // Switch weapons
        if (Input.GetKeyDown(KeyCode.Q) && !weapons[currentWeaponIndex].IsReloading)
        {
            SwitchWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) TryEquipWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TryEquipWeapon(1);
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
        OnWeaponSwap?.Invoke(this, new WeaponSwap { currentWeapon = weapons[nextIndex] });
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

    public void ReplaceCurrentWeapon(GunData newGunData)
    {
        foreach (Gun gun in weapons)
        {
            if (gun.gunData == newGunData)
            {
                Debug.Log("Already have this weapon! Cannot buy again.");
                return;
            }
        }

        Gun currentGun = weapons[currentWeaponIndex];
        if (currentGun != null)
        {
            currentGun.gunData = newGunData;
            currentGun.ReinitializeWeapon();
        }
    }

    private void StartCookingGrenade()
    {
        isCooking = true;
        cookTimer = 0f;
        Debug.Log("Started cooking grenade...");
        // Optional: Play pin sound here if not tied to grenade object
    }

    private void ThrowCookedGrenade()
    {
        isCooking = false;
        currentGrenades--;

        float timeLeft = grenadeFuseTime - cookTimer;
        timeLeft = Mathf.Max(0.1f, timeLeft);

        GameObject grenade = Instantiate(grenadePrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 throwDirection = (grenadeSpawnPoint.position - (transform.position + Vector3.up * 1.5f)).normalized;
            Vector3 finalThrowDirection = (throwDirection + Vector3.up * 0.5f).normalized;
            rb.AddForce(finalThrowDirection * throwForce, ForceMode.Impulse);
        }

        Grenade grenadeScript = grenade.GetComponent<Grenade>();
        if (grenadeScript != null)
        {
            grenadeScript.fuseTime = timeLeft;
            grenadeScript.PlayPinPullSound(); // Optional
            grenadeScript.BeginFuse();
        }

        Debug.Log($"Threw grenade with {timeLeft:F2}s fuse remaining");
    }
}
