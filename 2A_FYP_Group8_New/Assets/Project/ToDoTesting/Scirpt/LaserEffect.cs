using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEffect : MonoBehaviour
{
    public Transform endPosition;

    public float LineWidth = 0;
    public float LaserRange = 0;
    public Vector3 endPoint;
    public Transform FinEndpoint;
    public bool hit;

    //ParticleSystem laserLine;
    LineRenderer laserLine;


    // Start is called before the first frame update
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ShootLaser();
    }

    public void ShootLaser()
    {
        //var main = laserLine.main;
        //main.startSize = width;
        laserLine.startWidth = LineWidth;
        laserLine.SetPosition(0, transform.position);
        //Vector3 endPoint = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + range);
        if (hit)
        {
            laserLine.SetPosition(1, endPoint);
        }
        else
        {
            laserLine.SetPosition(1, FinEndpoint.position);
        }
        //main.startLifetime = range;
        //main.simulationSpeed = range;
    } 
}
