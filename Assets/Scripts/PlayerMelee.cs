using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMelee : MonoBehaviour
{
    public event EventHandler OnMeleeAction;
    
    [SerializeField] private GameObject playerMeleeHitBox;
    [SerializeField] private float meleeTimer= 0.1f;
    private bool meleeBoxIsActive = false;
    private InputSystem_Actions inputActions;

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

    private void MeleeHitBoxHandler_OnMeleeContact(object sender, EventArgs e)
    {
        //unpack the game object ref and deal damage
        Debug.Log("thip");
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
    private void Update()
    {
        //
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
}
