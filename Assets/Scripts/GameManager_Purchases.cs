using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;
//handles Game logic

public class GameManager_Purchases : MonoBehaviour
{

    //map items: doors, wall guns, box, perks
    //logic for buying stuff?
    [SerializeField] private List<PerkSodasSO> mapPerkSodas; //doesn't do anything

    ///player var
    private int perkCount = 0;
    private int perkMax = 2;
    

    [SerializeField] List<PerkSodasSO> playerPerkList; //move this to PlayerController ahhhhhhhhh
    private const int PERK_LAYER = 6;
    private const int WALLBUY_LAYER = 7;
    private const int DOOR_LAYER = 8;
    private const int BOX_LAYER = 9;
    [SerializeField] bool powerOn = true; //when Power made, default to false and listen to an Event flip
    //[SerializeField] bool fireSaleActive; //

    private void Awake()
    {

    }
    private void Start()
    {
        //listen to the interact lookat event
        PlayerController.Instance.OnRayCastHitInteract += PlayerMelee_OnRayCastHitInteract;
        PlayerController.Instance.OnOverLapHitInteract += PlayerMelee_OnOverLapHitInteract;
    }

    private void PlayerMelee_OnOverLapHitInteract(object sender, PlayerController.OverLapHitInteract e)
    {
        PlayerController player = sender as PlayerController;
        int playerScore = player.GetPoints();
        switch (e.overLapHit.layer)
        {
            case DOOR_LAYER:
                //pass the object hit, the players score, the player object for access to additional methods if needed
                HandleDoorPurchase(e.overLapHit, playerScore,player);
                break;
            case BOX_LAYER:
                HandleMysterBoxPurchase(e.overLapHit,playerScore,player);
                break;

        }
    }

    private void PlayerMelee_OnRayCastHitInteract(object sender, PlayerController.RayCastHitInteract e)
    {
        PlayerController player = sender as PlayerController;
        int playerScore = player.GetPoints();
        switch (e.lookAtInteract.layer)
        {
            case PERK_LAYER:
                if (powerOn)
                {
                    HandlePerkPurchase(e.lookAtInteract, playerScore, player); //pass the game object
                }
                else
                {
                    Debug.Log("NO POWWWWER!");
                }

                    break;
            case WALLBUY_LAYER:
                HandleWallBuyPurchase(e.lookAtInteract, playerScore, player);
                
                break;
            case DOOR_LAYER:
                HandleDoorPurchase(e.lookAtInteract, playerScore, player);
                break;
        }
    }
    private void HandleDoorPurchase(GameObject item,int playerScore, PlayerController player)
    {
        //already have access to door, just open it here :]
        
        int tempPrice= item.GetComponent<DoorsHandler>().GetPrice();
        if(playerScore >= tempPrice)
        {
           player.SetPoints(playerScore-tempPrice);
            item.GetComponent<BoxCollider>().enabled = false;
            item.SetActive(false);//todo have the doors do the comment in DoorsHandler
            
        }
        
    }
    private void HandleWallBuyPurchase(GameObject item, int playerScore, PlayerController player)
    {
        //TODO edit gun SO
       //edit weapon SO to have a price?
       //int tempPrice= item.GetComponent<GunSOHolder>.GetHeldGun();
       //do same thing as perks

    }
    private void HandleMysterBoxPurchase(GameObject item,int playerScore,PlayerController player)
    {
        //TODO make MysteryBox to finish this
        int mysteryBoxPrice = 1500;
        
        if(playerScore >= mysteryBoxPrice)
        {
            //mysterybox should have methods for all these. e.g item.RollGuns();
            //put all gun SO in an array that put it in random 
            //spawn gun(with collider), hide box collider, move gun down for x sec, then close box
            //if player interacts w/ gun, equip and close box
        }
        else
        {
            Debug.Log("No monies...");
        }

    }

    private void HandlePerkPurchase(GameObject item, int playerScore, PlayerController player)
    {
        PerkSodasSO tempPerkSO = item.GetComponent<PerkSodaSOHolder>().GetHeldPerkSodaSO();
        if (!HasPerk(tempPerkSO) && playerScore >= tempPerkSO.price && perkCount < perkMax)
        {
            perkCount++;
            player.SetPoints(playerScore -= tempPerkSO.price);
            playerPerkList.Add(tempPerkSO);//move this to playerController later
            //call method to do handle stats...
            HandlePerkSodaModifierAllocation(tempPerkSO);
            Debug.Log($"-{player.GetPoints()}, you got {tempPerkSO.perkID}");
            for (int i = 0; i < tempPerkSO.statModifiers.Count; i++)
            {
                Debug.Log($"stat affected:{tempPerkSO.statModifiers[i].statType}\n" +
                    $" +{tempPerkSO.statModifiers[i].valType} {tempPerkSO.statModifiers[i].value} ");
            }
        }
        else
        {
            Debug.Log("oof...");
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
                //TODO edit Gun.cs to finish rest
                //access player's weapon manager-> add an increase fire rate variable-> pass into equipped Gun.cs
                break;
            case PerkID.SpeedCola:
                //access player's weapon manager->add a x2 variable->pass it into equipped Gun.cs line 138 ...(gunData.reloadTime/speedCola)
                break;
            case PerkID.MuleKick:
                //optional soda
                //make da array 3 in weaponManager :) 
                break;


        }

    }
}
