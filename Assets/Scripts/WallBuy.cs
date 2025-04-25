using UnityEngine;

public class WallBuy : MonoBehaviour
{
    public GameObject weaponPrefab; //weapon to buy
    public int cost = 500;
    public string weaponName = "MP5";   //for display
    public KeyCode buyKey = KeyCode.E;

    private bool isPlayerInRange = false;
    private PlayerController player;

    private void Update()
    {     
        if (isPlayerInRange && player != null && Input.GetKeyDown(buyKey))
        {
            TryPurchaseWeapon();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null;
        }
    }

    private void TryPurchaseWeapon()
    {
        if (player != null && player.SpendPoints(cost))
        {
            player.GiveWeapon(weaponPrefab);
            Debug.Log($"Purchased: {weaponName}");
        }
        else
        {
            Debug.Log("Not enough points!");
        }
    }
}
