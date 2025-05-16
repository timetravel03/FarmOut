using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    //para bloquear el y liberar movimiento
    public PlayerController playerController;
    public Canvas fishingUI;
    public Image fishingPanel;
    public Image fishCage;
    public Image fish;
    public Image imageButton;

    private Button button;
    private bool started;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = imageButton.GetComponent<Button>();
    }
    private void Awake()
    {
        StartMinigame();
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            FallDown();
        }
    }

    void StartMinigame()
    {
        playerController.LockMovement();
        PlayerController.LockFireEvent = true;
        fishingUI.gameObject.SetActive(true);
        started = true;
    }

    void FallDown()
    {
        fishCage.transform.position -= new Vector3(0, -1);
    }

    public void ReelIn() { }
}
