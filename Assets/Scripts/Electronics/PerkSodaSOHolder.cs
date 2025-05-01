using UnityEngine;

public class PerkSodaSOHolder : MonoBehaviour
{

    [SerializeField] private PerkSodasSO heldPerkSodaSO;
    public PerkSodasSO GetHeldPerkSodaSO()
    {
        return heldPerkSodaSO;
    }
}
