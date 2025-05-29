using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroVideoController : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] AudioSource audioSource;
    [SerializeField] string nextScene = "MainMenu";

    void Start()
    {
        DontDestroyOnLoad(audioSource.gameObject); // music persists

        // Start both video and audio
        videoPlayer.Play();
        audioSource.Play();

        // When video ends, load next scene
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Stop video; audio keeps going
        vp.Pause();
        SceneManager.LoadScene(nextScene);
    }
}
