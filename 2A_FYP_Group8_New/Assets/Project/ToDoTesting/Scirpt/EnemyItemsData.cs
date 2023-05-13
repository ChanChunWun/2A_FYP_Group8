using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyItemsData : MonoBehaviour
{

    public List<InventoryItem> dropItem;
    public int minDropNumber = 0;
    public int maxDropNumber = 3;

    LifeSystem lf;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!lf.dead)
            return;

        int dropNumber = Random.Range(minDropNumber, maxDropNumber + 1);
        List<InventoryItem> itemList = new List<InventoryItem>();
        for (int i = 0; i < dropNumber; i++)
        {
            itemList.Add(dropItem[Random.Range(0, dropItem.Count)]);
        }
        //InventoryItem.Instantiate.AddItmes(itmeList);
        Destroy(this);
    }
}
