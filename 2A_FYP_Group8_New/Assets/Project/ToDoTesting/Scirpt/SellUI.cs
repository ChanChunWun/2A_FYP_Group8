using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellUI : MonoBehaviour
{
    ItemData nowPickItem;

    public void SetPickItem(ItemData item)
    {
        nowPickItem = item;
    }

    public void ResetPickItem()
    {
        nowPickItem = null;
    }

    public void SellObj()
    {
        if (nowPickItem == null)
            return;

        int price = 10;
        moneyManager.Instance.increaseMoney(price);
        Destroy(nowPickItem);
        ResetPickItem();
    }
}
