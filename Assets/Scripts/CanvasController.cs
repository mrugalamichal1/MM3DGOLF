using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    public GameObject ChampionshipCanvas;
    public GameObject SinglePlayerCanvas;
    public GameObject MenuCanvas;
    public GameObject LeaderboardCanvas;
    public GameObject HowToCanvas;
    public GameSessionController gameSessionController;
    public LeaderboardController leaderboardController;


    public void Awake()
    {
        ChampionshipCanvas = GameObject.Find("Championship_Canvas");
        SinglePlayerCanvas = GameObject.Find("Singleplayer_Canvas");
        MenuCanvas = GameObject.Find("Menu_Canvas");
        LeaderboardCanvas = GameObject.Find("Leaderboard_Canvas");
        HowToCanvas = GameObject.Find("HowTo_Canvas");
        gameSessionController = GameObject.FindObjectOfType<GameSessionController>();
        leaderboardController = GameObject.FindObjectOfType<LeaderboardController>();
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Menu_To_Leaderboard()
    {
        MenuCanvas.GetComponent<Canvas>().sortingOrder = 0;
        LeaderboardCanvas.GetComponent<Canvas>().sortingOrder = 1;
    }
    public void Leaderboard_To_Menu()
    {
        leaderboardController.DestroyLeaderboard();
        MenuCanvas.GetComponent<Canvas>().sortingOrder = 1;
        LeaderboardCanvas.GetComponent<Canvas>().sortingOrder = 0;

    }
    public void Menu_To_Singleplayer()
    {
        MenuCanvas.GetComponent<Canvas>().sortingOrder = 0;
        SinglePlayerCanvas.GetComponent<Canvas>().sortingOrder = 1;
        gameSessionController.GameMode = "sp";
    }
    public void Singleplayer_To_Menu()
    {
        GameObject.Find("Player_Name_sp").GetComponent<InputField>().text = "";
        SinglePlayerCanvas.GetComponent<Canvas>().sortingOrder = 0;
        MenuCanvas.GetComponent<Canvas>().sortingOrder = 1;
    }
    public void Championship_To_Menu()
    {
        GameObject.Find("Player1_Name_mp").GetComponent<InputField>().text = "";
        GameObject.Find("Player2_Name_mp").GetComponent<InputField>().text = "";
        ChampionshipCanvas.GetComponent<Canvas>().sortingOrder = 0;
        MenuCanvas.GetComponent<Canvas>().sortingOrder = 1;
    }
    public void Menu_to_Championship()
    {
        MenuCanvas.GetComponent<Canvas>().sortingOrder = 0;
        ChampionshipCanvas.GetComponent<Canvas>().sortingOrder = 1;
        gameSessionController.GameMode = "mp";
    }

    public void HowTo_To_Menu()
    {
        HowToCanvas.GetComponent<Canvas>().sortingOrder = 0;
        MenuCanvas.GetComponent<Canvas>().sortingOrder = 1;

    }
    public void Menu_To_HowTo()
    {
        HowToCanvas.GetComponent<Canvas>().sortingOrder = 1;
        MenuCanvas.GetComponent<Canvas>().sortingOrder = 0;
    }

}
