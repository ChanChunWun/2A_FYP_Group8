using UnityEngine;
using UnityEngine.UI;

public class PlayerTurretController : MonoBehaviour
{
    float MouseX;
    float MouseY;

    public Camera MyCam;
    public GameObject Turret;
    public Image RightHeatImage;
    public Image LeftHeatImage;
    public float ms = 60;

    int CamPos = 0;

    bool slowMo = false;
    bool UsingTurret = false;

    public bool testing;
    void Start()
    {
        if (!testing)
        {
            MyCam.transform.gameObject.SetActive(false);
        }
        else
        {
            UsingTurret = true;
        }
        Cursor.lockState = CursorLockMode.None;
        SetcamPos();
        Turret.SendMessage("SetUser", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            slowMo = !slowMo;
            Time.timeScale = slowMo ? 0.15f : 1f;
        }

        if (Turret != null && UsingTurret == true)
        {
            HeatLine(Turret.GetComponent<TurretSystem>().GetRightHeat(), Turret.GetComponent<TurretSystem>().GetLeftHeat());
            MouseX = Input.GetAxis("Mouse X") * ms * Time.deltaTime;
            MouseY = Input.GetAxis("Mouse Y") * ms * Time.deltaTime;
            Turret.GetComponent<TurretSystem>().ControlTurret(MouseY, MouseX);

            if (Input.GetKey(KeyCode.Mouse0))
            {
                Turret.GetComponent<TurretSystem>().Shoot(gameObject, MyCam);
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Turret.GetComponent<TurretSystem>().ChargeNotFullShoot(gameObject);
                Turret.GetComponent<TurretSystem>().LaserShootCountStop();
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
                Turret.GetComponent<TurretSystem>().ChargeNotFullShoot(gameObject);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeWeapon(2);
                Turret.GetComponent<TurretSystem>().ChargeNotFullShoot(gameObject);
            }
        }
    }

    void SetcamPos()
    {
        MyCam.transform.SetParent(Turret.GetComponent<TurretSystem>().CamerPos[CamPos]);
        MyCam.transform.localPosition = new Vector3(0, 0, 0);
    }

    void ChangeWeapon(int No)
    {
        Turret.GetComponent<TurretSystem>().SetUseNo(No);
    }

    void HeatLine(float RightHeat, float LeftHeat)
    {
        RightHeatImage.fillAmount = RightHeat;
        LeftHeatImage.fillAmount = LeftHeat;
    }

    public void SetTurret(GameObject turret1, GameObject Weapon1, GameObject Weapon2)
    {
        DontDestroyOnLoad(gameObject);
        Turret = Instantiate(turret1);
        SetcamPos();
    }

    public void SetUsing(bool tf)
    {
        if (UsingTurret == tf)
            return;

        MyCam.transform.gameObject.SetActive(tf);
        UsingTurret = tf;
    }
}
