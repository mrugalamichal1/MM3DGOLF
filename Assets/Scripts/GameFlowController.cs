using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameFlowController : MonoBehaviour
{
    public GameObject level;
    public GameObject hole;
    public GameObject player;
    public GameObject arrow;
    public GameObject mainCamera;

    private bool is_in = false;

    public PlayerList gamelist;
    public int idx_active_player = 0;
    public int levelPAR;
    public int Strikes = 0;
    public bool CanAct = true;

    public MovementController movementController;
    public UIController uIController;


    public void Start()
    {
        KeepCurrentPar();

        QualitySettings.vSyncCount = 1;

        Cursor.visible = false;

        gamelist = new PlayerList();
        gamelist = JsonUtility.FromJson<PlayerList>(PlayerPrefs.GetString("LOCAL_CHAMPIONSHIP"));
        

        movementController = GameObject.FindObjectOfType<MovementController>();
        uIController = GameObject.FindObjectOfType<UIController>();
       

        player = GameObject.Find("player");
        arrow = GameObject.Find("arrow");
        mainCamera = GameObject.Find("Main Camera");
        level = GameObject.Find("Level1");
        hole = GameObject.Find("hole");






        //Debug.Log(PlayerPrefs.GetString("LOCAL_CHAMPIONSHIP"));


    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            level.GetComponent<Collider>().enabled = false;
            hole.GetComponent<Collider>().enabled = true;
            is_in = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            level.GetComponent<Collider>().enabled = true;
            hole.GetComponent<Collider>().enabled = false;
            is_in = false;
        }
    }





    public void ScoreCheck()
    {
        enabled = true;
        if (is_in && player.transform.position.y <= hole.transform.position.y)
        {
            if (uIController.Game_score_panel.activeSelf)
            {
                uIController.DestroyScoreboard();
                uIController.Game_score_panel.SetActive(false);
            }

            enabled = false;
            UpdateLeaderboard(gamelist.getList()[idx_active_player].name, Strikes - levelPAR);
            movementController.newPlayer = true;
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 1:
                    gamelist.getList()[idx_active_player].levelScores.l1 = Strikes - levelPAR;
                    break;
                case 2:
                    gamelist.getList()[idx_active_player].levelScores.l2 = Strikes - levelPAR;
                    break;
                case 3:
                    gamelist.getList()[idx_active_player].levelScores.l3 = Strikes - levelPAR;
                    break;
            }
            gamelist.getList()[idx_active_player].levelScores.computeR();
            StartCoroutine(Sleep_congratulation_screen());


        }

    }



    public IEnumerator Sleep_congratulation_screen()
    {
        yield return new WaitForSeconds(1f);
        uIController.ScorePanel.SetActive(true);
        uIController.ShowScore();
        Cursor.visible = true;
    }




    public void KeepCurrentPar()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                levelPAR = 2;
                break;
            case 2:
                levelPAR = 3;
                break;
            case 3:
                levelPAR = 7;
                break;
        }
    }

    public void FixedUpdate()
    {
        ScoreCheck();
    }






    public void UpdateLeaderboard(string name, float pardiff)
    {
        LeaderBoardList leaderboard = JsonUtility.FromJson<LeaderBoardList>(PlayerPrefs.GetString("LEADERBOARD"));
        bool duplicate = false;

        if (pardiff < 0)
            pardiff = Math.Abs(pardiff * 5000);
        else if (pardiff > 0)
            pardiff *= 50;
        else
            pardiff = 2500f;

        foreach (LeaderBoardEntry r in leaderboard.getList())
        {
            if (r.getName() == name)
            {
                duplicate = true;
                r.score += pardiff;
            }
        }
        if (!duplicate)
        {
            LeaderBoardEntry temp = new LeaderBoardEntry(name, pardiff);
            leaderboard.leaderBoardEntries.Add(temp);
        }

        PlayerPrefs.SetString("LEADERBOARD", JsonUtility.ToJson(leaderboard)); 
        PlayerPrefs.Save();

    }

    public void NextPlayer()
    {
        uIController.ShowScore();
        movementController.new_player_normalize();
        idx_active_player++;
        uIController.ScorePanel.SetActive(false);
        Cursor.visible = false;
        Strikes = 0;
    }

    public void PauseGame(bool input)
    {
        if (input)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

}



