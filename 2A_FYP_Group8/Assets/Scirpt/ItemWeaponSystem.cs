using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeaponSystem : MonoBehaviour
{
    public GameObject RWeapon;
    public GameObject LWeapon;
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
}
