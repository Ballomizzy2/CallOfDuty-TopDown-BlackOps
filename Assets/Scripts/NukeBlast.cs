using UnityEngine;

public class NukeBlast : MonoBehaviour
{
    public float radius = 1000f;
    public int damage = 1000000000;

    void Start()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy") || hit.CompareTag("Zombie"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {

                    enemy.TakeDamage(damage,DamageType.Nuke);
                }
            }
        }
        GameManager_Scores.Instance.NukePoints();
        //Maybe stop the spawning of zombies for a few seconds
        //Give 400 points to the player
        //Play sound effect and visual effect
        //Maybe shake the screen

        Destroy(gameObject);
    }
}
