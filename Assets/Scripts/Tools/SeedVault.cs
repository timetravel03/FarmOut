using UnityEngine;
using UnityEngine.UI;

public class SeedVault : MonoBehaviour
{
    public Item item;
    public SpriteRenderer itemImage;

    public Texture2D bCursor;
    public Texture2D yCursor;
    Vector2 hotspot = new Vector2(0, 0);
    CursorMode cursorMode = CursorMode.Auto;

    private void Start()
    {
        itemImage.sprite = item.image;
    }
    private void OnMouseOver()
    {
        if (CloseEnough() && !InventoryManager.instance.mainInventory.gameObject.activeSelf)
        {
            GlobalVariables.CursorOverClickableObject = true;
            UnityEngine.Cursor.SetCursor(bCursor, hotspot, cursorMode);
            if (Input.GetMouseButtonDown(1))
            {
                SoundManager.Instance.Play("Chest");
                for (int i = 0; i < 8; i++)
                {
                    InventoryManager.instance.AddItem(item);
                }
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

    private void Update()
    {

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
