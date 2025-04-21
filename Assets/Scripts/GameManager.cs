using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;
//handles Game logic

public class GameManager : MonoBehaviour
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

    private void Awake()
    {

    }
    private void Start()
    {
        //listen to the interact lookat event
        PlayerMelee.Instance.OnRayCastHitInteract += PlayerMelee_OnRayCastHitInteract;
    }

    public void HandleLookAtInteractType(GameObject item)
    {
        //this method checks type of object by layer then calls respective purchaseHandler
        if (item.layer == 6)
        {
            //perks
            HandlePerkPurchase(item);
        }
        else if (item.layer == 7)
        {
            //wallbuy
        }

    }

    private void PlayerMelee_OnRayCastHitInteract(object sender, PlayerMelee.RayCastHitInteract e)
    {
        HandleLookAtInteractType(e.lookAtInteract);
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
            //Debug.Log("should work?!");
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
                PlayerMelee.Instance.SetPlayerHP((int)(perkSoda.statModifiers[0].value += PlayerMelee.Instance.GetPlayerHP()));
                break;
            case PerkID.StaminUp:
                //PlayerMovement: speed +0.7%, stamina x2
                float tempSpeed= PlayerMovement.Instance.speed;
                tempSpeed += (float) (perkSoda.statModifiers[0].value * tempSpeed);
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
