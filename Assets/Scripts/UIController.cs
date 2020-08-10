using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text CurrentPlayerText;
    public Text CurrentPARText;
    public Text ForceText;
    public Text StrikesText;
    public Text CurrentScoreText;
    public GameObject ScorePanel;
    public GameObject Game_score_panel;
    public GameObject EscMenu;
    public GameObject PowerBar;
    public GameFlowController gameFlowController;
    public MovementController movementController;

    public Transform recordTemplate;
    public Transform recordContainer;


    public void Awake()
    {
        gameFlowController = GameObject.FindObjectOfType<GameFlowController>();
        movementController = GameObject.FindObjectOfType<MovementController>();
        CurrentPlayerText = GameObject.Find("CurrentPlayerText").GetComponent<Text>();
        CurrentPARText = GameObject.Find("CurrentPARText").GetComponent<Text>();
        StrikesText = GameObject.Find("StrikesText").GetComponent<Text>();
        CurrentScoreText = GameObject.Find("CurrentScoreText").GetComponent<Text>();
        ForceText = GameObject.Find("ForceText").GetComponent<Text>();
        ScorePanel = GameObject.Find("Score_panel");
        Game_score_panel = GameObject.Find("Game_score_panel");
        EscMenu = GameObject.Find("EscMenu");
        PowerBar = GameObject.Find("Bar");

        recordTemplate = GameObject.Find("RecordTemplate").GetComponent<Transform>();
        recordContainer = GameObject.Find("RercordEntryContainer").GetComponent<Transform>();
        recordTemplate.gameObject.SetActive(false);

        ScorePanel.SetActive(false);
        Game_score_panel.SetActive(false);
        GameObject.Find("EscMenu").SetActive(false);


    }


    public void ShowCurrentPlayer()
    {
        CurrentPlayerText.text = gameFlowController.gamelist.getList()[gameFlowController.idx_active_player].getName();
    }
    public void ShowCurrentPAR()
    {
        CurrentPARText.text = "PAR: " + gameFlowController.levelPAR;
    }
    public void UpdateStrikes()
    {
        StrikesText.text = "Strikes: " + gameFlowController.Strikes;
    }
    public void UpdateCurrentScore()
    {
        CurrentScoreText.text = "Score: " + (gameFlowController.Strikes - gameFlowController.levelPAR).ToString();
    }

    public void Scene0()
    {
        gameFlowController.PauseGame(false);
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void NextScene()
    {
        PlayerPrefs.SetString("LOCAL_CHAMPIONSHIP", JsonUtility.ToJson(gameFlowController.gamelist));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ButtonsIntegrity()
    {

        if (ScorePanel.activeSelf)
        {
            if (gameFlowController.idx_active_player == gameFlowController.gamelist.getList().Count - 1 && SceneManager.GetActiveScene().buildIndex < 3)
            {
                GameObject.Find("Next_Level").GetComponent<Button>().interactable = true;
                GameObject.Find("Next_Player").GetComponent<Button>().interactable = false;
            }
            else if (gameFlowController.idx_active_player == gameFlowController.gamelist.getList().Count - 1 && SceneManager.GetActiveScene().buildIndex == 3)
            {
                GameObject.Find("Next_Player").GetComponent<Button>().interactable = false;
                GameObject.Find("Next_Level").GetComponent<Button>().interactable = false;
            }
            else
            {
                GameObject.Find("Next_Level").GetComponent<Button>().interactable = false;
            }


        }
    }

    public void BuildScoreboard()
    {

        List<Transform> recordEntryTransformList = new List<Transform>();

        for (int i = 0; i < gameFlowController.gamelist.getList().Count; i++)
        {
            Player player = gameFlowController.gamelist.getList()[i];

            Transform recordTransform = Instantiate(recordTemplate, recordContainer);
            RectTransform recordRectTransform = recordTransform.GetComponent<RectTransform>();
            recordRectTransform.anchoredPosition = new Vector2(0, -65f * recordEntryTransformList.Count);
            recordTransform.gameObject.SetActive(true);


            recordTransform.Find("NameText").GetComponent<Text>().text = player.name;
            recordTransform.Find("l1").GetComponent<Text>().text = player.levelScores.l1.ToString();
            recordTransform.Find("l2").GetComponent<Text>().text = player.levelScores.l2.ToString();
            recordTransform.Find("l3").GetComponent<Text>().text = player.levelScores.l3.ToString();
            recordTransform.Find("r").GetComponent<Text>().text = player.levelScores.r.ToString();

            if (recordTransform.Find("NameText").GetComponent<Text>().text == gameFlowController.gamelist.list[gameFlowController.idx_active_player].name)
                recordTransform.Find("NameText").GetComponent<Text>().color = Color.green;

            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 1:
                    GameObject.Find("l1").GetComponent<Text>().color = Color.green;
                    break;
                case 2:
                    GameObject.Find("l2").GetComponent<Text>().color = Color.green;
                    break;
                case 3:
                    GameObject.Find("l3").GetComponent<Text>().color = Color.green;
                    break;

            }

            recordEntryTransformList.Add(recordTransform);
        }

    }

    public void DestroyScoreboard()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Leaderboard");
        foreach (GameObject obj in go)
            GameObject.Destroy(obj);
    }

    public void ShowScore()
    {
        if (Game_score_panel.activeSelf && gameFlowController.CanAct)
        {
            DestroyScoreboard();
            Game_score_panel.SetActive(false);
        }
        else if (!Game_score_panel.activeSelf && gameFlowController.CanAct)
        {
            Game_score_panel.SetActive(true);
            BuildScoreboard();
        }
    }

    public void MenuToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EscMenu.activeSelf)
            {
                Cursor.visible = false;
                movementController.ManualCamera = true;

                gameFlowController.CanAct = true;
                EscMenu.SetActive(false);
                gameFlowController.PauseGame(false);


            }
            else if (!EscMenu.activeSelf && !ScorePanel.activeSelf)
            {
                movementController.ManualCamera = false;
                Cursor.visible = true;
                gameFlowController.CanAct = false;
                EscMenu.SetActive(true);
                gameFlowController.PauseGame(true);

            }


        }
    }
    public void IncreasePowerBar()
    {
        PowerBar.GetComponent<Image>().fillAmount += 0.01f;
    }
    public void ResetPowerBar()
    {
        PowerBar.GetComponent<Image>().fillAmount = 0f;
    }
    public void ChangeBarCollor()
    {
        if (movementController.Force < 500)
            PowerBar.GetComponent<Image>().color = Color.green;
        else if (movementController.Force > 500 && movementController.Force < 1000)
            PowerBar.GetComponent<Image>().color = Color.yellow;
        else
            PowerBar.GetComponent<Image>().color = Color.red;
    }
    public void Update()
    {
        ShowCurrentPlayer();
        ShowCurrentPAR();
        UpdateStrikes();
        ButtonsIntegrity();
        UpdateCurrentScore();
        MenuToggle();
        ChangeBarCollor();

        if (Input.GetKeyDown(KeyCode.Tab))
            ShowScore();

        if (movementController.player.GetComponent<Rigidbody>().isKinematic)
            PowerBar.GetComponent<Image>().fillAmount = 0f;


    }
}
