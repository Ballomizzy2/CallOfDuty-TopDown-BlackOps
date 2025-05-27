using UnityEngine;

public class ZombieInteracts : MonoBehaviour
{
    
    public int hitCounts = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            hitCounts--;
            if (hitCounts == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
