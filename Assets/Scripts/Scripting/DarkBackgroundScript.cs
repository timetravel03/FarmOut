using UnityEngine;
using UnityEngine.EventSystems;

public class DarkBackgroundScript : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalVariables.CursorOverClickableObject = false;
    }
}
