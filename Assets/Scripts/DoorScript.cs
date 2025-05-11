using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class DoorScript : MonoBehaviour
{
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
        UnityEngine.Cursor.SetCursor(bCursor, hotspot, cursorMode);
        if (Input.GetMouseButtonDown(1))
        {
            if (SceneManager.GetActiveScene().name == "Home")
            {
                SceneManager.LoadScene("SampleScene");
            }
            else
            {
                SceneManager.LoadScene("Home");
            }
            Debug.Log("Click en puerta");
        }
    }

    private void OnMouseExit()
    {
        UnityEngine.Cursor.SetCursor(yCursor, hotspot, cursorMode);

    }
}
