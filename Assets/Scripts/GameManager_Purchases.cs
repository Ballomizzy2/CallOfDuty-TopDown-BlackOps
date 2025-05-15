using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;
using System;
//handles Game point spending logic

public class GameManager_Purchases : MonoBehaviour
{

    //map items: doors, wall guns, box, perks
    //logic for buying stuff?
    public static GameManager_Purchases Instance {  get; private set; }
    public event EventHandler OnSpeedColaPurchase;
    public event EventHandler OnDoubleTapPurchase;
    [SerializeField] private List<PerkSodasSO> mapPerkSodas; //doesn't do anything
    [Header("HUD")]
    [SerializeField] private HUDController hudControllerObject; //for perks

    ///player var
    private int perkCount = 0;
    private int perkMax = 2;
    

    [SerializeField] List<PerkSodasSO> playerPerkList; //move this to PlayerController ahhhhhhhhh
    private const int PERK_LAYER = 6;
    private const int WALLBUY_LAYER = 7;
    private const int DOOR_LAYER = 8;
    private const int BOX_LAYER = 9;
    private const int POWER_LAYER = 11;
    private const int PACK_A_PUNCH_LAYER = 12;
    private const int TRAP_SWITCH_LAYER = 13;

    [SerializeField] bool powerOn = false;
    //[SerializeField] bool fireSaleActive; //
    private bool canPayFor = false;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //listen to the interact lookat event
        PlayerController.Instance.OnRayCastHitInteract += PlayerController_OnRayCastHitInteract;
        PlayerController.Instance.OnOverLapHitInteract += PlayerController_OnOverLapHitInteract;
        PowerSwitchController.Instance.OnLeverFlipped += PowerSwitchController_OnLeverFlipped;
    }

    private void PowerSwitchController_OnLeverFlipped(object sender, System.EventArgs e)
    {
        powerOn = true;
        Debug.Log("ZZZZZTT!");
    }

    private void PlayerController_OnOverLapHitInteract(object sender, PlayerController.OverLapHitInteract e)
    {
        PlayerController player = sender as PlayerController;
        int playerScore = player.GetPoints();
        IInteract recepiant = e.overLapHit.GetComponent<IInteract>();
        switch (e.overLapHit.layer)
        {
            case DOOR_LAYER:
                //pass the object hit, the players score, the player object for access to additional methods if needed
                recepiant.Interact(player);
                CanAfford_sound(recepiant.CanAffordSoundFX());
                break;
            case BOX_LAYER:
                HandleMysterBoxPurchase(e.overLapHit,playerScore,player);
                break;


        }
    }

    private void PlayerController_OnRayCastHitInteract(object sender, PlayerController.RayCastHitInteract e)
    {
        PlayerController player = sender as PlayerController;
        int playerScore = player.GetPoints();
        IInteract recepiant= e.lookAtInteract.GetComponent<IInteract>();  
        switch (e.lookAtInteract.layer)
        {
            case PERK_LAYER:
                if (powerOn)
                {
                    HandlePerkPurchase(e.lookAtInteract, playerScore, player); //pass the game object
                }
                else
                {
                    CanAfford_sound(canPayFor);
                    Debug.Log("NO POWWWWER!");
                }

                    break;
            case WALLBUY_LAYER:
                HandleWallBuyPurchase(e.lookAtInteract, playerScore, player);
                
                break;
            case DOOR_LAYER:
                //HandleDoorPurchase(e.lookAtInteract, playerScore, player);
                recepiant.Interact(player);
                CanAfford_sound(recepiant.CanAffordSoundFX());
                break;
            case PACK_A_PUNCH_LAYER:
                //todo make a pack A punch
                break;
            case TRAP_SWITCH_LAYER:
                if (powerOn)
                {

                    recepiant.Interact(player);
                    CanAfford_sound(recepiant.CanAffordSoundFX());
                }
                else
                {
                    CanAfford_sound(recepiant.CanAffordSoundFX());
                    Debug.Log("NO POWWWWER!");
                }


                break;
            case POWER_LAYER:

                recepiant.Interact(player);

                break;
        }
    }

    private void HandleWallBuyPurchase(GameObject item, int playerScore, PlayerController player)
    {
        //TODO edit gun SO
       //edit weapon SO to have a price?
       //int tempPrice= item.GetComponent<GunSOHolder>.GetHeldGun();
       //do same thing as perks
       item.GetComponent<WallBuy>().AttemptPurchase();

    }
    private void HandleMysterBoxPurchase(GameObject item,int playerScore,PlayerController player)
    {
        //HANDLED INSIDE MYSTERBOX PREFAB RN
        int mysteryBoxPrice = 950;
       
        
        if(playerScore >= mysteryBoxPrice)
        {
            //mysterybox should have methods for all these. e.g item.RollGuns();
            //put all gun SO in an array that put it in random 
            //spawn gun(with collider), hide box collider, move gun down for x sec, then close box
            //if player interacts w/ gun, equip and close box
            //canPayFor = true;
            //CanAfford_sound(canPayFor);

        }
        else
        {
            //CanAfford_sound(canPayFor);

        }

    }




    //perk stuff
    private void HandlePerkPurchase(GameObject item, int playerScore, PlayerController player)
    {
        PerkSodasSO tempPerkSO = item.GetComponent<PerkSodaSOHolder>().GetHeldPerkSodaSO();
        if (!HasPerk(tempPerkSO) && playerScore >= tempPerkSO.price && perkCount < perkMax)
        {
            canPayFor = true;
            CanAfford_sound(canPayFor);
            perkCount++;
            player.SetPoints(playerScore -= tempPerkSO.price);
            playerPerkList.Add(tempPerkSO);//move this to playerController later
            //call method to do handle stats...
            HandlePerkSodaModifierAllocation(tempPerkSO);
            Debug.Log($"-{player.GetPoints()}, you got {tempPerkSO.perkID}");
            //for (int i = 0; i < tempPerkSO.statModifiers.Count; i++)
            //{
            //    Debug.Log($"stat affected:{tempPerkSO.statModifiers[i].statType}\n" +
            //        $" +{tempPerkSO.statModifiers[i].valType} {tempPerkSO.statModifiers[i].value} ");
            //}
        }
        else
        {
            //canPayFor should default to false after calling noise
            CanAfford_sound(canPayFor);
            //Debug.Log("oof...");
        }

    }
    private bool HasPerk(PerkSodasSO perkSoda)
    {
        //iterate thu player list to see if they have said perk
        for (int i = 0; i < playerPerkList.Count; i++)
        {
            if (perkSoda == playerPerkList[i])
            {
                return true;
            }
        }
        return false;
    }

    private void HandlePerkSodaModifierAllocation(PerkSodasSO perkSoda)
    {
        //in here, setActive respective perk from HUD thingy

        switch (perkSoda.perkID)
        {
            case PerkID.Juggernog:
                PlayerController.Instance.SetPlayerHP((int)(perkSoda.statModifiers[0].value + PlayerController.Instance.GetPlayerHP()));
                break;
            case PerkID.StaminUp:
                //PlayerMovement: speed +0.7%, stamina x2
                float tempSpeed = PlayerMovement.Instance.speed;
                tempSpeed += (float)(perkSoda.statModifiers[0].value * tempSpeed);
                PlayerMovement.Instance.speed = tempSpeed;

                float tempStamina = PlayerMovement.Instance.maxStamina;
                tempStamina = (float)(perkSoda.statModifiers[1].value * tempStamina);
                PlayerMovement.Instance.maxStamina = tempStamina;
                break;
            case PerkID.DoubleTap:
                //call an event to gun, first line in Fire() adjust the gun.fireRate delay, like speedCola
                OnDoubleTapPurchase?.Invoke(this, EventArgs.Empty);
                break;
            case PerkID.SpeedCola:
                OnSpeedColaPurchase?.Invoke(this,EventArgs.Empty); 
                //access player's weapon manager->add a x2 variable->pass it into equipped Gun.cs line 138 ...(gunData.reloadTime/speedCola)
                break;
            case PerkID.MuleKick:
                //optional soda
                //make da array 3 in weaponManager :) 
                break;


        }

    }
    private void CanAfford_sound(bool canAfford)
    {
        if (canAfford)
        {
            SoundMng.Instance.PlayBuySound();
        }
        else
        {
            SoundMng.Instance.PlayDeniedSound();
        }
        //set to false
        canPayFor = false;
    }
    private void OnDestroy()
    {
        //unsubscribe
        PlayerController.Instance.OnRayCastHitInteract -= PlayerController_OnRayCastHitInteract;
        PlayerController.Instance.OnOverLapHitInteract -= PlayerController_OnOverLapHitInteract;
        PowerSwitchController.Instance.OnLeverFlipped -= PowerSwitchController_OnLeverFlipped;
    }
}
