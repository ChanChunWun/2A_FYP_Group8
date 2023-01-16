using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLaserSc : MonoBehaviour
{
    public bool LaserWeapon;
    public Transform shootpos;
    public float _Extents = 3;
    bool m_HitDetect;
    Collider m_Collider;
    RaycastHit m_Hit;

    // Start is called before the first frame update
    void Start()
    {
        m_Collider = shootpos.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Extents = new Vector3(_Extents, _Extents, shootpos.transform.localScale.z);
        m_HitDetect = Physics.BoxCast(m_Collider.bounds.center, Extents, shootpos.transform.forward, out m_Hit, shootpos.transform.rotation, 2000);
        if (m_HitDetect)
        {
            //Output the name of the Collider your Box hit
            Debug.Log("Hit : " + m_Hit.collider.name);
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 Extents = new Vector3(_Extents, _Extents, shootpos.transform.localScale.z);
        //Check if there has been a hit yet
        if (m_HitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(shootpos.transform.position, shootpos.transform.forward * m_Hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(shootpos.transform.position + shootpos.transform.forward * m_Hit.distance, Extents);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(shootpos.transform.position, shootpos.transform.forward * 2000);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(shootpos.transform.position + shootpos.transform.forward * 2000, Extents);
        }
    }
}
