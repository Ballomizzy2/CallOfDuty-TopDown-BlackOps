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

        // ?? Full-Auto
        if (gunData.isAutomatic)
        {
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                if (currentAmmo > 0) Fire();
                else StartCoroutine(Reload());
            }
        }
        // ?? Burst Fire
        else if (gunData.isBurst)
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                if (currentAmmo > 0) StartCoroutine(BurstFire());
                else StartCoroutine(Reload());
            }
        }
        // ?? Semi-Auto
        else
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                if (currentAmmo > 0) Fire();
                else StartCoroutine(Reload());
            }
        }

        // ?? Manual Reload
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

        if (gunData.shootSound)
        {
            AudioSource tempAudio = gameObject.AddComponent<AudioSource>();
            tempAudio.clip = gunData.shootSound;
            tempAudio.volume = 0.7f;
            tempAudio.pitch = Random.Range(0.95f, 1.05f); // optional: slight variation
            tempAudio.spatialBlend = 0f; // 0 = 2D, 1 = 3D
            tempAudio.Play();
            Destroy(tempAudio, gunData.shootSound.length);
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

    private IEnumerator BurstFire()
    {
        nextFireTime = Time.time + gunData.fireDelay; // Prevents starting a new burst too soon

        for (int i = 0; i < gunData.burstCount; i++)
        {
            if (currentAmmo > 0)
            {
                Fire();
                yield return new WaitForSeconds(gunData.burstDelay);
            }
            else
            {
                StartCoroutine(Reload());
                yield break;
            }
        }
    }
}
