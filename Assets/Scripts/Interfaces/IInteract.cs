using TMPro;
using UnityEngine;

public interface IInteract 
{
    public bool IsElectrical();
    public void Interact(PlayerController player);

    public bool CanAffordSoundFX();

    //add a bool for universal noise, other wise objects will handle their own noise?
    public bool UsesUniversalStoreSoundFX();

    public string GetInteractText();

}
