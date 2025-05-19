using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class DoorScript : MonoBehaviour
{
    public static event Action DoorEvent;
    public Texture2D bCursor;
    public Texture2D yCursor;
    Vector2 hotspot = new Vector2(0, 0);
    CursorMode cursorMode = CursorMode.Auto;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnityEngine.Cursor.SetCursor(yCursor, hotspot, cursorMode);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        // como solo responde a click izquierdo hay que hacer una detección manual

    }

    private void OnMouseOver()
    {
        if (CloseEnough() && !InventoryManager.instance.mainInventory.gameObject.activeSelf)
        {
            GlobalVariables.CursorOverClickableObject = true;
            UnityEngine.Cursor.SetCursor(bCursor, hotspot, cursorMode);
            if (Input.GetMouseButtonDown(1))
            {
                DoorEvent?.Invoke();
                GlobalVariables.GoToSleep = true;
                Debug.Log("Click en puerta");
                SoundManager.Instance.Play("Door", .3f);
            }
        }
    }

    private void OnMouseExit()
    {
        if (!InventoryManager.instance.mainInventory.gameObject.activeSelf)
        {
            GlobalVariables.CursorOverClickableObject = false;
            UnityEngine.Cursor.SetCursor(yCursor, hotspot, cursorMode);
        }
    }

    private bool CloseEnough()
    {
        Vector3 distance = PlayerController.instance.transform.position - transform.position;
        if (distance.magnitude < .5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
