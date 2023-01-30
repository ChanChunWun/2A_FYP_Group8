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
    public VolumetricLines.VolumetricLineBehavior light;
    public bool Shoot;
    // Start is called before the first frame update
    void Start()
    {
        m_Collider = shootpos.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Extents = new Vector3(_Extents/2, _Extents/2, shootpos.transform.localScale.z);
        if (Shoot)
        {
            m_HitDetect = Physics.BoxCast(m_Collider.bounds.center, Extents, shootpos.transform.forward, out m_Hit, shootpos.transform.rotation, 2000);
            light.LineWidth = _Extents * 9;
            if (m_HitDetect)
            {
                //light.StartPos = new Vector3(0, 0, m_Hit.distance / 1.5f);
                light.EndPos = new Vector3(0, 0, m_Hit.distance + 0.5f);
                Debug.Log("Hit : " + m_Hit.collider.name);
            }
            else
            {
                //light.StartPos = new Vector3(0, 0, 15);
                light.EndPos = new Vector3(0, 0, 4000);

            }

        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 Extents = new Vector3(_Extents, _Extents, shootpos.transform.localScale.z);
        if (m_HitDetect)
        {
            Gizmos.DrawRay(shootpos.transform.position, shootpos.transform.forward * m_Hit.distance);
            Gizmos.DrawWireCube(shootpos.transform.position + shootpos.transform.forward * m_Hit.distance, Extents);
        }
        else
        {
            Gizmos.DrawRay(shootpos.transform.position, shootpos.transform.forward * 2000);
            Gizmos.DrawWireCube(shootpos.transform.position + shootpos.transform.forward * 2000, Extents);
        }
    }
}
