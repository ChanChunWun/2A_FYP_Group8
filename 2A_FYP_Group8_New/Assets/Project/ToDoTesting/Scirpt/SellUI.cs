using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellUI : MonoBehaviour
{
    InventoryItem nowPickItem;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SetPickItem(InventoryController.Instance.SelectedItem);
            transform.position = Input.mousePosition;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void SetPickItem(InventoryItem item)
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

        transform.GetChild(0).gameObject.SetActive(false);
        moneyManager.Instance.increaseMoney(nowPickItem.GetComponent<ItemData>().price);
        
        Destroy(nowPickItem);
        ResetPickItem();
    }
}
