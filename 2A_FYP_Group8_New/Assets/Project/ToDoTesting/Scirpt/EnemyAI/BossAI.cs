using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    // Start is called before the first frame update
    LifeSystem lf;
    public GameObject ExplodeEff;
    
    public List<Transform> Bodys;
    /*
    0 LiftShou;
    1 LifeArm;
    2 RightShou;
    3 RightArm;
    4 Head;
    5 Waist;
     */

    public List<TurretWeaponSystem> weapons;

    private GameObject Player;
    private bool StartAttack;

    private List<Quaternion> OrgRos = new List<Quaternion>();
    /*
0 LiftShou;
1 LifeArm;
2 RightShou;
3 RightArm;
4 Head;
5 Waist;
 */

    void Start()
    {
        lf = GetComponent<LifeSystem>();
        Player = GameObject.FindWithTag("Player");
        for (int i = 0; i < Bodys.Count; i++)
        {
            OrgRos.Add(Bodys[i].rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            if (Vector3.Distance(Player.transform.position, transform.position) < 1000)
            {
                StartAttack = true;
            }
            else
            {
                StartAttack = false;
            }
        }

        if (lf.dead != true)
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i] != null)
                    weapons[i].Shoot(gameObject, null, null);
            }
            LiftShouTurn();
            LifeArmTurn();
            RightShouTurn();
            RightArmTurn();
            HeadTurn();
            WaistTurn();
        }
        
    }
    
    void LiftShouTurn()
    {
        if (Player != null)
        {
            //var rotation = Quaternion.LookRotation(Player.transform.position - transform.position);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 4);
            //rotation.y += 0;
            //rotation.x += -90;
            //rotation.z += 0;
            // Quaternion a = OrgRos[0] * Bodys[0];
            //Bodys[0].rotation = Quaternion.Slerp(LiftShou.rotation, rotation, Time.deltaTime * 4);

            //LiftShou.rotation = Quaternion.Euler(0, LiftShou.rotation.y - 90, 0);
        }
    }

    void LifeArmTurn()
    {
        if (Player != null)
        {

        }
    }

    void RightShouTurn()
    {
        if (Player != null)
        {

        }
    }

    void RightArmTurn()
    {
        if (Player != null)
        {

        }
    }

    void HeadTurn()
    {
        if (Player != null)
        {

        }
    }
    void WaistTurn()
    {
        if (Player != null)
        {

        }
    }
}
