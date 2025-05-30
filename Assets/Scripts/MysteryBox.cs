using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// logic for mysterybox
/// ANIMATION SHOULD BE A SEPERATE SCRIPT HELD BY A SEPERATE OBJECT (harder to implement in here since box disables on leave?)
/// </summary>
public class MysteryBox : MonoBehaviour, IInteract
{
    [Header("Weapon Pool")]
    public List<GunData> possibleGuns = new List<GunData>();

    [Header("Interaction")]
    public float interactDistance = 3f;

    private Transform playerTransform;

    [Header("GameObjectReferences")]
    [SerializeField] private GameObject lid;
    private bool isOpen=false;
    [SerializeField] private Transform weaponSpawnReference;
    [SerializeField] private GameObject weaponModelParent;
    private Dictionary<string, GameObject> modelLookup;
    private GunData tempWeapon;
    private GameObject currentSpawnedModel;
    private int boxPrice = 950;

    [Header("MysteryBox Display Handler")]
    [SerializeField] private MysteryBoxDisplayHandler displayHandler;

    [Header("debug")]
    [SerializeField] private bool isToy=false;

    //shop variables soundfx
    private bool canPayFor;
    private void Awake()
    {
        modelLookup = new Dictionary<string, GameObject>();

        // Search under a parent GameObject like "WeaponModels"
        foreach (Transform model in weaponModelParent.transform)
        {
            string cleanName = model.name.Replace(" Model", "").Trim();
            modelLookup[cleanName] = model.gameObject;
        }
    }
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {


    }

    void GiveRandomWeapon(GunData randomGun)
    {
        

        WeaponManager weaponManager = playerTransform.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            weaponManager.ReplaceCurrentWeapon(randomGun);
            ToggleLid();
            if (currentSpawnedModel != null)
            {
                Destroy(currentSpawnedModel);
            }
            Debug.Log("Mystery Box gave you: " + randomGun.gunName);
        }
    }
    
    public void ToggleLid()
    {
        lid.transform.localRotation = isOpen
            ? Quaternion.Euler(0f, 0f, 0f)
            : Quaternion.Euler(90f, 0f, 0f);

        isOpen = !isOpen;
    }
    public void SpawnWeapon()
    {
        if (possibleGuns.Count == 0)
        {
            Debug.LogWarning("No weapons assigned to Mystery Box!");
            return;
        }

        // 1. Pick a random weapon
        GunData randomGun = possibleGuns[Random.Range(0, possibleGuns.Count)];
        if (isToy)
        {
            //toy is last, force weapong to fail
            randomGun = possibleGuns[possibleGuns.Count - 1];
        }
        
        if(randomGun.name == "Toy")
        {

            displayHandler.StartCoroutine(displayHandler.AnimationBoxWarp());
        }
        else
        {
            tempWeapon = randomGun;
            string weaponKey = randomGun.gunName;

            // 2. Lookup the matching model
            if (!modelLookup.TryGetValue(weaponKey, out GameObject modelPrefab))
            {
                Debug.LogWarning($"Weapon model not found: {weaponKey} Model");
                return;
            }


            // Instantiate and store new one
            currentSpawnedModel = Instantiate(modelPrefab, weaponSpawnReference.position, Quaternion.identity);

            currentSpawnedModel.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        }

      
    }

    private bool CanAfford_n_lidClose()
    {
        
        return !isOpen && PlayerController.Instance.GetPoints() > boxPrice;
    }

    //IInteract contract
    public bool IsElectrical()
    {
        return false;
    }
    public void Interact(PlayerController player)
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(playerTransform.position, transform.position);


        if (distance <= interactDistance)
        {
            if (CanAfford_n_lidClose())
            {
                ToggleLid();
                SpawnWeapon();
                SoundMng.Instance.PlayBuySound();
                PlayerController.Instance.SetPoints(PlayerController.Instance.GetPoints() - boxPrice);
            }
            else if (isOpen)
            {
                SoundMng.Instance.PlayAcceptSound();
                GiveRandomWeapon(tempWeapon);
            }
            else
            {
                SoundMng.Instance.PlayDeniedSound();
            }
        }
    }



    public bool CanAffordSoundFX()
    {
        return canPayFor;
    }

    public bool UsesUniversalStoreSoundFX()
    {
        return false;
    }
    public string GetInteractText()
    {
        return $"Press [E] to purchase Mystery Box: {boxPrice}";
    }

}
