using UnityEngine;
<<<<<<< HEAD
using System.Collections.Generic;

public class BarricadeController : MonoBehaviour
{
    public List<GameObject> boards;
=======

public class BarricadeController : MonoBehaviour
{
    public GameObject[] boards;
>>>>>>> origin/Lester_D
    private int currentHits = 0;
    public bool isBreached = false;

    private void Start()
    {
<<<<<<< HEAD
        for(int i = 0; i < transform.childCount; i++)
        {
            boards.Add(transform.GetChild(i).gameObject);
        } 
=======
        foreach (GameObject board in boards) //all boards visible
            board.SetActive(true);

>>>>>>> origin/Lester_D
        currentHits = 0;
        isBreached = false;
    }

    public void RegisterHit()
    {
        if (isBreached) return;

<<<<<<< HEAD
        if (currentHits < boards.Count)
=======
        if (currentHits < boards.Length)
>>>>>>> origin/Lester_D
        {
            boards[currentHits].SetActive(false);
            currentHits++;
        }

<<<<<<< HEAD
        if (currentHits == boards.Count)
=======
        if (currentHits == boards.Length)
>>>>>>> origin/Lester_D
        {
            isBreached = true;
            Debug.Log("Barricade breached!");
            //sounds/animations
        }
    }

    public bool RepairOneBoard()
    {
<<<<<<< HEAD
        if (currentHits <= 0 || currentHits > boards.Count)
=======
        if (currentHits <= 0 || currentHits > boards.Length)
>>>>>>> origin/Lester_D
            return false;

        currentHits--;
        boards[currentHits].SetActive(true);

        if (currentHits == 0)
            isBreached = false;

        return true; // Successfully repaired one board
    }

}
