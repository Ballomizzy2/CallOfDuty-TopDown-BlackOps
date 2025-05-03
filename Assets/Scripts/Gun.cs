using UnityEngine;
using System.Collections;
using System;

public class Gun : MonoBehaviour
{

    public GunData gunData; // Assign in Inspector
    [SerializeField] private Transform gunMuzzle;
    [SerializeField] private GameObject hitEffectPrefab;
    private const string ZOMBIE_TAG = "Zombie";
    private GameObject weaponModelInstance;
    [SerializeField] private Transform modelHolder;


    internal int currentAmmo;
    internal int reserveAmmo;
    // public int GetCurrentAmmo() => currentAmmo;
    // public int GetReserveAmmo() => reserveAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;
    private AudioSource audioSource;
    private bool isAiming = false;
    private PlayerMovement playerMovement;
    private float originalMoveSpeed;
    private bool adsSlowed = false;

    [SerializeField] private GameManager_Scores gm_score;



    private void Start()
    {
        ReinitializeWeapon();
        currentAmmo = gunData.magazineSize;
        reserveAmmo = gunData.reserveAmmo;
        audioSource = GetComponent<AudioSource>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        originalMoveSpeed = playerMovement.speed;
    }

    void Update()
    {
        isAiming = Input.GetMouseButton(1); // Right-click to aim

        HandleADSMovement();

        if ((float)reserveAmmo / gunData.reserveAmmo < 0.30f && reserveAmmo > 0 && PlayerVoicelineManager.Instance.hasreloaded && PlayerVoicelineManager.Instance.canSpeak)
        {
            PlayerVoicelineManager.Instance.PlayVoiceline(PlayerVoicelineManager.Instance.lowAmmoClips);
            //Debug.Log((float)reserveAmmo / gunData.reserveAmmo);
            PlayerVoicelineManager.Instance.hasreloaded = false;
        }

        if (reserveAmmo <= 0 && !PlayerVoicelineManager.Instance.outOfAmmoSaid && PlayerVoicelineManager.Instance.canSpeak)
        {
            PlayerVoicelineManager.Instance.PlayVoiceline(PlayerVoicelineManager.Instance.outOfAmmoClips);
            PlayerVoicelineManager.Instance.outOfAmmoSaid = true;
        }

        if (isReloading) return;
        isAiming = Input.GetMouseButton(1); // Right-click = Aim Down Sights

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
            if (gunData.isShotgun)
                ShotgunRaycastShoot();
            else
                RaycastShoot();
        }
        else if (gunData.bulletPrefab)
        {
            GameObject bullet = Instantiate(gunData.bulletPrefab, gunMuzzle.position, gunMuzzle.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetBulletStats(gunData.bulletSpeed, gunData.damage, gm_score);
            }
        }

        if (gunData.shootSound)
        {
            AudioSource tempAudio = gameObject.AddComponent<AudioSource>();
            tempAudio.clip = gunData.shootSound;
            tempAudio.volume = 1f;
            tempAudio.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
            tempAudio.spatialBlend = 0f;
            tempAudio.Play();
            Destroy(tempAudio, gunData.shootSound.length);
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
            tempAudio.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
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

        PlayerVoicelineManager.Instance.hasreloaded = true;
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

    private void RaycastShoot()
    {
        Vector3 shootDirection = gunMuzzle.forward;
        float currentSpread = isAiming ? gunData.adsSpreadAngle : gunData.hipfireSpreadAngle;

        if (currentSpread > 0f)
        {
            shootDirection = ApplySpread(shootDirection, currentSpread);
        }

        Ray ray = new Ray(gunMuzzle.position, shootDirection);
        Vector3 hitPoint = ray.origin + ray.direction * gunData.raycastRange; // default endpoint if no hit

        if (Physics.Raycast(ray, out RaycastHit hit, gunData.raycastRange))
        {
            hitPoint = hit.point;

            if (hit.collider.CompareTag("Zombie"))
            {
                hit.collider.GetComponent<Enemy>().TakeDamage(gunData.damage,DamageType.Gun);
            }
        }

        // Now spawn the bullet trail
        if (bulletTrailPrefab)
        {
            GameObject trailObj = Instantiate(bulletTrailPrefab, gunMuzzle.position, Quaternion.identity);
            BulletTrail trail = trailObj.GetComponent<BulletTrail>();

            if (trail != null)
            {
                trail.Initialize(gunMuzzle.position, hitPoint);
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * gunData.raycastRange, Color.red, 0.5f);
    }



    private void ShotgunRaycastShoot()
    {
        for (int i = 0; i < gunData.pelletsPerShot; i++)
        {
            Vector3 pelletDirection = gunMuzzle.forward;
            float currentSpread = isAiming ? gunData.adsSpreadAngle : gunData.hipfireSpreadAngle;

            if (currentSpread > 0f)
            {
                pelletDirection = ApplySpread(pelletDirection, currentSpread);
            }

            Ray ray = new Ray(gunMuzzle.position, pelletDirection);
            Vector3 hitPoint = ray.origin + ray.direction * gunData.raycastRange;

            if (Physics.Raycast(ray, out RaycastHit hit, gunData.raycastRange))
            {
                hitPoint = hit.point;

                if (hit.collider.CompareTag("Zombie"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }

            // Spawn trail for each pellet
            if (bulletTrailPrefab)
            {
                GameObject trailObj = Instantiate(bulletTrailPrefab, gunMuzzle.position, Quaternion.identity);
                BulletTrail trail = trailObj.GetComponent<BulletTrail>();

                if (trail != null)
                {
                    trail.Initialize(gunMuzzle.position, hitPoint);
                }
            }

            Debug.DrawRay(ray.origin, ray.direction * gunData.raycastRange, Color.yellow, 0.2f);
        }
    }


    private Vector3 ApplySpread(Vector3 direction, float spreadAngle)
    {
        float spreadRadius = Mathf.Tan(spreadAngle * Mathf.Deg2Rad / 2f);

        // Only randomize along the X-axis (left-right)
        float randomX = UnityEngine.Random.Range(-spreadRadius, spreadRadius);

        Vector3 spreadDirection = direction + (gunMuzzle.right * randomX);
        return spreadDirection.normalized;
    }


    // Weapon Manager access
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public bool IsReloading => isReloading;

    public int GetCurrentAmmo() => currentAmmo;
    public int GetReserveAmmo() => reserveAmmo;

    public void ReinitializeWeapon()
    {
        currentAmmo = gunData.magazineSize;
        reserveAmmo = gunData.reserveAmmo;

        if (weaponModelInstance != null)
        {
            Destroy(weaponModelInstance);
        }

        if (gunData.weaponModelPrefab != null && modelHolder != null)
        {
            weaponModelInstance = Instantiate(
                gunData.weaponModelPrefab,
                modelHolder.position,
                modelHolder.rotation,
                modelHolder
            );
        }
    }



    private void HandleADSMovement()
    {
        if (playerMovement == null) return;

        if (isAiming && !adsSlowed)
        {
            playerMovement.speed = originalMoveSpeed * gunData.adsSpeedMultiplier;
            adsSlowed = true;
        }
        else if (!isAiming && adsSlowed)
        {
            playerMovement.speed = originalMoveSpeed;
            adsSlowed = false;
        }
    }


}
