using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject winUI, loseUI;

    private void Start()
    {
        winUI.SetActive(false);
        loseUI.SetActive(false);
    }
    public void WinGame()
    {
        winUI.SetActive(true);
        loseUI.SetActive(false);
    }

    public void LoseGame()
    {
        loseUI.SetActive(true);
        winUI.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game Scene");
    }
    public void LeaveGame()
    {
        SceneManager.LoadScene("Main Menu");
    }


}
