using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Head;
    public GameObject ExEff;
    public Transform target;

    TurretWeaponSystem Wp;
    LifeSystem lf;

    void Start()
    {
        Wp = Head.GetComponent<TurretWeaponSystem>();
        lf = GetComponent<LifeSystem>();
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (lf.dead != true)
        {
            if (target != null)
            {
                if (Vector3.Distance(target.transform.position, transform.position) < 500)
                {
                    var rotation = Quaternion.LookRotation(target.transform.position - transform.position);
                    Head.transform.rotation = Quaternion.Slerp(Head.transform.rotation, rotation, Time.deltaTime * 3);
                    RaycastHit hit;
                    Wp.Shoot(gameObject, null, null);
                    /*if (Physics.Raycast(transform.position, transform.forward * 500, out hit))
                    {
                        Debug.DrawRay(transform.position, transform.forward, Color.red, 500);
                        if (hit.transform.gameObject == target)
                        {
                            Wp.Shoot(gameObject, null);
                        }
                    }*/
                }

            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
