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

    [SerializeField] private WeaponManager weaponManagerScript;
    private void Awake()
    {
        //weaponManager, get current gun-> gun.cs, subscribe to that event
        weaponManagerScript.GetCurrentWeapon().OnBulletHitZombie += CurrentGun_OnBulletHitZombie;
        weaponManagerScript.OnWeaponSwap += WeaponManagerScript_OnWeaponSwap;
    }

    private void WeaponManagerScript_OnWeaponSwap(object sender, WeaponManager.WeaponSwap e)
    {
        //usubscribe to old gun
        weaponManagerScript.GetCurrentWeapon().OnBulletHitZombie -= CurrentGun_OnBulletHitZombie;

        //subscribe to new gun
        e.currentWeapon.OnBulletHitZombie += WeaponSwap_OnBulletHitZombie;
    }

    private void WeaponSwap_OnBulletHitZombie(object sender, System.EventArgs e)
    {
        //updated event for weapon swaping?
        int pointsEarned = 10;
        //hit a zombie +10 points
        PointCalulation(pointsEarned);
        Debug.Log("Here in GM_Scores!");
    }

    private void CurrentGun_OnBulletHitZombie(object sender, System.EventArgs e)
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
        //Kitchen Chaos timer
        //after x secs
        multiplier = 1;
    }
    private void PointCalulation(int pointsToAdd)
    {
        PlayerController.Instance.AddPoints(pointsToAdd * multiplier);
    }

    private void OnDestroy()
    {
        weaponManagerScript.GetCurrentWeapon().OnBulletHitZombie -= CurrentGun_OnBulletHitZombie;
    }

}
