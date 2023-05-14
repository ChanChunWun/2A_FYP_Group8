using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class moneyManager : Singleton<moneyManager>
{
    int money;
    public TMP_Text _moneyText;
    public PlayerMoneySave playerMoney;

    // Start is called before the first frame update
    void Start()
    {
        money = playerMoney.GetMoney();
    }

    // Update is called once per frame
    void Update()
    {
        money = playerMoney.GetMoney();
        _moneyText.text = "$" + money.ToString();

        if (SceneManager.GetActiveScene().name != "StartScene")
        {
            _moneyText.alpha = 0;
        }
        else
        {
            _moneyText.alpha = 1;
        }
    }

    public int getMoney()
    {
        return money;
    }

    public void reduceMoney(int reduce)
    {
        playerMoney.ReduceMoney(reduce);
    }

    public void increaseMoney(int increase)
    {
        playerMoney.IncreaseMoney(increase);
    }
}
