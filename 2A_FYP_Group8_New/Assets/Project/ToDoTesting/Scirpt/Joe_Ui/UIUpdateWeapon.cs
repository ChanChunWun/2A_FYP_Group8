using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public class UIUpdateWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> items;
    public Transform beforeStartPoint;
    public Transform afterStartPoint;
    public TMP_Text _text;
    public TMP_Text _levelText;
    public TMP_Text _nameText;
    public Transform stopPoint;
    public Transform levelIndicatorSpawnTransform;

    public ItemManager itemManager;
    public GameObject levelNullObj;
    public GameObject levelFullObj;

    GameObject showIngOj;
    int nowItem = 0;
    string type = "left";
    void Start()
    {
        type = "right";
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        showIngOj = Instantiate(items[nowItem], afterStartPoint);
        showIngOj.transform.SetParent(transform);
        ShowWeaponLevel();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = "Cose: " + ((items[nowItem].GetComponent<ItemWeaponSystem>().Level + 1) * 500).ToString();
        float step = 50 * Time.deltaTime;
        //Debug.Log(step);
        showIngOj.transform.position = Vector3.MoveTowards(showIngOj.transform.position, stopPoint.position, step);
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
        ShowWeaponLevel();
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
        ShowWeaponLevel();
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

        ShowWeaponLevel();

    }

    void ShowWeaponLevel()
    {
        _nameText.text = items[nowItem].GetComponent<ItemWeaponSystem>().name;
        _levelText.text = items[nowItem].GetComponent<ItemWeaponSystem>().Level.ToString();
        foreach (Transform child in levelIndicatorSpawnTransform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < (items[nowItem].GetComponent<ItemWeaponSystem>().MaxLevel - items[nowItem].GetComponent<ItemWeaponSystem>().Level); i++)
        {
            Instantiate(levelNullObj, levelIndicatorSpawnTransform);
        }
        for (int i = 0; i < items[nowItem].GetComponent<ItemWeaponSystem>().Level; i++)
        {
            Instantiate(levelFullObj, levelIndicatorSpawnTransform);
        }

        Debug.Log(items[nowItem].name + " level showed");
    }

    public void setType(string _type)
    {
        type = _type;
    }
    public void UpgradeWeapon()
    {
        int needs = (int)((items[nowItem].GetComponent<ItemWeaponSystem>().Level + 1) * 500);
        if (moneyManager.Instance.getMoney() - needs < 0)
            return;

        if (items[nowItem].GetComponent<ItemWeaponSystem>().Level >= items[nowItem].GetComponent<ItemWeaponSystem>().MaxLevel)
            return;

        moneyManager.Instance.reduceMoney(needs);      
        items[nowItem].GetComponent<ItemWeaponSystem>().Level += 1;
        ShowWeaponLevel();
    }
}
