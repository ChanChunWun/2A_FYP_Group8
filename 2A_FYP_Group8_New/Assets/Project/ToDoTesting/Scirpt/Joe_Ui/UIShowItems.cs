using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public class UIShowItems : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> items;
    public Transform beforeStartPoint;
    public Transform afterStartPoint;
    public TMP_Text _text;
    public Transform stopPoint;

    ItemManager itemManager;
    GameObject showIngOj;
    int nowItem = 0;
    string type = "left";
    void Start()
    {
        type = "right";
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        showIngOj = Instantiate(items[nowItem], afterStartPoint);
        showIngOj.transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = "Please choose " + type + " arm weapon";
        float step = 50 * Time.deltaTime;
        //Debug.Log(step);
        showIngOj.transform.position = Vector3.MoveTowards(showIngOj.transform.position, stopPoint.position,step );
    }

    public void ShowAfterItem()
    {
        Destroy(showIngOj);
        if (nowItem == items.Count - 1)
        {
            nowItem = 0;
        }
        else
        {
            nowItem++;
        }
        showIngOj = Instantiate(items[nowItem], afterStartPoint);
        showIngOj.transform.SetParent(transform);
    }

    public void ShowBeforeItem()
    {
        Destroy(showIngOj);
        if (nowItem == 0)
        {
            nowItem = items.Count - 1;
        }
        else
        {
            nowItem--;
        }
        showIngOj = Instantiate(items[nowItem], beforeStartPoint);
        showIngOj.transform.SetParent(transform);
    }

    public void changeType()
    {
        if (type == "right")
        {
            type = "left";
        }
        else if (type == "left")
        {
            type = "right";
            ScenceManager.goScene("TestTrack");
        }
    }

    public void setType(string _type)
    {
        type = _type;
    }
    public void ChoosedItem()
    {
        itemManager.SetWeapon(type, showIngOj);
    }
}
