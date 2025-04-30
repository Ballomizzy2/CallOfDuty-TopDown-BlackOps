using UnityEngine;

public class GameManager_Scores : MonoBehaviour
{
    /*listen for:
     * [event] - [origin of event]
     * hit zombie- gun hit zombie
     * kill zombie -gun hit zombie && hp<=0
     * barriers - barriers built
     * wave completed - GameManager_Rounds (or what ever handles the rounds)
     * -----Discuss how these will work-----
     * double points - DoublePoints prefab,James (influences multiplier variable)
     * powers: nuke, carpenter, etc
     */
    [SerializeField] private int multiplier=1;
    [SerializeField] private bool doublePointsTimer;

    [SerializeField] private Gun gunScript;
    private void Awake()
    {
        gunScript.OnBulletHitZombie += Gun_OnBulletHitZombie;
    }

    private void Gun_OnBulletHitZombie(object sender, System.EventArgs e)
    {
        int pointsEarned = 10;
        //hit a zombie +10 points
        PointCalulation(pointsEarned);
        Debug.Log("Here in GM_Scores!");
    }

    private void Start()
    {
        multiplier = 1;
    }
    private void Update()
    {
        if(multiplier == 2)
        {
            CountDownDoublePoints();
        }
    }

    private void CountDownDoublePoints()
    {
        //Kitchen Chaos timer
        //after x secs
        multiplier = 1;
    }
    private void PointCalulation(int pointsToAdd)
    {
        PlayerController.Instance.AddPoints(pointsToAdd * multiplier);
    }

}
