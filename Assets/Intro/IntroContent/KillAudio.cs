using UnityEngine.SceneManagement;
using UnityEngine;

public class KillAudio : MonoBehaviour
{
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Main Scene")
        {
            Destroy(gameObject);
        }
    }
}
