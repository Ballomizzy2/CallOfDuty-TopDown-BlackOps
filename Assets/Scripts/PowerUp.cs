using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public GameObject nukeBlastPrefab;
    public GameObject playerPrefab;

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
            }
            
            Destroy(gameObject);
        }
    }
}
