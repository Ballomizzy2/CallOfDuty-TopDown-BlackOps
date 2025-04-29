using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static MeleeHitBoxHandler;

public class PlayerMelee : MonoBehaviour
{
    public static PlayerMelee Instance { get; private set; }
    public event EventHandler OnMeleeAction; 
    public event EventHandler<RayCastHitInteract> OnRayCastHitInteract; //event for telling GM player interact
    public class RayCastHitInteract :EventArgs
    {
        public GameObject lookAtInteract;
    }

    [Header("Melee")]
    [SerializeField] private GameObject playerMeleeHitBox;
    [SerializeField] private float meleeTimer= 0.1f;
    private bool meleeBoxIsActive = false;
    private InputSystem_Actions inputActions;


    [Header("Health")]
    [SerializeField] private int hp = 3;
    private bool isHurt = false;
    private float hurtInterval = 3;

    [Header("Interaction")]

    
    [SerializeField] float interactRayCastDist = 4f;
    private GameObject storedRayHit =null;
    //private GameObject storedShereHit; //for to hold object from sphere hitm when/if set up

    private void Start()
    {
        playerMeleeHitBox.SetActive(false);
    }

    private void Awake()
    {

        Instance = this;

        //reference the new input system to get acess to 'performed.
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        //subscribing to events from input system
        inputActions.Player.Melee.performed += Melee_performed;

        inputActions.Player.Interact.performed += Interact_performed;

        //subscribing to an event from itself
        OnMeleeAction += PlayerMelee_OnMeleeAction;

        MeleeHitBoxHandler.Instance.OnMeleeContact += MeleeHitBoxHandler_OnMeleeContact;

      
    }

    private void Update()
    {
        //timer to disable the actived melee hitbox
        MeleeHitBoxReset();

        //hp regen timer
        HurtCountDown();

        //interactions w/ world objs
        Interactions();
    }


    ///interact related methods
    private void Interact_performed(InputAction.CallbackContext obj)
    {
        //Player HOLD E
        //Debug.Log("MOZZERELLA!");
        Debug.Log(storedRayHit);
        if (storedRayHit != null)
        {
            //package the object data and send to GM
       

            OnRayCastHitInteract?.Invoke(this, new RayCastHitInteract { lookAtInteract = storedRayHit });

        }

    }

    private void Interactions()
    {
        //turn objects that hold an SO into interface?
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, interactRayCastDist))
        {
            //Debug.Log(raycastHit.transform);
            storedRayHit = raycastHit.transform.gameObject;
        }
        else
        {
            //stored item= null
            storedRayHit = null;
        }
        


        //Debug.DrawRay(transform.position, transform.forward, Color.red);
    }

    public int GetPlayerHP()
    {
        return hp;
    }
    public void SetPlayerHP(int hp)
    {
        this.hp = hp;
    }

    public float sphereRadius = 1f;
    public float castDistance = 5f;

    //private void OnDrawGizmos()
    //{
    //    Vector3 origin = transform.position;
    //    Vector3 direction = transform.forward;
    //    Vector3 endPoint = origin + direction * castDistance;

    //    Gizmos.color = Color.red;

    //    // Draw start and end spheres
    //    Gizmos.DrawWireSphere(origin, sphereRadius);
    //    Gizmos.DrawWireSphere(endPoint, sphereRadius);

    //    // Draw line between start and end
    //    Gizmos.DrawLine(origin, endPoint);
    //}

    ///Melee related methods
    private void MeleeHitBoxHandler_OnMeleeContact(object sender, MeleeHitEventArgs e)
    {
        //unpack the game object ref and deal damage
        //Debug.Log("thip");

        GameObject hitObject = e.hitObject;

        if (hitObject != null)
        {
            //Debug.Log("Hit: " + hitObject.name);
            Destroy(hitObject );
        }
        

    }

    private void PlayerMelee_OnMeleeAction(object sender, EventArgs e)
    {
        Debug.Log("swish!");
        playerMeleeHitBox.SetActive(true);
        meleeBoxIsActive=true;
    }

    private void Melee_performed(InputAction.CallbackContext obj)
    {
        //firing event when 'F' key is pressed
        OnMeleeAction?.Invoke(this,EventArgs.Empty);
        
    }

    private void MeleeHitBoxReset()
    {
        if (meleeBoxIsActive)
        {
            meleeTimer -= Time.deltaTime;
            if (meleeTimer < 0)
            {
                meleeBoxIsActive = false;
                playerMeleeHitBox.SetActive(false);
                meleeTimer = 0.1f;

            }
        }
    }

    ///Health relaated methods
    public void isHurtOn()
    {
        hp--;
        Debug.Log("ow: " + hp);
        isHurt =true;
    }

    private void HurtCountDown()
    {
        if (isHurt)
        {
            if (hp <= 0)
            {
                gameObject.SetActive(false);
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



}
