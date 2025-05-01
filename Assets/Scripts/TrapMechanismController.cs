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

    private void Update()
    {
        if (isActive)
        {
            TrapHurtTime();
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
        if(other.CompareTag(ZOMBIE_TAG))
        {
            bool isKnife = false;
            other.gameObject.GetComponent<Enemy>().TakeDamage(shockDMG, isKnife);
        }
        else if(other.CompareTag(PLAYER_TAG))
        {
            other.gameObject.GetComponent<PlayerController>().isHurtOn();
        }
    }
}
