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
     * double points - DoublePoints drop will fire an EventEMpty.args and when it gets here, multiplier =2
     * powers: nuke, carpenter, etc
     */
    [SerializeField] private int multiplier=1;
    [SerializeField] private float doublePointsTime=30f;
    private int roundPoints = 0;

    [SerializeField] private WeaponManager weaponManagerScript;
    int pointsToAdd = 10;
    public static GameManager_Scores Instance {  get; private set; }
    private void Awake()
    {
      Instance = this;
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
    public void StartDoublePoints()
    {
        multiplier = 2;
    }

    private void CountDownDoublePoints()
    {
        doublePointsTime -= Time.deltaTime;
        if (doublePointsTime <= 0f)
        {
            //Kitchen Chaos timer
            //after x secs
            multiplier = 1;
            doublePointsTime = 30f;
        }
        
    }
    public void PointsPerHit()
    {
        pointsToAdd = 10;
        PointsMathStuff(pointsToAdd);
    }

    public void PointsPerKill(DamageType killType)
    {
        switch (killType)
        {
            case DamageType.Knife:
                pointsToAdd = 130;
                break;
            case DamageType.Gun:
                pointsToAdd = 60;
                break;
            case DamageType.Explosive:
                //place holder, prob not gonna make is :-:
                break;
            case DamageType.Nuke:
                pointsToAdd = 400;
                break;

        }
        PlayerController.Instance.AddKillCount();
        PointsMathStuff(pointsToAdd);

    }
    public void PointsPerBarrier()
    {
        pointsToAdd = 20;
        PointsMathStuff(pointsToAdd);
    }
    public void NukePoints()
    {
        pointsToAdd = 400;
        PointsMathStuff(pointsToAdd);
    }

    public void PointsPerRound()
    {
        roundPoints += 50;
        PointsMathStuff(roundPoints);
    }

    private void PointsMathStuff(int p)
    {
        //tired of calling player Instance
        PlayerController.Instance.AddPoints(p * multiplier);
    }


}
