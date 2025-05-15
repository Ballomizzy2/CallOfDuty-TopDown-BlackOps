using UnityEngine;

public interface IInteract 
{
    public bool IsElectrical();
    public void Interact(PlayerController player);
    public bool CanAfford();
}
