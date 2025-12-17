using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;
public class MainMenuManager : MonoBehaviour
{
   public TextMeshProUGUI recordText;
    void Start()
    {
        int bestKills = PlayerPrefs.GetInt("ZombieRecord", 0);

        if (recordText != null)
        {
            recordText.text = "KILL RECORD: " + bestKills;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void QuitGame()
    {
        string logPath = Path.Combine(Application.dataPath + "/../", "Game_History_Log.txt");
        System.IO.File.AppendAllText(logPath, "[" + System.DateTime.Now.ToString("HH:mm:ss") + "] The player exited the game through the menu.\n");

        Debug.Log("The game is closed");
        Application.Quit();
    }
}