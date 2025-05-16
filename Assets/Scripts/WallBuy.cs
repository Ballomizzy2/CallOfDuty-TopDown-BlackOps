using UnityEngine;

public class WallBuy : MonoBehaviour, IInteract
{
    [Header("Weapon Settings")]
    public GunData weaponToGive;
    public int cost = 500; // Placeholder for future point system

    //[Header("Interaction Settings")]
    //public float interactionDistance = 3f; // How close the player needs to be
    //public KeyCode interactionKey = KeyCode.E;

    [Header("Audio")]
    public AudioClip buySound;
    public AudioClip deniedSound;

    [Header("UI Settings")]
    public Canvas wallBuyCanvas;
    public UnityEngine.UI.Image weaponIconImage;
    public TMPro.TextMeshProUGUI costText;


    private Transform player;

    //shop variables soundfx
    private bool canPayFor;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Update UI visuals
        if (weaponIconImage != null && weaponToGive != null && weaponToGive.weaponIcon != null)
        {
            weaponIconImage.sprite = weaponToGive.weaponIcon;
        }

        if (costText != null)
        {
            costText.text = cost.ToString();
        }

    }

    private void Update()
    {
        //if (player == null) return;

        //float distance = Vector3.Distance(player.position, transform.position);

        //if (distance <= interactionDistance)
        //{
        //    if (Input.GetKeyDown(interactionKey))
        //    {
        //        AttemptPurchase();
        //    }
        //}
    }

    public void AttemptPurchase()
    {
        int playerPoints = PlayerController.Instance.GetPoints();

        WeaponManager weaponManager = player.GetComponent<WeaponManager>();

        if (weaponManager == null) return;

        // Check distance before wasting time
        if (playerPoints >= cost)
        {
            // Check if player already has this weapon
            bool alreadyHasWeapon = false;
            foreach (Gun gun in weaponManager.weapons)
            {
                if (gun.gunData == weaponToGive)
                {
                    alreadyHasWeapon = true;
                    break;
                }
            }

            if (alreadyHasWeapon)
            {
                Debug.Log("Player already has this weapon! Purchase denied.");
                PlayDeniedSound();
                return;
            }
            PlayerController.Instance.SetPoints(playerPoints-cost);
            // Otherwise, purchase successful
            weaponManager.ReplaceCurrentWeapon(weaponToGive);
            Debug.Log($"Player bought {weaponToGive.gunName} for {cost} points!");

            PlayBuySound();

            // TODO: Deduct points here
        }
        else
        {
            Debug.Log("Not enough points to buy weapon!");
            PlayDeniedSound();
        }
    }

    private void PlayBuySound()
    {
        if (buySound)
        {
            AudioSource tempAudio = gameObject.AddComponent<AudioSource>();
            tempAudio.clip = buySound;
            tempAudio.volume = 1f;
            tempAudio.pitch = Random.Range(0.95f, 1.05f);
            tempAudio.spatialBlend = 1f;
            tempAudio.minDistance = 5f;
            tempAudio.maxDistance = 30f;
            tempAudio.Play();
            Destroy(tempAudio, buySound.length);
        }
    }

    private void PlayDeniedSound()
    {
        if (deniedSound)
        {
            AudioSource tempAudio = gameObject.AddComponent<AudioSource>();
            tempAudio.clip = deniedSound;
            tempAudio.volume = 1f;
            tempAudio.pitch = Random.Range(0.95f, 1.05f);
            tempAudio.spatialBlend = 1f;
            tempAudio.minDistance = 5f;
            tempAudio.maxDistance = 30f;
            tempAudio.Play();
            Destroy(tempAudio, deniedSound.length);
        }
    }

    //IInteract contract
    public bool IsElectrical()
    {
        return false;
    }
    public void Interact(PlayerController player)
    {
        AttemptPurchase();
    }

    public bool CanAffordSoundFX()
    {
        return canPayFor;
    }

    public bool UsesUniversalStoreSoundFX()
    {
        return false;
    }

}