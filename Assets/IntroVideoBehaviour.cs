using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class IntroVideoBehaviour : MonoBehaviour
{
    [SerializeField] private VideoPlayer vp;

    private AudioSource music;

    void Awake()
    {
        music = vp.GetTargetAudioSource(0);
        DontDestroyOnLoad(music.gameObject);
    }

    void Start()
    {
        vp.loopPointReached += OnClipEnd;
        StartCoroutine(PlaySynced());
    }

    private IEnumerator PlaySynced()
    {
        // prepares video before playing
        vp.Prepare();
        while (!vp.isPrepared)
            yield return null;

        // slight audio buffer warmup
        music.Play();
        yield return new WaitForSeconds(0.05f);

        vp.Play();
    }

    private void OnClipEnd(VideoPlayer source)
    {
        source.Pause(); // freeze frame
        SceneManager.LoadScene("MainMenu");
    }
}
