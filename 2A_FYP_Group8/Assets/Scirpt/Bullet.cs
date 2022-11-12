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
    public GameObject light;
    

    private void Update()
    {
        if (ShowCount < 0.0001f)
        {
            ShowCount += Time.deltaTime;
            light.SetActive(false);
        }
        else
        {
            light.SetActive(true);
        }

    }

    public void SetShooter(GameObject Pl)
    {
        shooter = Pl;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag != tag)
        {
            //col.GetComponent<...>().Hit(Damage, TrueDamage, shooter);
            Destroy(gameObject);
        }
    }

    public void SetData(float Dam, float TrueDam, float Force)
    {
        Damage = Dam;
        TrueDamage = TrueDam;
        ShootForce = Force;
    }
}
