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
}