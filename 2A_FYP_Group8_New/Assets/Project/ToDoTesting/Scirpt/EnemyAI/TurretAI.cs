using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Head;
    public GameObject ExEff;
    public Transform target;
    public Transform shootPoint;
    public float canShootRange = 500;

    TurretWeaponSystem Wp;
    LifeSystem lf;
    RaycastHit hit;

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
                    var rotation = Quaternion.LookRotation(target.transform.position - Head.transform.position);
                    Head.transform.rotation = Quaternion.Slerp(Head.transform.rotation, rotation, Time.deltaTime * 3);
                    
                    Debug.DrawLine(shootPoint.position, shootPoint.transform.forward * canShootRange, Color.red);

                    if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward * canShootRange, out hit))
                    {
                        Debug.Log("Hit" + hit.transform.name);
                        if (hit.transform.tag.Equals("Player"))
                        {
                            Wp.GetComponent<TurretWeaponSystem>().Shoot(gameObject, shootPoint, null);
                        }
                    }
                }

            }
        }
        else
        {
            GetComponent<EnemyItemsData>().SetItems();
            //MissionSaver.Instance.MissionDoing(GetComponent<EnemyAIName>().enemyName);
            Destroy(gameObject);
        }
    }
}
