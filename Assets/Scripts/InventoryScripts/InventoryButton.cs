using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.Play("Chest2");
        GlobalVariables.CursorOverClickableObject = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GlobalVariables.CursorOverClickableObject = true;
    }
}
