using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShowItems : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> items;
    public Transform beforeStartPoint;
    public Transform afterStartPoint;
    public Transform stopPoint;
    GameObject showIngOj;
    int nowItem = 0;
    void Start()
    {
        showIngOj = Instantiate(items[nowItem], afterStartPoint);
        showIngOj.transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {
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
}
