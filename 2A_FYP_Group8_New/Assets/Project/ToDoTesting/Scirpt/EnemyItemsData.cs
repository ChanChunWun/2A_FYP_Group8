using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyItemsData : MonoBehaviour
{
    public List<ItemData> dropItem;
    public int minDropNumber = 0;
    public int maxDropNumber = 3;

    LifeSystem lf;

    public List<ItemData> itemList;

    private void Awake()
    {
        lf = GetComponent<LifeSystem>();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void SetItems()
    {
        int dropNumber = Random.Range(minDropNumber, maxDropNumber + 1);
        Debug.Log("Random: " + dropNumber);
        itemList = new List<ItemData>();
        for (int i = 0; i < dropNumber; i++)
            itemList.Add(dropItem[Random.Range(0, dropItem.Count)]);
        InventoryController.Instance.AddItems(itemList);
        Destroy(gameObject);
    }
}
