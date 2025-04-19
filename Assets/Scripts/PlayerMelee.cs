using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static MeleeHitBoxHandler;

public class PlayerMelee : MonoBehaviour
{
    public event EventHandler OnMeleeAction;

    [Header("Melee")]
    [SerializeField] private GameObject playerMeleeHitBox;
    [SerializeField] private float meleeTimer= 0.1f;
    private bool meleeBoxIsActive = false;
    private InputSystem_Actions inputActions;

    //health stuff- extract into appropriate place later
    [Header("Health")]
    [SerializeField] private int hp = 3;
    private bool isHurt = false;
    private float hurtInterval = 3;

    [Header("Interaction")]
    [SerializeField] float interactRayCastDist = 4f;

    private void Start()
    {
        playerMeleeHitBox.SetActive(false);
    }

    private void Awake()
    {
        //reference the new input system to get acess to 'performed.
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        //subscribing to event from input system
        inputActions.Player.Melee.performed += Melee_performed;

        //subscribing to an event from itself
        OnMeleeAction += PlayerMelee_OnMeleeAction;

        MeleeHitBoxHandler.Instance.OnMeleeContact += MeleeHitBoxHandler_OnMeleeContact;
    }

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
        //Debug.Log("swish!");
        playerMeleeHitBox.SetActive(true);
        meleeBoxIsActive=true;
    }

    private void Melee_performed(InputAction.CallbackContext obj)
    {
        //firing event when 'F' key is pressed
        OnMeleeAction?.Invoke(this,EventArgs.Empty);
        
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
    private void Interactions()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, interactRayCastDist))
        {
            Debug.Log(raycastHit.transform);
        }

        
        //Debug.DrawRay(transform.position, transform.forward, Color.red);
    }
}
