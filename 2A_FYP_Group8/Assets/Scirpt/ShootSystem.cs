using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    public GameObject NowWeapon;
    public GameObject MainCam;
    public Transform NomalPos;
    public Transform AimPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            NowWeapon.transform.position = AimPos.position;
            MainCam.GetComponent<Camera>().fieldOfView = 40f;
        }
        else
        {
            NowWeapon.transform.position = NomalPos.position;
            MainCam.GetComponent<Camera>().fieldOfView = 60f;
        }
        if (Input.GetButton("Fire1"))
        {
            Debug.Log("Pull Fire1");
            if (NowWeapon != null)
            {
                NowWeapon.GetComponent<ShootWeapon>().Shoot();
            }
        }
       
    }

    public void Recoil(float GunForce)
    {
        MainCam.transform.Rotate(GunForce, GunForce, 0f);
    }

   
}
