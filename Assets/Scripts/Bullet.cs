using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject impactEffectPrefab;

    private float speed;
    public float lifetime = 2f;
    public int damage;
    [SerializeField] private GameManager_Scores gm_score;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SetBulletStats(float bulletSpeed, int bulletDamage, GameManager_Scores temp_gm_score)
    {
        speed = bulletSpeed;
        damage = bulletDamage;
        gm_score = temp_gm_score;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            // Apply damage logic here (for now just destroy the bullet)
            other.GetComponent<Enemy>().TakeDamage(damage, DamageType.Gun);
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

           
            other.GetComponent<Enemy>().TakeDamage(damage, DamageType.Gun);

            gm_score.PointsPerHit();
            Destroy(gameObject);
        }
    }

}
