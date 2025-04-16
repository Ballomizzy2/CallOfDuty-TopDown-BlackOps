using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public GunData gunData; // Drag the ScriptableObject into this field
    [SerializeField] private Transform gunMuzzle;

    private int currentAmmo;
    private int reserveAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = gunData.magazineSize;
        reserveAmmo = gunData.reserveAmmo;
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetMouseButton(1) && Time.time >= nextFireTime) // Right-click = MouseButton(1)
        {
            if (currentAmmo > 0) Fire();
            else StartCoroutine(Reload());
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < gunData.magazineSize && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
        }
    }


    private void Fire()
    {
        nextFireTime = Time.time + gunData.fireDelay;
        currentAmmo--;

        Debug.Log("Fired " + gunData.gunName + " | Ammo: " + currentAmmo + "/" + reserveAmmo);

        if (gunData.bulletPrefab)
        {
            GameObject bullet = Instantiate(gunData.bulletPrefab, gunMuzzle.position, gunMuzzle.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = gunData.damage;
            }
        }
    }


    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(gunData.reloadTime);

        int ammoNeeded = gunData.magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);
        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        isReloading = false;
    }
}
