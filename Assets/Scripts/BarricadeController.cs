using UnityEngine;
using System.Collections.Generic;

public class BarricadeController : MonoBehaviour, IInteract
{
    public List<GameObject> boards;
    private int currentHits = 0;
    public bool isBreached = false;

    //shop variables soundfx
    private bool canPayFor = true;

    [Header("Debug")]
    [SerializeField] private bool playerIsReparing = true;
    private void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            boards.Add(transform.GetChild(i).gameObject);
        } 
        currentHits = 0;
        isBreached = false;
    }

    public void RegisterHit()
    {
        if (isBreached) return;

        if (currentHits < boards.Count)
        {
            canPayFor = true;
            boards[currentHits].SetActive(false);
            currentHits++;
        }

        if (currentHits == boards.Count)
        {
            canPayFor = false;
            isBreached = true;
            Debug.Log("Barricade breached!");
            //sounds/animations
        }
    }

    public bool RepairOneBoard()
    {
        if (currentHits <= 0 || currentHits > boards.Count)
        {
            canPayFor = false;
            return false;
        }
            

        currentHits--;
        boards[currentHits].SetActive(true);

        if (currentHits == 0)
            isBreached = false;
        GameManager_Scores.Instance.PointsPerBarrier();
        return true; // Successfully repaired one board
    }

    //IInteract contract
    public bool IsElectrical()
    {
        return false;
    }
    public void Interact(PlayerController player)
    {
        if (playerIsReparing)
        {
            RepairOneBoard();
        }
        else
        {
            RegisterHit();
        }
        
    }

    public bool CanAffordSoundFX()
    {
        if (playerIsReparing)
        {
            return canPayFor;
        }
        else
        {
            return false;
        }
        
    }

    public bool UsesUniversalStoreSoundFX()
    {
        return false;
    }

    public string GetInteractText()
    {
        if (playerIsReparing)
        {
            return $"Press [E] to purchase repair barrier";
        }
        else
        {
            return $"Press [E] to damage repair barrier";
        }
        
    }

}
