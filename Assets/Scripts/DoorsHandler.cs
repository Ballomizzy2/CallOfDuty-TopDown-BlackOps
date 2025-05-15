using UnityEngine;

public class DoorsHandler : MonoBehaviour, IInteract
{
    [SerializeField] int price;
    [SerializeField] GameObject doorA;
    [SerializeField] GameObject doorB;
    [SerializeField] BoxCollider boxCollider;

    //shop variables soundfx
    private bool canPayFor;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //listen for a successful purchase from GM then:
    //disable collider
    //
    public int GetPrice()
    {
        return price;
    }
    public void AnimateDoors()
    {
        boxCollider.enabled = false;
        // Instantly rotate Door A -90° around Y
        doorA.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);

        // Instantly rotate Door B +90° around Y
        doorB.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);

    }

    public bool IsElectrical()
    {
        return false;
    }
    public void Interact(PlayerController player)
    {
        //already have access to door, just open it here :]

        int tempPrice = GetPrice();
        int playerScore = player.currentPoints;
        if (playerScore >= tempPrice)
        {
            canPayFor = true;

            player.SetPoints(playerScore - tempPrice);
            AnimateDoors();

        }
        else
        {
            //canPayFor should default to false after calling noise
            canPayFor = false;
        }
    }
    public bool CanAffordSoundFX()
    {
        return canPayFor;
    }
}
