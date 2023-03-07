using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    NavMeshAgent nav;
    Animator anim;
    Transform target;
    LifeSystem lfSys;
    float minFlyHeight = 1.5f;
    float fixedFlyHeight;
    RaycastHit hit;

    public Transform hand;
    public GameObject weapon;
    public Transform shootPoint;

    public float speed;
    [Range(1.5f, 4f)]
    public float flyHeightRange;
    public float canShootRange = 50;

    

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        lfSys = GetComponent<LifeSystem>();
        fixedFlyHeight = Random.Range(minFlyHeight, flyHeightRange);
    }

    // Update is called once per frame
    void Update()
    {
        if (!lfSys.dead)
        {
            if (target != null)
            {

                if (Physics.Raycast(shootPoint.position, target.transform.position, out hit, canShootRange))
                {
                    if (hit.transform.gameObject == target)
                    {
                        weapon.GetComponent<TurretWeaponSystem>().Shoot(gameObject, shootPoint, null);
                    }
                }
            }
        }
        
    }
}
