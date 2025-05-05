using UnityEngine;
using UnityEngine.EventSystems;

public class StartButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject scareImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        scareImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scareImage.SetActive(false);
    }
}
