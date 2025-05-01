using UnityEngine;

public class PlayerRepair : MonoBehaviour
{
    public float repairRange = 2f;
    public KeyCode repairKey = KeyCode.E; //E to repair
    public float repairDelay = 0.5f; //time between each board

    private bool isRepairing = false;
    private float repairTimer = 0f; //countdown between board repairs  
    private BarricadeController targetBarricade; //focused barricade to repair

    void Update()
    {
        CheckForWindow();

        if (targetBarricade != null && Input.GetKey(repairKey))
        {
            if (!isRepairing)
            {
                isRepairing = true;
                repairTimer = repairDelay; //resets timer for first repair
            }
            
            //decreases timer as key is held
            repairTimer -= Time.deltaTime;

            
            if (repairTimer <= 0f)
            {
                bool repaired = targetBarricade.RepairOneBoard();
                if (!repaired)
                {
                    //fully repaired
                    isRepairing = false;
                }
                repairTimer = repairDelay; //resets timer for next board
            }
        }
        else
        {
            //key is not held or player moves away
            isRepairing = false;
        }
    }

    void CheckForWindow()
    {
        //looks for all colliders in a sphere around the player in repairRange
        Collider[] hits = Physics.OverlapSphere(transform.position, repairRange);
        //clear any previous focus target
        targetBarricade = null;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Window"))
            {
                //barricadeController from parent
                BarricadeController bc = hit.GetComponentInParent<BarricadeController>();
                //only breached windows are valid targets
                if (bc != null && bc.isBreached)
                {
                    targetBarricade = bc;
                    return;
                }
            }
        }
    }
}
