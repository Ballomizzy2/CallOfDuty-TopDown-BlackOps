using UnityEngine;

public class PowerUpSoundManager : MonoBehaviour
{
    public AudioSource soundEffectChannel;

    [Header("Power Up Sounds")] 
    public AudioClip instaKillClip;
    public AudioClip doublePointsClip;
    public AudioClip maxAmmoClip;
    public AudioClip nukeClip;
    public AudioClip carpenterClip;
    
    public static PowerUpSoundManager Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlaySoundEffect(AudioClip clip)
    {
        if (clip != null)
        {
            soundEffectChannel.PlayOneShot(clip);
        }
    }
}
