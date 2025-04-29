using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerMelee Instance { get; private set; }
    [Header("Points")]
    public int currentPoints = 1000;

    [Header("Inventory")]
    public Transform weaponHolder;
    private GameObject currentWeapon;

    //points

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
}
