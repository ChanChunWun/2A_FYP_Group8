using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemManager : MonoBehaviour
{
    GameObject LeftWeapon;
    GameObject RightWeapon;
    public List<GameObject> weaponList;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWeapon(string type, GameObject weapon)
    {
        //if (weapon == null)
        //    return;
        
        for (int i = 0; i < weaponList.Count; i++)
        {
            Debug.Log(weapon.name + "&" + weaponList[i].name);
            if (weapon.name == weaponList[i].name + "(Clone)")
            {
                if (type == "left")
                {
                    LeftWeapon = weaponList[i];
                }
                else if (type == "right")
                {
                    RightWeapon = weaponList[i];
                }
            }
        }
    }

    public GameObject GetWeapon(string type)
    {
        if (type == "left")
        {
            return LeftWeapon;
        }
        else if (type == "right")
        {
            return RightWeapon;
        }
        else
        {
            Debug.Log("The weaponType Error");
            return null;
        }
    }
}
