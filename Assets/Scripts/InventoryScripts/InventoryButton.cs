using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GlobalVariables.CursorOverClickableObject = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GlobalVariables.CursorOverClickableObject = false;
    }
}
