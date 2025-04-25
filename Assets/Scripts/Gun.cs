using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public GunData gunData; // Assign in Inspector
    [SerializeField] private Transform gunMuzzle;
    [SerializeField] private GameObject hitEffectPrefab;

    private int currentAmmo;
    private int reserveAmmo;
    public int GetCurrentAmmo() => currentAmmo;
    public int GetReserveAmmo() => reserveAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;
    private AudioSource audioSource;

    public enum WeaponSlot { Primary, Secondary }
    public WeaponSlot slot = WeaponSlot.Primary;

    private void Start()
    {
        currentAmmo = gunData.magazineSize;
        reserveAmmo = gunData.reserveAmmo;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isReloading) return;

        if (gunData.isAutomatic)
        {
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                if (currentAmmo > 0) Fire();
                else StartCoroutine(Reload());
            }
        }
        else if (gunData.isBurst)
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                if (currentAmmo > 0) StartCoroutine(BurstFire());
                else StartCoroutine(Reload());
            }
        }
        else // Semi-auto
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                if (currentAmmo > 0) Fire();
                else StartCoroutine(Reload());
            }
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

        if (gunData.useRaycast)
        {
            RaycastShoot();
        }
        else if (gunData.bulletPrefab)
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
            tempAudio.volume = 1f;
            tempAudio.pitch = Random.Range(0.95f, 1.05f);
            tempAudio.spatialBlend = 0f;
            tempAudio.Play();
            Destroy(tempAudio, gunData.shootSound.length);
        }
    }

    private void RaycastShoot()
    {
        Ray ray = new Ray(gunMuzzle.position, gunMuzzle.forward);

        // Draw a visible line in the Scene view
        Debug.DrawRay(ray.origin, ray.direction * gunData.raycastRange, Color.red, 0.5f);

        if (Physics.Raycast(ray, out RaycastHit hit, gunData.raycastRange))
        {
            Debug.Log("Hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Zombie"))
            {
                Destroy(hit.collider.gameObject); // Replace with proper damage system later
            }

            // Optional: Visual impact point
            if (hitEffectPrefab)
            {
                Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }



    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        if (gunData.reloadSound)
        {
            AudioSource tempAudio = gameObject.AddComponent<AudioSource>();
            tempAudio.clip = gunData.reloadSound;
            tempAudio.volume = 1f;
            tempAudio.pitch = Random.Range(0.95f, 1.05f);
            tempAudio.spatialBlend = 0f;
            tempAudio.Play();
            Destroy(tempAudio, gunData.reloadSound.length);
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
        nextFireTime = Time.time + gunData.fireDelay;

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

    // For weapon switching
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public bool IsReloading => isReloading;
}
