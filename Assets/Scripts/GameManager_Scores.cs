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
    [SerializeField] private float doublePointsTime=5f;

    [SerializeField] private WeaponManager weaponManagerScript;
    private void Awake()
    {
        //weaponManager, get current gun-> gun.cs, subscribe to that event
        weaponManagerScript.GetCurrentWeapon().OnBulletHitZombie += CurrentGun_OnRayCastBulletHitZombie;
        weaponManagerScript.OnWeaponSwap += WeaponManagerScript_OnWeaponSwap;
    }

    private void WeaponManagerScript_OnWeaponSwap(object sender, WeaponManager.WeaponSwap e)
    {
        //bullet variant

        //ray cast variants
        //usubscribe to old gun
        weaponManagerScript.GetCurrentWeapon().OnBulletHitZombie -= CurrentGun_OnRayCastBulletHitZombie;

        //subscribe to new gun
        e.currentWeapon.OnBulletHitZombie += WeaponSwap_OnRayCastBulletHitZombie;
    }

    private void WeaponSwap_OnRayCastBulletHitZombie(object sender, System.EventArgs e)
    {
        //updated event for weapon swaping?
        int pointsEarned = 10;
        //hit a zombie +10 points
        PointCalulation(pointsEarned);
        Debug.Log("Here in GM_Scores!");
    }

    private void CurrentGun_OnRayCastBulletHitZombie(object sender, System.EventArgs e)
    {
        //default event
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
        doublePointsTime -= Time.deltaTime;
        if (doublePointsTime <= 0f)
        {
            //Kitchen Chaos timer
            //after x secs
            multiplier = 1;
            doublePointsTime = 5f;
        }
        
    }
    private void PointCalulation(int pointsToAdd)
    {
        PlayerController.Instance.AddPoints(pointsToAdd * multiplier);
    }

    private void OnDestroy()
    {
        weaponManagerScript.GetCurrentWeapon().OnBulletHitZombie -= CurrentGun_OnRayCastBulletHitZombie;
    }

}
