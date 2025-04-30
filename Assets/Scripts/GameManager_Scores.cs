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
    [SerializeField] private int multiplier;
    [SerializeField] private int score;
    [SerializeField] private bool doublePointsTimer;

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

}
