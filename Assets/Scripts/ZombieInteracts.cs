using UnityEngine;

public class ZombieInteracts : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Barricade"))
        {
            BarricadeController barricade = other.GetComponent<BarricadeController>();
            if (barricade != null && !barricade.isBreached)
            {
                barricade.RegisterHit();
            }
        }
    }
}
