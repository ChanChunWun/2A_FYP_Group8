using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurretController : MonoBehaviour
{
    float MouseX;
    float MouseY;
    public GameObject Turret;
    public float ms = 60;
    int CamPos = 0;
    Camera MyCam;

    // Start is called before the first frame update
    void Start()
    {
        MyCam = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.None;
        SetcamPos();
    }

    // Update is called once per frame
    void Update()
    {
        MouseX = Input.GetAxis("Mouse X") * ms * Time.deltaTime;
        MouseY = Input.GetAxis("Mouse Y") * ms * Time.deltaTime;
        Turret.GetComponent<TurretSystem>().ControlTurret(MouseY, MouseX);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Turret.GetComponent<TurretSystem>().Shoot(gameObject);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (MyCam.fieldOfView > Turret.GetComponent<TurretSystem>().ZoomFoV)
            {
                MyCam.fieldOfView -= 500 * Time.deltaTime;
            }
            else
            {
                MyCam.fieldOfView = Turret.GetComponent<TurretSystem>().ZoomFoV;
            }
        }
        else
        {
            if (MyCam.fieldOfView < 60)
            {
                MyCam.fieldOfView += 500 * Time.deltaTime;
            }
            else
            {
                MyCam.fieldOfView = 60;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(2);
        }
    }

    void SetcamPos()
    {
        gameObject.transform.SetParent(Turret.GetComponent<TurretSystem>().CamerPos[CamPos]);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    void ChangeWeapon(int No)
    {
        Turret.GetComponent<TurretSystem>().SetUseNo(No);
    }
}
