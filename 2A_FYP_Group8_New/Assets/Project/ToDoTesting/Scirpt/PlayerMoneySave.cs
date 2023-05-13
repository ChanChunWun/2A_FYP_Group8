using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Money Saver", menuName = "FYP/Player")]
public class PlayerMoneySave : ScriptableObject
{
    string playerName;
    int money;
    // Start is called before the first frame update
    public void SetMoney(int mon)
    {
        money = mon;
    }

    public void IncreaseMoney(int mon)
    {
        money += mon;
    }

    public void ReduceMoney(int mon)
    {
        money -= mon;
    }

    public int GetMoney()
    {
        return money;
    }

    public void SetName(string name)
    {
        playerName = name;
    }

    public string GetName()
    {
        return name;
    }
}
