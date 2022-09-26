using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GunPart : MonoBehaviour
{
    public string MainPartTag;
    public List<string> GunPartTag;
    public List<GameObject> GunParts;
    public List<Transform> GunPartPos;
    public List<bool> Must;
    public float Accuracy = 0.1f;
    public float LessRecoil = 0f;
    public List<GameObject> Ammo;
    int NowAmmo = 0;
    public GameObject ShootPos;
    public GameObject FirePos;

    // Start is called before the first frame update
    void Start()
    {
        LoadGunPart();
    }

    // Update is called once per frame
    void Update()
    {
        if (MainPartTag == "Magazine")
        {
            LoadNowAmmo();
        }
    }

    void LoadGunPart()
    {
        for (int i = 0; i < GunParts.Count; i++)
        {
            GameObject PartofGun = Instantiate(GunParts[i], GunPartPos[i].position, GunPartPos[i].rotation);
            PartofGun.transform.parent = GunPartPos[i];
        }
    }

    void LoadNowAmmo()
    {
        NowAmmo = 0;
        for (int i = 0; i < Ammo.Count; i++)
        {
            if (Ammo[i] != null)
            {
                NowAmmo++;
            }
        }
    }
    public void PullupAmmo()
    {
        for (int i = 1; i < Ammo.Count; i++)
        {
            Ammo[i - 1] = Ammo[i];
            Ammo[i] = null;
        }
    }
}