using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //player's movement speed variable
    public float speed;
    private Vector2 move;

    //This function gets the new input system settings and stores them in move
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    
    void Update()
    {
        movePlayer();
        aimPlayer();
    }

    //function that controls our player's movement
    void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0, move.y);
        
        transform.Translate(movement * (speed * Time.deltaTime), Space.World);
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
}
