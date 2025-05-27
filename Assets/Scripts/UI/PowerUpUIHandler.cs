using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// this script is held by UI to identify image for disabling/enabling
/// </summary>
public class PowerUpUIHandler : MonoBehaviour
{
   
    [SerializeField] private PowerUpType powerUpUIType;//to use for removing icon, set in respective inspector

    public PowerUpType GetPowerUpType()
    {
        return powerUpUIType;
    }

    
}
