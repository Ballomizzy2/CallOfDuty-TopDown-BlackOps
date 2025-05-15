using System;
using UnityEngine;

public class PowerSwitchController : MonoBehaviour, IInteract
{
    public static PowerSwitchController Instance { get; private set; }
    public event EventHandler OnLeverFlipped;
    [SerializeField] GameObject lever;//move to y 1.3

    private void Awake()
    {
        Instance = this;
    }
    public void ActivatePower()
    {
        //simple visual
        Vector3 newPos = lever.transform.localPosition;
        newPos.y = 1.3f;
        lever.transform.localPosition = newPos;

        OnLeverFlipped?.Invoke(this,EventArgs.Empty);
    }
    public bool IsElectrical()
    {
        return false;
    }
    public void Interact(PlayerController player)
    {
        ActivatePower();
    }
    public bool CanAffordSoundFX()
    {
        return true;
    }
}
