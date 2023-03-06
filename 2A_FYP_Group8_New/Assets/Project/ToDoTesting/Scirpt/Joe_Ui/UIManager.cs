using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //public List<GameObject> menuList;

    private void Start()
    {
        
    }

    /*public void showOnceUI(GameObject UI)
    {
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i] == UI)
            {
                UI.SetActive(true);
            }
            else
            {
                menuList[i].SendMessage("Clean");
                menuList[i].SetActive(false);
            }
        }
    }*/
    public static void s_showOnceUI(GameObject UI)
    {
        GameObject[] menus = GameObject.FindGameObjectsWithTag("Menu");
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i] == UI)
            {
                UI.SetActive(true);
            }
            else
            {
                menus[i].SendMessage("Clean", SendMessageOptions.DontRequireReceiver);
                menus[i].SetActive(false);
            }
        }
    }
    public static void menuOn()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }
    public static void menuOff()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
