using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float radius = 5f;
    public float maxDamage = 100f;
    public LayerMask damageMask;

    [Header("Visuals")]
    public GameObject explosionEffect; // Assign a prefab like ExplosionVFX

    [Header("Audio")]
    public AudioClip explosionSound;


    void Start()
    {
        if (explosionEffect)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        if (explosionSound)
        {
            AudioSource tempAudio = gameObject.AddComponent<AudioSource>();
            tempAudio.clip = explosionSound;
            tempAudio.volume = 1f;
            tempAudio.pitch = Random.Range(0.95f, 1.05f);
            tempAudio.spatialBlend = 1f;
            tempAudio.minDistance = 5f;
            tempAudio.maxDistance = 50f;
            tempAudio.Play();
            Destroy(tempAudio, explosionSound.length);
        }

        Explode();
        Destroy(gameObject, explosionSound.length); // Destroy AFTER sound finishes
    }


    void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, damageMask);

        foreach (Collider hit in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            float distanceFactor = 1f - Mathf.Clamp01(distance / radius); // 1 = close, 0 = far
            float damage = maxDamage * distanceFactor;

            if (hit.CompareTag("Zombie"))
            {
                // Apply damage to zombie
                /*ZombieHealth zombie = hit.GetComponent<ZombieHealth>();
                if (zombie != null)
                {
                    zombie.TakeDamage(damage);
                }*/

                Debug.Log($"Explosion hit {hit.name} for {damage:F1} damage.");
            }

            // Optional: Knockback force
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDir = (hit.transform.position - transform.position).normalized;
                rb.AddForce(forceDir * 500f * distanceFactor, ForceMode.Impulse);
            }
        }
    }
}
