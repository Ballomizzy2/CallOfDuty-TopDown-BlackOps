using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Teleporter : MonoBehaviour
{
    public GameObject teleportTarget; // The target object to teleport to
    public float teleportDelay = 0.5f; // Delay before teleporting

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collider has the "Player" tag
        {
            Debug.Log("Player entered the teleport area"); // Log message for debugging
            StartCoroutine(TeleportAfterDelay(other.gameObject)); // Start the teleportation coroutine
        }
    }

    private IEnumerator TeleportAfterDelay(GameObject player)
    {
        yield return new WaitForSeconds(teleportDelay); // Wait for the specified delay
        player.transform.position = teleportTarget.transform.position; // Teleport the player to the target position
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collider has the "Player" tag
        {
            // Optional: Add any actions to perform when the player exits the trigger area
        }
    }
}
