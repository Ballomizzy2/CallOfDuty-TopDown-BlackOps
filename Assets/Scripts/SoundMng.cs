using UnityEngine;

public class SoundMng : MonoBehaviour
{
    public static SoundMng Instance { get; set; }

    public AudioClip zombieAttack;
    public AudioClip zombieDeath;
    public AudioClip zombieHurt;

    public AudioSource zombieChannel;

    public AudioSource playerChannel;
    public AudioClip playerDeath;
    public AudioClip playerHurt;

    [Header("Purchase sfx")]
    public AudioClip buySound;
    public AudioClip deniedSound;
    public AudioClip acceptSound;

    [Header("Round sfx")]
    [SerializeField] private AudioClip roundStartJingle;
    [SerializeField] private AudioClip roundEndJingle;
    [SerializeField] private AudioClip gameOverJingle;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayBuySound()
    {
        if (buySound)
        {
            AudioSource tempAudio = gameObject.AddComponent<AudioSource>();
            tempAudio.clip = buySound;
            tempAudio.volume = 1f;
            tempAudio.pitch = Random.Range(0.95f, 1.05f);
            tempAudio.spatialBlend = 1f;
            tempAudio.minDistance = 5f;
            tempAudio.maxDistance = 30f;
            tempAudio.Play();
            Destroy(tempAudio, buySound.length);
        }
    }

    public void PlayDeniedSound()
    {
        if (deniedSound)
        {
            AudioSource tempAudio = gameObject.AddComponent<AudioSource>();
            tempAudio.clip = deniedSound;
            tempAudio.volume = 1f;
            tempAudio.pitch = Random.Range(0.95f, 1.05f);
            tempAudio.spatialBlend = 1f;
            tempAudio.minDistance = 5f;
            tempAudio.maxDistance = 30f;
            tempAudio.Play();
            Destroy(tempAudio, deniedSound.length);
        }
    }
    public void PlayAcceptSound()
    {
        if (deniedSound)
        {
            AudioSource tempAudio = gameObject.AddComponent<AudioSource>();
            tempAudio.clip = deniedSound;
            tempAudio.volume = 1f;
            tempAudio.pitch = Random.Range(0.95f, 1.05f);
            tempAudio.spatialBlend = 1f;
            tempAudio.minDistance = 5f;
            tempAudio.maxDistance = 30f;
            tempAudio.Play();
            Destroy(tempAudio, deniedSound.length);
        }
    }
    public void PlayRoundStartJingle()
    {
        //add the src thingy
    }
    public void PlayRoundEndJingle()
    {

    }
    public void PlayGameOverJingle()
    {

    }
}