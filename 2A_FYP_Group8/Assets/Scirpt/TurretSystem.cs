using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSystem : MonoBehaviour
{
    public float ZoomFoV = 20;
    public Transform ShootPos;
    public List<GameObject> TurretObject;
    public List<GameObject> WeaponPos;
    public Transform[] CamerPos;
    public Transform AllTurretPos;
    public Transform AllWeaponPos;
    GameObject User;
    int UsingNo = 0;
    float xRotat = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Shoot(GameObject Player)
    {
        User = Player;
        TurretObject[UsingNo].GetComponent<TurretWeaponSystem>().Shoot(Player, ShootPos);
    }

    public void ControlTurret(float MouseY, float MouseX)
    {
        xRotat -= MouseY;
        xRotat = Mathf.Clamp(xRotat, -80, 8);
        AllWeaponPos.transform.localRotation = Quaternion.Euler(xRotat, 0f, 0f);
        AllTurretPos.transform.Rotate(Vector3.up * MouseX);
    }

    public void Reload()
    {
        TurretObject[UsingNo].GetComponent<TurretWeaponSystem>().Reload();
    }
}
