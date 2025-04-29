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
}
