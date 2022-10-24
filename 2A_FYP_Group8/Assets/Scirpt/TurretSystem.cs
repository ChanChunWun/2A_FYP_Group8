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
    public Transform[] Heads;
    public Transform AllTurretPos;
    public Transform AllWeaponPos;
    public float Depression = 8f;   //俯角
    public float Elevation = 10f;   //仰角
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
        if (n-1 <= TurretObject.Count)
        {
            UsingNo = n - 1;
        }
    }
}
