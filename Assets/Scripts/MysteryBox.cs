using System.Collections.Generic;
using UnityEngine;

public class MysteryBox : MonoBehaviour
{
    [Header("Weapon Pool")]
    public List<GunData> possibleGuns = new List<GunData>();

    [Header("Interaction")]
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance && Input.GetKeyDown(interactKey))
        {
            GiveRandomWeapon();
        }
    }

    void GiveRandomWeapon()
    {
        if (possibleGuns.Count == 0)
        {
            Debug.LogWarning("No weapons assigned to Mystery Box!");
            return;
        }

        GunData randomGun = possibleGuns[Random.Range(0, possibleGuns.Count)];

        WeaponManager weaponManager = player.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            weaponManager.ReplaceCurrentWeapon(randomGun);
            Debug.Log("Mystery Box gave you: " + randomGun.gunName);
        }
    }
}
