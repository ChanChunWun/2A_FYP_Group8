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

        moneyManager.Instance.increaseMoney(nowPickItem.price);
        Destroy(nowPickItem);
        ResetPickItem();
    }
}
