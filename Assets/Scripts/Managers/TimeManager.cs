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

    private static float deltaTimer = 1f;
    private bool reverse = false;
    private bool daytime;
    private int daycounter;

    private readonly GUIStyle debugGuiStyle = new GUIStyle();

    private float dayLenght = 64f;
    private bool goToSleep;

    private void Start()
    {
        DoorScript.DoorEvent += ToggleSleep;
    }

    void Update()
    {
        transform.position = playerTransform.position;
    }

    private void FixedUpdate()
    {
        if (goToSleep)
        {
            FadeToBlack();
        }
        else
        {
            DayTimer();
        }
    }

    private void DayTimer()
    {
        if (deltaTimer >= dayLenght - 10f)
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

        spriteRenderer.color = new Color(1f, 1f, 1f, deltaTimer / dayLenght);
    }

    //private void OnGUI()
    //{
    //    debugGuiStyle.fontSize = 12;
    //    debugGuiStyle.fontStyle = FontStyle.Bold;

    //    float x = 10;
    //    float y = 10;

    //    GUI.Label(new Rect(x, y + 90, 200, 50), $"Day Timer: {deltaTimer.ToString()}");
    //    GUI.Label(new Rect(x, y + 105, 200, 50), $"DayTime: {DayTime.ToString()}");
    //}

    public static void RestartDay()
    {
        deltaTimer = 0f;
        GlobalVariables.LockPlayerMovement = false;
        PlayerController.LockFireEvent = false;
    }

    private void FadeToBlack()
    {
        GlobalVariables.LockPlayerMovement = true;
        spriteRenderer.color = new Color(1f, 1f, 1f, deltaTimer / dayLenght);

        if (deltaTimer >= dayLenght)
        {
            goToSleep = false;
            RestartDay();
        }
        else if (deltaTimer < dayLenght)
        {
            deltaTimer += Time.deltaTime * (dayLenght/2f);
        }
    }

    private void ToggleSleep()
    {
        goToSleep = true;
    }
}
