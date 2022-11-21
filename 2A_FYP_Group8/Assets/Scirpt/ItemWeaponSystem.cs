using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeaponSystem : MonoBehaviour
{
    public GameObject RWeapon;
    public GameObject LWeapon;

    public float Level = 0;
    public float MaxLevel = 20;

    [SerializeField]
    private float ShootSpeed = 450f;
    [SerializeField]
    private float Deviation = 0.001f;
    [SerializeField]
    private float Damage = 10f;
    [SerializeField]
    private float TrueDamage = 10f;
    public int BulletNum = 1;
    public float AddHeat = 0.01f;
    public float CoolAdd = 0.02f;
    public float FullHeatCoolTime = 3f;
    public float DamageMult = 2f;
    public float TrueDamageMult = 2f;

    // Start is called before the first frame update
    public GameObject GetWeaponWeapon(GameObject WeaponPos)
    {
        if (WeaponPos.tag == "RightWeapon")
        {
            return RWeapon;
        }
        else if (WeaponPos.tag == "LeftWeapon")
        {
            return LWeapon;
        }
        else
        {
            return null;
        }
    }

    public object[] SendWeaponData()
    {
        object[] WeaponData = new object[9];
        WeaponData[0] = ShootSpeed * (1 + (Level * 0.1f));
        WeaponData[1] = Deviation;
        WeaponData[2] = Damage * (1 + (Level * 0.25f));
        WeaponData[3] = TrueDamage * (1 + (Level * 0.25f));
        WeaponData[4] = AddHeat * (1 - (Level * 0.15f));
        WeaponData[5] = CoolAdd * (1 - (Level * 0.15f));
        WeaponData[6] = FullHeatCoolTime * (1 - (Level * 0.1f));
        WeaponData[7] = DamageMult * (1 + (Level * 0.1f));
        WeaponData[8] = TrueDamageMult * (1 + (Level * 0.1f));
        //Caller.SendMessage("GetWeaponData", WeaponData);
        return WeaponData;
    }
}
