using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public GameObject nukeBlastPrefab;
    public GameObject playerPrefab;
    internal bool instaKillActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(gameObject.name);

            if (type == PowerUpType.Nuke)
            {
                Instantiate(nukeBlastPrefab, transform.position, Quaternion.identity);
            }
            else if (type == PowerUpType.MaxAmmo)
            {
                playerPrefab.GetComponentInChildren<Gun>().reserveAmmo = playerPrefab.GetComponentInChildren<Gun>().gunData.reserveAmmo;
                playerPrefab.GetComponentInChildren<Gun>().currentAmmo = playerPrefab.GetComponentInChildren<Gun>().gunData.magazineSize;
                //figure out how to fill ammo for second weapon
            }
            else if (type == PowerUpType.InstaKill)
            {
                StartCoroutine(InstaKillRoutine(30));
            }
            else if (type == PowerUpType.DoublePoints)
            {
                //GameManager_Scores.Instance.CountDownDoublePoints();
            }
            else if (type == PowerUpType.Carpenter)
            {
                //BarricadeController
            }
            
            Destroy(gameObject);
        }
    }

    private IEnumerator InstaKillRoutine(float duration)
    {
        instaKillActive = true;
        
        yield return new WaitForSeconds(duration);

        instaKillActive = false;
    }

    private void instaKillEffect()
    {
        playerPrefab.GetComponentInChildren<Gun>().gunData.damage = 2147483647;
        //change melee and grenade damage
        
        //Put all of this inside a manager object and call the object.instance.isInstaKillActive in enemy.takedamage
        //if instakill is active, make zombie take 2147483647 damage
    }
}
