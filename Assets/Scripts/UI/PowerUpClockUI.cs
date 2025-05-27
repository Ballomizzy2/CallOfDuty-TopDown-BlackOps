using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// used to "animate" the power up UI
/// refrence: Code Monkey Kitchen Chaos 8:28:25
/// script location | power up held...
/// -----------------------
/// powerUpManager | instakill (IEnumerator)
/// GM_scores      | double points (time - Time.deltatime)
/// </summary>
public class PowerUpClockUI : MonoBehaviour
{

    [SerializeField] private PowerUpType powerUpType;
    [SerializeField] private Image powerUpClock;

    private void DoTheCountDown()
    {
        switch (powerUpType)
        {
            case PowerUpType.InstaKill:
                //PowerUpManager uses an IEnumerator...
                //powerUpClock.fillAmount = 
                break;
            case PowerUpType.DoublePoints:
                //somethn
                powerUpClock.fillAmount = GameManager_Scores.Instance.GetPowerUpTimer();
                break;
            default:
                //lol
                break;
        }
    }
    private void Update()
    {
        DoTheCountDown();
    }

    

}
