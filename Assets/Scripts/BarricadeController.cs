using UnityEngine;

public class BarricadeController : MonoBehaviour
{
    public GameObject[] boards;
    private int currentHits = 0;
    public bool isBreached = false;

    private void Start()
    {
        foreach (GameObject board in boards) //all boards visible
            board.SetActive(true);

        currentHits = 0;
        isBreached = false;
    }

    public void RegisterHit()
    {
        if (isBreached) return;

        if (currentHits < boards.Length)
        {
            boards[currentHits].SetActive(false);
            currentHits++;
        }

        if (currentHits == boards.Length)
        {
            isBreached = true;
            Debug.Log("Barricade breached!");
            //sounds/animations
        }
    }

    public bool RepairOneBoard()
    {
        if (currentHits <= 0 || currentHits > boards.Length)
            return false;

        currentHits--;
        boards[currentHits].SetActive(true);

        if (currentHits == 0)
            isBreached = false;

        return true; // Successfully repaired one board
    }

}
