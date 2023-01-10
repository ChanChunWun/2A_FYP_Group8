using UnityEngine;
using UnityEngine.UI;

public class PlayerTurretController : MonoBehaviour
{
    float MouseX;
    float MouseY;

    public GameObject Turret;
    public Image RightHeatImage;
    public Image LeftHeatImage;
    public float ms = 60;

    private TurretSystem TurretSystem;
    private Camera MyCam;
    int CamPos = 0;

    bool slowMo = false;
    bool UsingTurret = false;

    private void Awake()
    {
        MyCam = GetComponent<Camera>();
        TurretSystem = Turret.GetComponent<TurretSystem>();
    }

    void Start()
    {
        MyCam.gameObject.SetActive(false);
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
            HeatLine(TurretSystem.GetRightHeat(), TurretSystem.GetLeftHeat());
            MouseX = Input.GetAxis("Mouse X") * ms * Time.deltaTime;
            MouseY = Input.GetAxis("Mouse Y") * ms * Time.deltaTime;
            TurretSystem.ControlTurret(MouseY, MouseX);

            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (MyCam.fieldOfView > TurretSystem.ZoomFoV)
                {
                    MyCam.fieldOfView -= 500 * Time.deltaTime;
                }
                else
                {
                    MyCam.fieldOfView = TurretSystem.ZoomFoV;
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
    }

    void SetcamPos()
    {
        gameObject.transform.SetParent(TurretSystem.CamerPos[CamPos]);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    void ChangeWeapon(int No)
    {
        TurretSystem.SetUseNo(No);
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

        MyCam.gameObject.SetActive(tf);
        UsingTurret = tf;
    }

    public void Fire(bool value)
    {
        if (value)
            TurretSystem.Shoot(gameObject);
        else
            TurretSystem.ChargeNotFullShoot(gameObject);
    }
}
