using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellUI : MonoBehaviour
{
    InventoryItem nowPickItem;
    CanvasGroup cg;

    private void Awake()
    {
        cg = transform.parent.GetChild(0).GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SetPickItem(InventoryController.Instance.itemToHighlight);
            if (nowPickItem == null)
                return;

            if (cg.alpha == 0)
            {
                nowPickItem = null;
                transform.GetChild(0).gameObject.SetActive(false);
                return;
            }
                

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
        moneyManager.Instance.increaseMoney(nowPickItem.itemData.price);
        
        Destroy(nowPickItem.gameObject);
        ResetPickItem();
    }
}
