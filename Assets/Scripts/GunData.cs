using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun/New Gun Data")]
public class GunData : ScriptableObject
{
    [Header("Gun Identity")]
    public string gunName = "New Gun";

    [Header("Ammo Settings")]
    public int magazineSize = 8;
    public int reserveAmmo = 80;
    public float reloadTime = 1.5f;

    [Header("Shooting Settings")]
    public float fireDelay = 0.25f; // Time between shots
    public float bulletSpeed = 30f;
    public int damage = 20;

    [Header("References")]
    public GameObject bulletPrefab;
    public Transform muzzleFlashPrefab; // Optional

    [Header("Audio")]
    public AudioClip shootSound;
    public AudioClip reloadSound;

    [Header("Fire Mode Settings")]
    public bool isAutomatic = false;
    public bool isBurst = false;
    public int burstCount = 3;         // How many bullets per burst
    public float burstDelay = 0.1f;    // Delay between shots in a burst

    [Header("Firing Method")]
    public bool useRaycast = false;
    public float raycastRange = 100f;

    [Header("Accuracy Settings")]
    [Tooltip("Spread angle when hipfiring (in degrees).")]
    public float hipfireSpreadAngle = 5f;

    [Tooltip("Spread angle when aiming down sights (ADS) (in degrees).")]
    public float adsSpreadAngle = 1f;
    public float adsSpeedMultiplier = 0.5f; // How much the player slows when aiming


    [Header("Shotgun Settings")]
    public bool isShotgun = false;
    public int pelletsPerShot = 8;

    [Header("UI Settings")]
    public Sprite weaponIcon;
}
