using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    public event EventHandler<RayCastHitInteract> OnRayCastHitInteract; //event for telling GM player interact
    public event EventHandler<OverLapHitInteract> OnOverLapHitInteract;
    public class RayCastHitInteract : EventArgs
    {
        public GameObject lookAtInteract;
    }
    public class OverLapHitInteract : EventArgs
    {
        public GameObject overLapHit;
    }

    [Header("Points")]
    public int currentPoints = 0;
    [SerializeField] private int totalPoints = 0;
    [SerializeField] private int totalKills = 0;

    [Header("Inventory")]
    public Transform weaponHolder;
    private GameObject currentWeapon;
    [SerializeField] public List<PerkSodasSO> playerPerkList;
    ///player var
    private int perkCount = 0;
    private int perkLimit = 4;

    [Header("Interaction")]
    private InputSystem_Actions inputActions;

    [SerializeField] float interactRayCastDist = 4f;
    private GameObject storedRayHit = null;
    private GameObject storedShereHit; //for to hold object from sphere hitm when/if set up

    public float sphereRadius = 1f;
    public float castDistance = 5f;

    [Header("Health")]
    [SerializeField] private int hp = 3;
    private bool isHurt = false;
    private float hurtInterval = 3;

    [Header("HUD")]
    [SerializeField] private HUDController hudControllerObject;
    [SerializeField] private TextMeshProUGUI interactText;
    private bool sawTheObject = false;

    private void Awake()
    {
        Instance = this;
        //reference the new input system to get acess to 'performed.
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
        //subscribe to interact event
        inputActions.Player.Interact.performed += Interact_performed;

        hudControllerObject.UpdateScore(currentPoints); // call this to set points to 0?
    }
    private void Start()
    {
        HideUI();
    }

    private void Update()
    {
        //interactions w/ world objs
        Interactions();

        //hp regen timer
        HurtCountDown();
    }

    ///hp related methods
    public int GetPlayerHP()
    {
        return hp;
    }
    public void SetPlayerHP(int hp)
    {
        this.hp = hp;
    }
    public void isHurtOn()
    {
        hp--;
        Debug.Log("ow: " + hp);
        isHurt = true;
    }

    private void HurtCountDown()
    {
        if (isHurt)
        {
            if (hp <= 0)
            {
                // Game Ends
                Destroy(gameObject);
                SceneManager.LoadScene(0);
            }

            //if hp=0 then GAMEOVER
            hurtInterval -= Time.deltaTime;
            if (hurtInterval < 0)
            {
                //if count down finishes reset variables n give back hp
                hp = 3;
                isHurt = false;
                hurtInterval = 3;
                Debug.Log("Timer Done. HP restored: " + hp);
            }
        }
    }

    ///interact related methods
    private void Interact_performed(InputAction.CallbackContext obj)
    {
        //Player HOLD E
        //Debug.Log("MOZZERELLA!");
        Debug.Log("stored ray hit: " + storedRayHit);
        Debug.Log("stored sphere hit: " + storedShereHit);
        if (storedRayHit != null)
        {
            //package the object data and send to GM


            OnRayCastHitInteract?.Invoke(this, new RayCastHitInteract { lookAtInteract = storedRayHit });
            //MiscInteractions(storedRayHit);


        }
        else if (storedShereHit != null)
        {
            OnOverLapHitInteract?.Invoke(this, new OverLapHitInteract { overLapHit = storedShereHit });
        }

    }

    private void Interactions()
    {
        // these should wake up respective UI for objects hit e.g. Press [KEY] to interact
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, interactRayCastDist))
        {
            //Debug.Log(raycastHit.transform);
            storedRayHit = raycastHit.transform.gameObject;
            storedShereHit = null;
            if (storedRayHit.GetComponent<IInteract>()!=null)
            {
              
                    ShowUI(storedRayHit.GetComponent<IInteract>());
               
                
            }
            else
            {
                HideUI();
            }
            
        }
        else
        {
            Collider[] sphereHits = Physics.OverlapSphere(transform.position, sphereRadius);

            storedRayHit = null; // clear ray so you don't accidentally double interact

            foreach (var hit in sphereHits)
            {
                if (hit != null) // optional: filter by tag/layer here
                {
                    storedShereHit = hit.gameObject;
                    if (storedShereHit.GetComponent<IInteract>() != null)
                    {
                        //interactText.text += storedShereHit.GetComponent<IInteract>().GetInteractText();
                      
                            ShowUI(storedShereHit.GetComponent<IInteract>());
                       
                        
                    }
                    else
                    {
                        
                        HideUI();
                       
                    }

                    break;
                }
            }
        }
      




            Debug.DrawRay(transform.position, transform.forward, Color.red);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand") && hp > 0)
            isHurtOn();
    }
    // points 
    public void AddPoints(int amount)
    {
        //the object calling this does math (addition) and passes it into this method
        currentPoints += amount;
        totalPoints += amount;
        HUDController.Instance.UpdateScore(currentPoints);

    }
    public bool SpendPoints(int amount)
    {
        if (currentPoints >= amount)
        {
            currentPoints -= amount;
            return true;
        }

        return false;
    }

    public int GetPoints()
    {
        return currentPoints;
    }
    public int GetTotalPoints()
    {
        return totalPoints;
    }
    public int GetTotalKills()
    {
        return totalKills;
    }
    public void SetPoints(int score)
    {
        //the object that is calling this does math (subtract) and passes the result into this method
        currentPoints = score;
        HUDController.Instance.UpdateScore(currentPoints);
    }

    public void AddKillCount()
    {
        totalKills++;
    }
    //weapon management

    public void GiveWeapon(GameObject weaponPrefab)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        currentWeapon = Instantiate(weaponPrefab, weaponHolder);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
    }

    public bool HasWeapon()
    {
        return currentWeapon != null;
    }
    private void OnDestroy()
    {
        inputActions.Player.Interact.performed -= Interact_performed;
    }

    //perks
    public bool HasPerk(PerkSodasSO perkSoda)
    {
        //iterate thu player list to see if they have said perk
        for (int i = 0; i < playerPerkList.Count; i++)
        {
            if (perkSoda == playerPerkList[i])
            {
                return true;
            }
        }
        return false;
    }
    public void AddPerkCount()
    {
        perkCount++;
    }
    public int GetPerkCount()
    {
        return perkCount;
    }
    public int GetPerkLimit()
    {
        return perkLimit;
    }

    //for the interact ui
    public void ShowUI(IInteract interactObject)
    {
        
        if (!interactObject.IsElectrical())
        {
            interactText.text = interactObject.GetInteractText();
        }
        else
        {
            //assume is electrical
            if (GameManager_Purchases.Instance.GetPowerStatus())
            {
                interactText.text = interactObject.GetInteractText();
            }
            else
            {
                interactText.text = "Turn on the power!";
            }
        }
        //if we add a backdrop to text, uncomment if to prevent empty backdrops
        //if(!string.IsNullOrEmpty(interactObject.GetInteractText()){ }
        interactText.enabled = true;
    }
    public void HideUI()
    {
        interactText.enabled = false;
    }


}
