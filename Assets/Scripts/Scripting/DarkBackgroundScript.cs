using UnityEngine;
using UnityEngine.EventSystems;

public class DarkBackgroundScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalVariables.CursorOverClickableObject = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GlobalVariables.CursorOverClickableObject = true;
    }
}
