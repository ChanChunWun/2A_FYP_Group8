using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    public bool ai = true;
    public bool MainBody;
    public float Armor = 60;
    public float MaxHp = 500;
    float hp;
    public bool nonMainHit; // will take damage too when it is not mainbody
    public GameObject LinkBody;
    public bool dead = false;
    bool sent = false;
    // Start is called before the first frame update
    void Start()
    {
        hp = MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit (object[] data)
    {
        float totDamage = 0;
        object[] damagedata = new object[2];
        damagedata[0] = (float)data[0];
        damagedata[1] = (float)data[1];
        if (MainBody)
        {
            if(((float)data[0] - Armor) > 0)
            {
                totDamage += (float)data[0] - Armor;
            }
            else
            {
                
            }
            Debug.Log(totDamage);
            totDamage += (float)data[1];
            
            hp -= totDamage;
        }
        else
        {
            if (nonMainHit)
            {
                LinkBody.SendMessage("Hit", damagedata);
                if (((float)data[0] - Armor) > 0)
                {

                }
                else
                {
                    totDamage += (float)data[0] - Armor;
                }
                totDamage += (float)data[1];
                Debug.Log(totDamage);
                hp -= totDamage;
            }
            else
            {
                LinkBody.SendMessage("Hit", damagedata);
            }
        }

        if (hp <= 0)
        {
            dead = true;
            if (dead && !sent)
            {
                sent = true;
                MissionSaver.Instance.MissionDoing(GetComponent<EnemyAIName>().enemyName);
            }
        }
    }
}
