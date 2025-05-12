using System;
using System.Globalization;
using UnityEngine;
using static PlayerController;

public class TimeManager : MonoBehaviour
{
    public static event Action OnCycleComplete;
    public Transform playerTransform;
    public SpriteRenderer spriteRenderer;
    public bool DayTime { get { return daytime; } }
    public int DayCounter { get { return daycounter; } }

    private float deltaTimer;
    private bool reverse;
    private bool daytime;
    private int daycounter;

    private readonly GUIStyle debugGuiStyle = new GUIStyle();

    void Start()
    {
        deltaTimer = 0;
        reverse = false;
    }

    void Update()
    {
        transform.position = playerTransform.position;
    }

    private void FixedUpdate()
    {
        DayTimer();
    }

    private void DayTimer()
    {
        if (deltaTimer >= 124f)
        {
            reverse = true;
        }

        if (deltaTimer <= 0f)
        {
            reverse = false;
            OnCycleComplete?.Invoke();
        }

        if (reverse)
        {
            deltaTimer -= Time.deltaTime;
        }
        else
        {
            deltaTimer += Time.deltaTime;
        }

        daytime = deltaTimer <= 12f;

        spriteRenderer.color = new Color(1f, 1f, 1f, deltaTimer / 140f);
    }

    private void OnGUI()
    {
        debugGuiStyle.fontSize = 12;
        debugGuiStyle.fontStyle = FontStyle.Bold;

        float x = 10;
        float y = 10;

        GUI.Label(new Rect(x, y + 90, 200, 50), $"Day Timer: {deltaTimer.ToString()}");
        GUI.Label(new Rect(x, y + 105, 200, 50), $"DayTime: {DayTime.ToString()}");
    }
}
