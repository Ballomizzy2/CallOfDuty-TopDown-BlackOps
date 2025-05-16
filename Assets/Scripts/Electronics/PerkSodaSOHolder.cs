using System;
using UnityEngine;

public class PerkSodaSOHolder : MonoBehaviour,IInteract
{

    [SerializeField] private PerkSodasSO heldPerkSodaSO;
    //---INSTANCE CANNOT WORK HERE---
    //shop variables soundfx
    private bool canPayFor;
    private bool inList = false;
    public PerkSodasSO GetHeldPerkSodaSO()
    {
        return heldPerkSodaSO;
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
                //OnDoubleTapPurchase?.Invoke(this, EventArgs.Empty);
                PerkEventHub.DoubleTapPurchased();
                break;
            case PerkID.SpeedCola:
                //OnSpeedColaPurchase?.Invoke(this, EventArgs.Empty);
                PerkEventHub.SpeedColaPurchased();
                //access player's weapon manager->add a x2 variable->pass it into equipped Gun.cs line 138 ...(gunData.reloadTime/speedCola)
                break;
            case PerkID.MuleKick:
                //optional soda
                //make da array 3 in weaponManager :) 
                break;


        }

    }



    //IIinteract contract
    public bool IsElectrical()
    {
        return true;
    }
    public void Interact(PlayerController player)
    {

        int playerScore = player.currentPoints;
        if (!player.HasPerk(heldPerkSodaSO) && playerScore >= heldPerkSodaSO.price && player.GetPerkCount() < player.GetPerkLimit())
        {
            canPayFor = true;

            player.AddPerkCount();
            player.SetPoints(playerScore -= heldPerkSodaSO.price);
            player.playerPerkList.Add(heldPerkSodaSO);
            //call method to do handle stats...
            HandlePerkSodaModifierAllocation(heldPerkSodaSO);
            Debug.Log($"-{player.GetPoints()}, you got {heldPerkSodaSO.perkID}");
            HUDController.Instance.SetPerkIcons(player.playerPerkList);
            inList = true;

        }
        else
        {
            //canPayFor should default to false after calling noise
            canPayFor=false;
            //Debug.Log("oof...");
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
   
        if (!inList)
        {
            //if player owns: ""
            return $"Press [E] to purchase {heldPerkSodaSO.name}: {heldPerkSodaSO.price}";
        }
        else
        {
            return "";
        }
 
    }

}
