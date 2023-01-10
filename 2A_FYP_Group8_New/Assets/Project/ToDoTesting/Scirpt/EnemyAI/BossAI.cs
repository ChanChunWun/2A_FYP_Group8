using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    // Start is called before the first frame update
    LifeSystem lf;
    public GameObject ExplodeEff;
    public Transform LiftShou;
    public Transform LifeArm;
    public Transform RightShou;
    public Transform RightArm;
    public Transform Head;
    public Transform Waist;

    public List<TurretWeaponSystem> weapons;

    GameObject Player;
    bool StartAttack;
    void Start()
    {
        lf = GetComponent<LifeSystem>();
        Player = GameObject.FindWithTag("Player");
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

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] != null)
            weapons[i].Shoot(gameObject, null);
        }
    }
    
    void LiftShouTurn()
    {
        if (Player != null)
        {

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
