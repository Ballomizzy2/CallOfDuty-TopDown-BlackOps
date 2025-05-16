using UnityEngine;

public class TrapSwitchController : MonoBehaviour,IInteract
{
    /*this will:
     * make a reference to the trapMechism
     * trapMechanism will have a an id that will alter the trap damage output?
     * this will have to listen for power
     * 
     * 3 states:
     * off - no power
     * standby/ green
     * coolDown - red
     * on- blink green
     */
    [SerializeField] TrapMechanismController trapMech;
    [SerializeField] GameObject lever;
    [SerializeField] private float coolDownTimer = 5f;
    [SerializeField] private Renderer bulbRenderer;
    [SerializeField] private Color colorOff = Color.gray;
    [SerializeField] private Color colorReady = Color.green;
    [SerializeField] private Color colorCooldown = Color.red;
    private int price = 300;
    private bool isCoolDown=false;
    private bool isActive = false;

    //shop variables soundfx
    private bool canPayFor;
    private void Update()
    {
        if (isCoolDown)
        {
            TrapCoolDown();
        }
    }
    private void Start()
    {
        PowerSwitchController.Instance.OnLeverFlipped += PowerSwitchController_OnLeverFlipped;
    }

    private void PowerSwitchController_OnLeverFlipped(object sender, System.EventArgs e)
    {
        bulbRenderer.material.color = colorReady; // Green
    }

    public void TrapActivate()
    {
        //move lever 90?
        lever.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);

        //turn bulb green/blink
        bulbRenderer.material.color = colorReady; // Green

        //turn on trapMechanism
        trapMech.gameObject.SetActive(true);
        
        trapMech.GetComponent<TrapMechanismController>().TrapMechActivate();

    }
    public void TrapDeactivate()
    {
        //turn bulb red
        //s
        bulbRenderer.material.color = colorCooldown; // Red
        isCoolDown = true;


    }
    public void TrapReady()
    {
        //move lever back
        //
        bulbRenderer.material.color = colorReady; // Green

        lever.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

    }
    private void TrapCoolDown()
    {
        coolDownTimer -= Time.deltaTime;
        if(coolDownTimer <= 0 )
        {
            TrapReady();
            coolDownTimer = 5f;
            isCoolDown=false;
            isActive = false;
        }
    }
    public bool GetIsActive()
    {
        return isActive;
    }
    public void SetActive()
    {
        isActive = true;
    }

    //IInteract contract
    public bool IsElectrical()
    {
        return true;
    }
    public void Interact(PlayerController player)
    {
        bool readyToBuy = !GetIsActive();
       
        int playerScore = player.currentPoints;
        
        if (playerScore >= price && readyToBuy)
        {
           
            //- points
            //call trapOn
            canPayFor = true;
           
            player.SetPoints(playerScore -= price);
            TrapActivate();

        }
        else
        {
            canPayFor= false;
        }

    }

    public bool CanAffordSoundFX()
    {
        return canPayFor;
    }

    public bool UsesUniversalStoreSoundFX()
    {
        return true;
    }

    public string GetInteractText()
    {
        return $"Press [E] to purchase trap: {price}";
    }

}
