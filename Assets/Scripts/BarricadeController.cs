using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// ----two birds one stone---
/// IF zombie, trigger should change state to "break barrier"
///     -zombie should pass once barriers are down
/// player will use this for its Interact via raycast
///     -player CANNOT pass no matter what
/// </summary>

public class BarricadeController : MonoBehaviour, IInteract
{
    public List<GameObject> boards;
    private int currentHits = 0;
    public bool isBreached = false;

    //shop variables soundfx
    private bool canPayFor = false;

    [Header("Debug")]
    [SerializeField] private bool debug_playerIsReparing = true;
    private void Start()
    {
        //if (boards == null || boards.Count == 0)
        //{
        //    boards = new List<GameObject>();
        //    for (int i = 0; i < transform.childCount; i++)
        //    {
        //        boards.Add(transform.GetChild(i).gameObject);
        //    }
        //}
        Debug.Log(boards.Count);
        currentHits = 0;
        isBreached = false;
    }

    public void RegisterHit()
    {
        if (isBreached)
        {
            //Debug.Log("Tried to hit, but already breached.");
            return;
        }

        //Debug.Log($"RegisterHit called. currentHits = {currentHits}, boards.Count = {boards.Count}");

        if (currentHits < boards.Count)
        {
            canPayFor = true;
            boards[currentHits].SetActive(false);
            currentHits++;
            //Debug.Log($"Board {currentHits} disabled.");
        }

        if (currentHits == boards.Count)
        {
            canPayFor = false;
            isBreached = true;
            //Debug.Log("Barricade breached!");
        }
    }

    public bool RepairOneBoard()
    {
        //player calls
        if (currentHits <= 0 || currentHits > boards.Count)
        {
            canPayFor = false;
            return false;
        }
            

        currentHits--;
        boards[currentHits].SetActive(true);

        if (currentHits == 0)
            isBreached = false;
        canPayFor = true;
        GameManager_Scores.Instance.PointsPerBarrier();
        return true; // Successfully repaired one board
    }
    public bool RepairAllBoards()
    {
        //carpenter calls
        if (!isBreached) return false;

        isBreached=false;

        currentHits=0;
        //boards[currentHits].SetActive(true);
        foreach (var board in boards)
        {
            board.SetActive(true);
        }
        

        return true; // Successfully repaired one board
    }

    //IInteract contract
    public bool IsElectrical()
    {
        return false;
    }
    public void Interact(PlayerController player)
    {
        if (debug_playerIsReparing)
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
        if (debug_playerIsReparing)
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
        if (debug_playerIsReparing)
        {
            if (canPayFor)
            {
                return $"Press [E] to repair barrier";
            }
            else
            {
                return "";
            }
            
        }
        else
        {
            return $"Press [E] to damage barrier";
        }
        
    }

}
