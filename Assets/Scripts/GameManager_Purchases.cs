using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;
//handles Game logic

public class GameManager_Purchases : MonoBehaviour
{

    //player inv: perks, load out
    //map items: doors, wall guns, box, perks
    //logic for buying stuff?
    //logic for waves
    [SerializeField] private List<PerkSodasSO> mapPerkSodas;

    ///player var
    private int perkCount = 0;
    private int perkMax = 2;
    public int playerScore;

    [SerializeField] List<PerkSodasSO> playerPerkList;
    private const int PERK_LAYER = 6;
    private const int WALLBUY_LAYER = 7;
    private const int DOOR_LAYER = 8;
    private const int BOX_LAYER = 9;

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
        //unlike ray case, go stright to filter bc i didnt think of it then....
        switch (e.overLapHit.layer)
        {
            case DOOR_LAYER:
                HandleDoorPurchase(e.overLapHit);
                break;
            case BOX_LAYER:
                break;

        }
    }

    private void PlayerMelee_OnRayCastHitInteract(object sender, PlayerController.RayCastHitInteract e)
    {
        switch (e.lookAtInteract.layer)
        {
            case PERK_LAYER:
                HandlePerkPurchase(e.lookAtInteract); //pass the game object
                break;
            case WALLBUY_LAYER:
                //wall buy
                break;
        }
    }
    private void HandleDoorPurchase(GameObject item)
    {
        
        int tempPrice= item.GetComponent<DoorsHandler>().GetPrice();
        if(playerScore>=tempPrice)
        {
            playerScore-=tempPrice;
            
        }
        
    }

    private void HandlePerkPurchase(GameObject item)
    {
        PerkSodasSO tempPerkSO = item.GetComponent<PerkSodaSOHolder>().GetHeldPerkSodaSO();
        if (!HasPerk(tempPerkSO) && playerScore >= tempPerkSO.price && perkCount < perkMax)
        {
            perkCount++;
            playerScore -= tempPerkSO.price;
            playerPerkList.Add(tempPerkSO);
            //call method to do handle stats...
            HandlePerkSodaModifierAllocation(tempPerkSO);
            Debug.Log($"-{playerScore}, you got {tempPerkSO.perkID}");
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
                break;
            case PerkID.SpeedCola:
                break;
            case PerkID.MuleKick:
                break;


        }

    }
}
