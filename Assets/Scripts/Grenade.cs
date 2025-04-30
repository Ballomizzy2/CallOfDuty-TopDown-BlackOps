using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip pinPullSound;

    public float fuseTime = 3f;
    public GameObject explosionPrefab;

    private Collider grenadeCollider;
    private Rigidbody grenadeRb;

    private void Start()
    {
        grenadeCollider = GetComponent<Collider>();
        grenadeRb = GetComponent<Rigidbody>();
    }

    public void PlayPinPullSound()
    {
        if (pinPullSound == null) return;

        AudioSource tempAudio = gameObject.AddComponent<AudioSource>();
        tempAudio.clip = pinPullSound;
        tempAudio.volume = 1f;
        tempAudio.pitch = Random.Range(0.95f, 1.05f);
        tempAudio.spatialBlend = 0f;
        tempAudio.Play();
        Destroy(tempAudio, pinPullSound.length);
    }

    public void BeginFuse()
    {
        Invoke(nameof(Explode), fuseTime);
    }

    private void Explode()
    {
        if (grenadeCollider != null) grenadeCollider.enabled = false;
        if (grenadeRb != null) grenadeRb.isKinematic = true;

        if (explosionPrefab)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 0.05f);
    }
}
