using UnityEngine;

public class Bullet : MonoBehaviour
{
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
            other.GetComponent<Enemy>().TakeDamage(damage);
            
            gm_score.PointsPerHit();
            Destroy(gameObject);
        }
    }
}
