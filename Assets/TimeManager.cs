using UnityEngine;
using static PlayerController;

public class TimeManager : MonoBehaviour
{
    public Transform playerTransform;
    public SpriteRenderer spriteRenderer;

    private float deltaTimer;
    private readonly GUIStyle debugGuiStyle = new GUIStyle();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deltaTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position;
    }
    private void FixedUpdate()
    {
        if (deltaTimer < 20f)
        {
            deltaTimer += Time.deltaTime;
        }
        else 
        {
            deltaTimer = 0;
        }

        spriteRenderer.color = new Color(1f, 1f, 1f, deltaTimer / 40f);
    }

    private void OnGUI()
    {
        debugGuiStyle.fontSize = 12;
        debugGuiStyle.fontStyle = FontStyle.Bold;

        float x = 10;
        float y = 10;

        GUI.Label(new Rect(x, y + 90, 200, 50), $"Delta Timer: {deltaTimer.ToString()}");
    }
}
