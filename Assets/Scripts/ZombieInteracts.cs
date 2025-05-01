using UnityEngine;

public class ZombieInteracts : MonoBehaviour
{
<<<<<<< HEAD

    public int hitCount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {

            hitCount--;
            if(hitCount == 0)
                Destroy(gameObject);
=======
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Barricade"))
        {
            BarricadeController barricade = other.GetComponent<BarricadeController>();
            if (barricade != null && !barricade.isBreached)
            {
                barricade.RegisterHit();
            }
>>>>>>> origin/Lester_D
        }
    }
}
