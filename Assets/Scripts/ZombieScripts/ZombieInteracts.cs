using UnityEngine;

public class ZombieInteracts : MonoBehaviour
{

    public int hitCount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {

            hitCount--;
            if(hitCount == 0)
                Destroy(gameObject);
        }
    }
}
