using UnityEngine;
using System.Collections.Generic;

public class BarricadeController : MonoBehaviour
{
    public List<GameObject> boards;
    private int currentHits = 0;
    public bool isBreached = false;

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
            boards[currentHits].SetActive(false);
            currentHits++;
        }

        if (currentHits == boards.Count)
        {
            isBreached = true;
            Debug.Log("Barricade breached!");
            //sounds/animations
        }
    }

    public bool RepairOneBoard()
    {
        if (currentHits <= 0 || currentHits > boards.Count)
            return false;

        currentHits--;
        boards[currentHits].SetActive(true);

        if (currentHits == 0)
            isBreached = false;

        return true; // Successfully repaired one board
    }

}
