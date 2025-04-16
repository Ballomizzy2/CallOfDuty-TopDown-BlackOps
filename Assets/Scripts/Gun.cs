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

    private AudioSource audioSource;

    private void Start()
    {
        currentAmmo = gunData.magazineSize;
        reserveAmmo = gunData.reserveAmmo;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime) // Left-click
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
                bulletScript.SetBulletStats(gunData.bulletSpeed, gunData.damage);
            }
        }

        if (gunData.shootSound && audioSource)
        {
            audioSource.PlayOneShot(gunData.shootSound);
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        if (gunData.reloadSound && audioSource)
        {
            audioSource.PlayOneShot(gunData.reloadSound);
        }

        yield return new WaitForSeconds(gunData.reloadTime);

        int ammoNeeded = gunData.magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);
        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        isReloading = false;
    }
}
