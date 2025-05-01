using UnityEngine;

public class TrapMechanismController : MonoBehaviour
{
    [SerializeField] private TrapSwitchController controller;
    [SerializeField] private GameObject shockHitBox;
    [SerializeField] private float activeTimer = 10f;
    [SerializeField] private bool isActive = false;
    [SerializeField] private int shockDMG = 100;
    private const string PLAYER_TAG= "Player";
    private const string ZOMBIE_TAG = "Zombie";
    //dot timer
    [SerializeField] private float slowedTimer = 2f;
    [SerializeField] private bool isSlowed = false;
    //player ref?
    private PlayerMovement pm;
    private float pm_originalSpeed;
    private float pm_tempSpeed;

    //zombie ref?

    

    private void Update()
    {
        if (isActive)
        {
            TrapHurtTime();
        }
        if(isSlowed)
        {
            ResetSpeed();
        }
    }
    public void TrapMechActivate()
    {
        activeTimer = 10f;
        isActive = true;
        shockHitBox.SetActive(true);
    }
    public void TrapHurtTime()
    {
        activeTimer -= Time.deltaTime;
        if (activeTimer <= 0f)
        {
            isActive = false;
            shockHitBox.SetActive(false);
            controller.TrapDeactivate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //remember who got hurt and their speed
        //slow them down (1/2)
        //after x sec,  return their speed
        if(other.CompareTag(ZOMBIE_TAG))
        {
            bool isKnife = false;
            other.gameObject.GetComponent<Enemy>().TakeDamage(shockDMG, isKnife);
            //zombie ref, idk how speed is calculated...
        }
        else if(other.CompareTag(PLAYER_TAG))
        {
            other.gameObject.GetComponent<PlayerController>().isHurtOn();
            pm= other.GetComponent<PlayerMovement>();
            DamageSpeed(other.gameObject);
        }
    }

    private void DamageSpeed(GameObject other)
    {
        if (other.CompareTag(ZOMBIE_TAG))
        {
            //slow da zombie
        }
        else if (other.CompareTag(PLAYER_TAG))
        {
            isSlowed = true;
            //remember base speed
            pm_originalSpeed = pm.speed;
            //half it
            pm.speed *= 0.5f;
        }
    }
    private void ResetSpeed()
    {
        slowedTimer -= Time.deltaTime;
        if(slowedTimer <= 0f)
        {
            isSlowed = false;
            pm.speed = pm_originalSpeed;
        }
    }
}
