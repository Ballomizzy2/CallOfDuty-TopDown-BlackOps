using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    //player's movement speed variable
    public float speed = 10f;
    
    public float sprintMultiplier = 1.5f;
    //currentSpeed is public for testing purposes
    public float currentSpeed;

    [Header("Stamina")] 
    public float maxStamina = 4f;
    public float staminaRegenRate = 1f; //per second
    public float staminaDrainRate = 1f; //per second
    public float sprintCooldown = 1f;
    //currentStamina, isInCooldown, and cooldownTimer are public for testing purposes
    public float currentStamina;
    public bool isInCooldown = false;
    public float cooldownTimer = 0f;
    
    private Vector2 move;
    private bool isSprinting = false;

    private void Awake()
    {
        currentStamina = maxStamina;
    }

    //This function gets the new input system settings and stores them in move
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    //This function uses the new input system to make the player sprint by holding the SHIFT key
    //Read the HandleStamina function for more context on this function
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (isInCooldown)
        {
            isSprinting = false;
            return;
        }
        
        if (context.performed)
        {
            isSprinting = true;
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }
    }
    void Update()
    {
        movePlayer();
        aimPlayer();
        handleStamina();
    }

    //function that controls our player's movement
    void movePlayer()
    {
        //if isSprinting == true: currentSpeed = sprintMultiplier * speed, else: currentSpeed = speed
        currentSpeed = isSprinting ? sprintMultiplier * speed : speed;
        
        Vector3 movement = new Vector3(move.x, 0, move.y);
        
        transform.Translate(movement * (currentSpeed * Time.deltaTime), Space.World);
    }

    //function that controls our player's aim. Makes player rotate to face the mouse.
    //Uses a ray pointing from the camera and an invisible plane created at the player's Y-coordinate
    //If the ray hits the plane, it rotates our player to face where the ray hit
    void aimPlayer()
    { 
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        // Create a flat XZ plane at the player's current Y level
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 direction = hitPoint - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = rotation;
            }
        }
    }

    //This function handles the player's stamina meter while sprinting
    void handleStamina()
    {
        //If the player's sprint meter was fully drained, we put a cooldown on sprinting
        //This if statement checks if we are on sprinting cooldown. If we are it counts down from our cooldown timer
        //and starts to regen our stamina.
        if (isInCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0)
            {
                isInCooldown = false;
            }
            
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
            return;
        }
        
        //If SHIFT is being held down, the player is moving, and our currentStamina is above zero, we start to drain
        //our stamina. Mathf.Max() doesn't allow our current stamina to go below zero
        if (isSprinting && move != Vector2.zero && currentStamina > 0)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0);
        }
        //If the player isn't currently sprinting we start to regenerate our stamina. Mathf.Min() doesn't allow
        //our currentStamina to go above our maxStamina
        else
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }

        //If currentStamina reaches 0, we initiate the cooldownTimer for our sprint and disable the ability to sprint
        //for the cooldown's duration
        if (currentStamina <= 0)
        {
            cooldownTimer = sprintCooldown;
            isInCooldown = true;
            isSprinting = false;
        }
    }
}
