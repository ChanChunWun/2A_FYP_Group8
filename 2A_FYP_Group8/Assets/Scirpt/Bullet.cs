using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float ShootForce = 20;
    public float Damage = 40;
    public float TrueDamage = 20;
    public float AddAccuracy = 0f;
    public int BulletNum = 1;
    float ShowCount = 0;
    GameObject shooter;
    public GameObject lights;
    

    private void Update()
    {
        if (ShowCount < 0.0001f)
        {
            ShowCount += Time.deltaTime;
            lights.SetActive(false);
        }
        else
        {
            lights.SetActive(true);
        }

    }

    public void SetShooter(GameObject Pl)
    {
        shooter = Pl;
    }

    void OnCollisionEnter(Collision col)
    {
        object[] damagedata = new object[2];
        damagedata[0] = Damage;
        damagedata[1] = TrueDamage;
        Debug.Log("Hit");
        if (col.gameObject.tag != gameObject.tag)
        {
            Debug.Log("Hit : " + col.gameObject.name);
            //col.SendMessage("Hit", damagedata);
            Destroy(gameObject);
        }
    }

    
    public void SetData(float Dam, float TrueDam, float Force)
    {
        Damage = Dam;
        TrueDamage = TrueDam;
        ShootForce = Force;
    }

    void RayGot()
    {
        object[] damagedata = new object[2];
        damagedata[0] = Damage;
        damagedata[1] = TrueDamage;
        Debug.Log("Hit");
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward * 0.1f);
        if (Physics.Raycast(ray, out hit, 0.1f))
        {
            if (hit.transform.tag != gameObject.tag)
            {
                Debug.Log("Hit : " + hit.transform.name);
                //col.SendMessage("Hit", damagedata);
                Destroy(gameObject);

            }
        }
    }
}
