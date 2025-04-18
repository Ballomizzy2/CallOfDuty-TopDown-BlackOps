using UnityEngine;

public class TestDummyDamageTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject player;
    private float damageCooldown = 1.5f;
    private bool damageReady = true;
    private bool hitAlready=false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player && damageReady)
        {
            damageReady = false;
            player.GetComponent<PlayerMelee>().isHurtOn();
        }
    }
    private void Update()
    {
        if (!damageReady)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                damageReady = true;
                damageCooldown = 1.5f;
                Debug.Log("Ready to hurt you!");
            }
        }
    }

}
