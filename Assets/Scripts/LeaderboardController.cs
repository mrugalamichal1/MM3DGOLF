using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LeaderboardController : MonoBehaviour
{
    public Transform recordContainer;
    public Transform recordTemplate;
    public List<Transform> recordEntryTransformList;
    public LeaderBoardList leaderBoardList;
    
    public void Awake()
    {
        leaderBoardList = new LeaderBoardList();

        if (!PlayerPrefs.HasKey("LEADERBOARD"))
        {
            PlayerPrefs.SetString("LEADERBOARD", JsonUtility.ToJson(leaderBoardList));
            PlayerPrefs.Save();
        }

        recordTemplate.gameObject.SetActive(false);
    }
    public void BuildLeaderboard()
    {

        leaderBoardList = JsonUtility.FromJson<LeaderBoardList>(PlayerPrefs.GetString("LEADERBOARD"));


        recordEntryTransformList = new List<Transform>();

        for (int i = 0; i < leaderBoardList.getList().Count; i++)
        {
            for(int j=i+1;j< leaderBoardList.getList().Count; j++)
            {
                if(leaderBoardList.getList()[j].getScore() > leaderBoardList.getList()[i].getScore()){
                    LeaderBoardEntry temp = leaderBoardList.getList()[i];
                    leaderBoardList.getList()[i] = leaderBoardList.getList()[j];
                    leaderBoardList.getList()[j] = temp;
                }
            }
        }

        

        for(int i=0;i<10;i++)
        {
            LeaderBoardEntry r = leaderBoardList.getList()[i];
  
            Transform recordTransform = Instantiate(recordTemplate, recordContainer);
            RectTransform recordRectTransform = recordTransform.GetComponent<RectTransform>();
            recordRectTransform.anchoredPosition = new Vector2(0, -30f * recordEntryTransformList.Count);
            recordTransform.gameObject.SetActive(true);

            int rank = recordEntryTransformList.Count + 1;
            
            recordTransform.Find("RankText").GetComponent<Text>().text = rank.ToString();
            recordTransform.Find("NameText").GetComponent<Text>().text = r.getName();
            recordTransform.Find("ScoreText").GetComponent<Text>().text = r.getScore().ToString();

            recordEntryTransformList.Add(recordTransform);

        }


      
    }
    public void DestroyLeaderboard()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Leaderboard");
        foreach (GameObject obj in go)
            GameObject.Destroy(obj);
    }

}
[System.Serializable]
public class LeaderBoardEntry
{
    public string playerName;
    public float score;

    public LeaderBoardEntry(string name)
    {
        this.playerName = name;
        this.score = 0f;
    }

    public LeaderBoardEntry(string name,float score)
    {
        this.playerName = name;
        this.score = score;
    }

    public string getName()
    {
        return this.playerName;
    }
    public float getScore()
    {
        return this.score;
    }
}
public class LeaderBoardList
{
    public List<LeaderBoardEntry> leaderBoardEntries;

    public LeaderBoardList()
    {
        this.leaderBoardEntries = new List<LeaderBoardEntry>();
    }
    
    public List<LeaderBoardEntry> getList()
    {
        return this.leaderBoardEntries;
    }
}

