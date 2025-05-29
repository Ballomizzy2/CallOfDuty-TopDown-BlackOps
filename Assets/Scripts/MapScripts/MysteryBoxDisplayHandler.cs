using UnityEngine;
/// <summary>
/// this script makes it easier to access the children of this prefab
/// when box spawns in room call enableBox
/// when box leaves room call disableBox
/// 
/// </summary>

public class MysteryBoxDisplayHandler : MonoBehaviour
{
    [SerializeField] private GameObject toy;
    [SerializeField] private GameObject box;

    public void EnableBox()
    {
        box.SetActive(true);
        toy.SetActive(false);
    }
    public void DisableBox() { box.SetActive(false); toy.SetActive(true); }
    public bool BoxStatus()
    {
        return box.activeSelf;
    }
}
