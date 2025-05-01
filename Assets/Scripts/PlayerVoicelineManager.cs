using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

public class PlayerVoicelineManager : MonoBehaviour
{
    public AudioSource voiceChannel;

    [Header("General Voicelines")]
    public AudioClip[] lowAmmoClips;
    public AudioClip[] outOfAmmoClips;
    public AudioClip[] noMoneyClips;
    public AudioClip[] zombieKillClips;
    public AudioClip[] pickupInstaKillClips;
    public AudioClip[] pickupDoublePointsClips;
    public AudioClip[] pickupMaxAmmoClips;
    public AudioClip[] pickupNukeClips;
    public AudioClip[] pickupCarpenterClips;

    [Header("Perk-Specific Voicelines")]
    public AudioClip juggernogClip;
    public AudioClip speedColaClip;
    public AudioClip doubleTapClip;

    [Header("Cooldown")]
    public float voicelineCooldown = 10f;
    internal bool canSpeak = true;
    internal bool outOfAmmoSaid = false;
    internal bool hasreloaded = false;

    public static PlayerVoicelineManager Instance;

    private Dictionary<string, AudioClip> perkClips;

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

        perkClips = new Dictionary<string, AudioClip>()
        {
            { "Juggernog", juggernogClip },
            { "SpeedCola", speedColaClip },
            { "DoubleTap", doubleTapClip },
        };
    }

    public void PlayVoiceline(AudioClip[] clipPool)
    {
        if (canSpeak && clipPool.Length > 0)
        {
            AudioClip chosen = clipPool[Random.Range(0, clipPool.Length)];
            voiceChannel.PlayOneShot(chosen);
            StartCoroutine(VoicelineCooldown());
        }
    }

    public void PlayVoiceline(AudioClip clip)
    {
        if (canSpeak && clip != null)
        {
            voiceChannel.PlayOneShot(clip);
            StartCoroutine(VoicelineCooldown());
        }
    }

    public void PlayPerkVoiceline(string perkName)
    {
        if (perkClips.TryGetValue(perkName, out AudioClip clip))
        {
            PlayVoiceline(clip);
        }
        else
        {
            Debug.LogWarning($"No voiceline assigned for perk: {perkName}");
        }
    }

    private IEnumerator VoicelineCooldown()
    {
        canSpeak = false;
        yield return new WaitForSeconds(voicelineCooldown);
        canSpeak = true;
    }
}
