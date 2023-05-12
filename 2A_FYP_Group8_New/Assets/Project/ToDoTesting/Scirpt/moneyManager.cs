using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class moneyManager : Singleton<moneyManager>
{
    int money = 1000;
    public TMP_Text _moneyText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _moneyText.text = money.ToString();
    }

    public int getMoney()
    {
        return money;
    }

    public void reduceMoney(int reduce)
    {
        money -= reduce;
    }

    public void increaseMoney(int increase)
    {
        money += increase;
    }
}
