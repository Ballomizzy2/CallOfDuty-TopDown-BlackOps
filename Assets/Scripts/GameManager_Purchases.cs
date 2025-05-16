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
    public static GameManager_Purchases Instance { get; private set; }
    [Header("HUD")]
    [SerializeField] private HUDController hudControllerObject; //for perks


    private const int PERK_LAYER = 6;
    private const int WALLBUY_LAYER = 7;
    private const int DOOR_LAYER = 8;
    private const int MYSTERY_BOX_LAYER = 10;
    private const int POWER_LAYER = 11;
    private const int PACK_A_PUNCH_LAYER = 12;
    private const int TRAP_SWITCH_LAYER = 13;

    [SerializeField] private bool powerOn = false;
    //[SerializeField] bool fireSaleActive; //

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
        IInteract target = e.overLapHit.GetComponent<IInteract>();

        if (target == null) return;

        switch (e.overLapHit.layer)
        {
            case DOOR_LAYER:
            case MYSTERY_BOX_LAYER:
                break; // Allowed
            default:
                return;
        }

        bool needsPower = target.IsElectrical();
        bool canUse = !needsPower || powerOn;

        if (canUse)
        {
            //edit this later for objects that play a noise once...
            target.Interact(player);
            CanAfford_sound(target.CanAffordSoundFX());//plays success sound
            
        }
        else
        {
            CanAfford_sound(target.CanAffordSoundFX());//plays denied sound
            Debug.Log("NO POWWWWER!");
        }
    }

    private void PlayerController_OnRayCastHitInteract(object sender, PlayerController.RayCastHitInteract e)
    {
        PlayerController player = sender as PlayerController;
        IInteract target = e.lookAtInteract.GetComponent<IInteract>();

        if (target == null) return;

        // Only allow raycast to interact with these layers
        switch (e.lookAtInteract.layer)
        {
            case PERK_LAYER:
            case WALLBUY_LAYER:
            case DOOR_LAYER:
            case TRAP_SWITCH_LAYER:
            case POWER_LAYER:
            case PACK_A_PUNCH_LAYER:
            case MYSTERY_BOX_LAYER:
                break; // Allowed
            default:
                return; // Block anything else
        }

        bool needsPower = target.IsElectrical();
        bool canUse = !needsPower || powerOn;

        if (canUse)
        {
            target.Interact(player);
            if(target.UsesUniversalStoreSoundFX())
            {
                CanAfford_sound(target.CanAffordSoundFX());
            }
           
            
            
        }
        else
        {
            if (target.UsesUniversalStoreSoundFX())
            {
                CanAfford_sound(target.CanAffordSoundFX());
            }
            Debug.Log("NO POWWWWER!");
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
   
    }
    public bool GetPowerStatus()
    {
        return powerOn;
    }
    private void OnDestroy()
    {
        //unsubscribe
        PlayerController.Instance.OnRayCastHitInteract -= PlayerController_OnRayCastHitInteract;
        PlayerController.Instance.OnOverLapHitInteract -= PlayerController_OnOverLapHitInteract;
        PowerSwitchController.Instance.OnLeverFlipped -= PowerSwitchController_OnLeverFlipped;
    }
}
