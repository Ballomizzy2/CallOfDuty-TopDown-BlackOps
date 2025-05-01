using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject impactEffectPrefab;

    private float speed;
    public float lifetime = 2f;
    public int damage;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SetBulletStats(float bulletSpeed, int bulletDamage)
    {
        speed = bulletSpeed;
        damage = bulletDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
<<<<<<< HEAD
            // Apply damage logic here (for now just destroy the bullet)
            other.GetComponent<Enemy>().TakeDamage(damage);
=======
            if (impactEffectPrefab)
            {
                Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
        else
        {
            if (impactEffectPrefab)
            {
                Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            }

>>>>>>> origin/Lester_D
            Destroy(gameObject);
        }
    }

}
