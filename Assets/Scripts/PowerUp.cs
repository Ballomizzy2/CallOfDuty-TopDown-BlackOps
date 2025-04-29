using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public GameObject nukeBlastPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(gameObject.name);

            if (type == PowerUpType.Nuke)
            {
                Instantiate(nukeBlastPrefab, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject);
        }
    }
}
