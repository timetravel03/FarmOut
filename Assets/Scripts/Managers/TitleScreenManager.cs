using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public Text noSaveMessage;
    public Text deleteSaveMessage;

    private bool newSaveWarning = false;
    public void StartGame()
    {
        if (GlobalVariables.SaveFileExists() && !newSaveWarning)
        {
            deleteSaveMessage.gameObject.SetActive(true);
            newSaveWarning = true;
        }
        else
        {
            GlobalVariables.ResumeGame = false;
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    public void LoadGame()
    {
        if (GlobalVariables.SaveFileExists())
        {
            GlobalVariables.ResumeGame = true;
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            noSaveMessage.gameObject.SetActive(true);
        }
    }
}
