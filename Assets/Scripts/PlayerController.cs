<<<<<<< HEAD
using System;
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

=======
using UnityEngine;

public class PlayerController : MonoBehaviour
{
>>>>>>> origin/Lester_D
    [Header("Points")]
    public int currentPoints = 1000;

    [Header("Inventory")]
    public Transform weaponHolder;
    private GameObject currentWeapon;

<<<<<<< HEAD
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

    private const int POWER_LAYER = 11;

    private void Awake()
    {
        Instance = this;
        //reference the new input system to get acess to 'performed.
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
        //subscribe to interact event
        inputActions.Player.Interact.performed += Interact_performed;
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
                SceneManager.LoadScene("Main Menu");
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
            if(storedRayHit.layer == POWER_LAYER)
            {
                PowerSwitchController.Instance.ActivatePower();
            }

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
                    break;
                }
            }
        }



        //Debug.DrawRay(transform.position, transform.forward, Color.red);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand") && hp > 0)
            isHurtOn();
    }
=======
    //points

>>>>>>> origin/Lester_D
    public void AddPoints(int amount)
    {
        currentPoints += amount;
        //ui update later
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

<<<<<<< HEAD
    public int GetPoints()
    {
        return currentPoints;
    }
    public void SetPoints(int score)
    {
        currentPoints = score;
    }

=======
>>>>>>> origin/Lester_D
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
<<<<<<< HEAD
    private void OnDestroy()
    {
        inputActions.Player.Interact.performed -= Interact_performed;
    }
=======
>>>>>>> origin/Lester_D
}
