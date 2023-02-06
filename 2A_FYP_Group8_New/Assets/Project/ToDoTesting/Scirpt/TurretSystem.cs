using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretSystem : MonoBehaviour
{
    public float ZoomFoV = 20;
    public bool HeavyTurret = false;
    public Transform ShootPos;
    public List<GameObject> TurretObject;
    public List<GameObject> WeaponPos;
    public Image[] HeatList;
    public Transform[] CamerPos;
    public Transform[] Heads;
    public Transform AllTurretPos;
    public Transform AllWeaponPos;
    public float Depression = 8f;   //俯角
    public float Elevation = 10f;   //仰角
    GameObject User;
    int UsingNo = 0;
    float xRotat = 0;
    List<GameObject> ShowWeapons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        ShowWeapon();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot(GameObject Player, Camera cam)
    {
        User = Player;
        ShowWeapons[UsingNo].GetComponent<TurretWeaponSystem>().Shoot(Player, ShootPos, cam);
    }

    public void ChargeNotFullShoot(GameObject Player)
    {
        User = Player;
        ShowWeapons[UsingNo].GetComponent<TurretWeaponSystem>().ChargeNotFullShoot(Player, ShootPos);
    }
    public void LaserShootCountStop()
    {
        ShowWeapons[UsingNo].GetComponent<TurretWeaponSystem>().LaserShootCountStop();

    }

    public void SetUser(GameObject Player)
    {
        User = Player;
    }

    public void ControlTurret(float MouseY, float MouseX)
    {
        xRotat -= MouseY;
        xRotat = Mathf.Clamp(xRotat, -Elevation, Depression);
        AllWeaponPos.transform.localRotation = Quaternion.Euler(xRotat, 0f, 0f);
        for (int i = 0; i < Heads.Length; i++)
        {
            Heads[i].transform.localRotation = Quaternion.Euler(xRotat, 0f, 0f);
        }
        AllTurretPos.transform.Rotate(Vector3.up * MouseX);
    }

    public void SetUseNo(int n)
    {
        if (n - 1 <= TurretObject.Count)
        {
            UsingNo = n - 1;
        }
    }

    public float GetRightHeat()
    {
        float ReturnHeat = 0;
        for (int i = 0; i < WeaponPos.Count; i++)
        {
            if (WeaponPos[i].tag == "RightWeapon")
            {
                ReturnHeat = ShowWeapons[i].GetComponent<TurretWeaponSystem>().GetHeat();
            }
        }
        return ReturnHeat;
    }

    public float GetLeftHeat()
    {
        float ReturnHeat = 0;
        for (int i = 0; i < WeaponPos.Count; i++)
        {
            if (WeaponPos[i].tag == "LeftWeapon")
            {
                ReturnHeat = ShowWeapons[i].GetComponent<TurretWeaponSystem>().GetHeat();
            }
        }
        return ReturnHeat;
    }

    public void SetWeapon(GameObject Weapon, int nos)
    {
        TurretObject[nos] = Weapon;
    }

    void ShowWeapon()
    {
        for (int i = 0; i < TurretObject.Count; i++)
        {
            if (TurretObject[i] != null)
            {
                GameObject ShowWeapon = Instantiate(TurretObject[i].GetComponent<ItemWeaponSystem>().GetWeaponWeapon(WeaponPos[i]), WeaponPos[i].transform.position, WeaponPos[i].transform.localRotation);
                ShowWeapons.Add(ShowWeapon);
                ShowWeapon.GetComponent<TurretWeaponSystem>().SetData(TurretObject[i].GetComponent<ItemWeaponSystem>().SendWeaponData());
                ShowWeapon.transform.SetParent(WeaponPos[i].transform);
                ShowWeapon.transform.localPosition = new Vector3(0, 0, 0);
                ShowWeapon.transform.localRotation = Quaternion.identity;
                ShowWeapon.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
