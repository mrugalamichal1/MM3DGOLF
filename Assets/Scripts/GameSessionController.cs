using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSessionController : MonoBehaviour
{

    public CanvasController canvasController;
    public string GameMode = "";

    public void Awake()
    {
        canvasController = GameObject.FindObjectOfType<CanvasController>();
    }

    public void CreateSession()
    {
        PlayerList local_championship = new PlayerList();
        if (GameMode == "sp") 
        {
            local_championship.getList().Add(new Player(GameObject.Find("Player_Name_sp").GetComponent<InputField>().text));

        }
        else
        {
            local_championship.getList().Add(new Player(GameObject.Find("Player1_Name_mp").GetComponent<InputField>().text));
            local_championship.getList().Add(new Player(GameObject.Find("Player2_Name_mp").GetComponent<InputField>().text));
        }
        PlayerPrefs.SetString("LOCAL_CHAMPIONSHIP", JsonUtility.ToJson(local_championship));
        PlayerPrefs.Save();
        canvasController.NextLevel();

        }

    public void ButtonsIntegrity()
    {
        if (GameMode == "sp")
        {
            if (string.IsNullOrEmpty(GameObject.Find("Player_Name_sp").GetComponent<InputField>().text))
                GameObject.Find("Start_sp").GetComponent<Button>().interactable = false;
            else
                GameObject.Find("Start_sp").GetComponent<Button>().interactable = true;

        }
        else
        {
            if (
                       string.IsNullOrEmpty(GameObject.Find("Player1_Name_mp").GetComponent<InputField>().text) ||
                       string.IsNullOrEmpty(GameObject.Find("Player2_Name_mp").GetComponent<InputField>().text)
               )
                GameObject.Find("Start_mp").GetComponent<Button>().interactable = false;
            else
                GameObject.Find("Start_mp").GetComponent<Button>().interactable = true;
        }
    }

    public void Update()
    {
        ButtonsIntegrity();
    }
       

}
