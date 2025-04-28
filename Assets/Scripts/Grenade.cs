using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float fuseTime = 3f;
    public GameObject explosionPrefab;

    private Collider grenadeCollider;
    private Rigidbody grenadeRb;

    private void Start()
    {
        grenadeCollider = GetComponent<Collider>();
        grenadeRb = GetComponent<Rigidbody>();

        Invoke(nameof(Explode), fuseTime);
    }

    private void Explode()
    {
        if (explosionPrefab)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Disable collision and physics
        if (grenadeCollider != null) grenadeCollider.enabled = false;
        if (grenadeRb != null) grenadeRb.isKinematic = true;

        Destroy(gameObject, 0.05f); // Slight delay for VFX separation
    }
}
