using UnityEngine;

public class ZombieInteracts : MonoBehaviour
{
    public int hitCount = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            hitCount--;
            if (hitCount == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
