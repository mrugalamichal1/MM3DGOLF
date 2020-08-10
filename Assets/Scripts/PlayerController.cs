using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

 

}

[System.Serializable]
public class Player
{
    public string name;
    public LevelScores levelScores;


    public Player(string name)
    {
        this.name = name;
        this.levelScores = new LevelScores();
    }
    
    public string getName()
    {
        return this.name;
    }
}

[System.Serializable]
public class LevelScores 
{
    public int l1;
    public int l2;
    public int l3;
    public int r;

    public LevelScores()
    {
        this.l1 = this.l2 = this.l3 = this.l3 = this.r = 0;
    }
    public void computeR()
    {
        this.r=this.l1 + this.l2 + this.l3;
    }
}


public class PlayerList
{
    public List<Player> list;

    public PlayerList()
    {
        this.list = new List<Player>();
    }
    public PlayerList(List<Player> list)
    {
        this.list = list;
    }
    public void setList(List<Player> list)
    {
        this.list = list;
    }
    public List<Player> getList()
    {
        return this.list;
    }
}



