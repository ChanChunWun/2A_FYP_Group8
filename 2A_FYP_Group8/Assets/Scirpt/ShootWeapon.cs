using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWeapon : MonoBehaviour
{
    AudioSource Audio;
    public GameObject chamber = null;
    public List<string> GunTag;
    public bool AutoFire;
    public bool CanUse;
    public float ShootSpeed = 600f;
    public List<string> GunPartTag;
    public List<GameObject> GunParts;
    public List<Transform> GunPartPos; // Gun Part position
    List<GameObject> ShowGunPart;
    public List<bool> Must;
    public Transform MagPos; //magazine position
    public GameObject Magazine;
    public float RecoilForce;
    float FixedRecoilForce;
    float ShootSp, ShootCount;
    float Deviation = 0;
    GameObject shootposition = null;
    GameObject Fireposition = null;
    public AudioClip ShootSound;
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        FixedRecoilForce = RecoilForce / 100;
        Audio = GetComponent<AudioSource>();
        ShootCount = 0;
        ShootSp = 1 / (ShootSpeed / 60);
        ShowGunPart = new List<GameObject>();
        LoadGunPart();
        CheckGunData();
        if (chamber == null)
        {
            chamber = Magazine.GetComponent<GunPart>().Ammo[0];
            Magazine.GetComponent<GunPart>().PullupAmmo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ShootCount <= 5f)
        {
            ShootCount += Time.deltaTime;
        }
    }

    void LoadGunPart()
    {
        GameObject Mag = Instantiate(Magazine, MagPos.position, MagPos.rotation);
        Mag.transform.parent = MagPos;
        for (int i = 0; i < GunParts.Count; i++)
        {
            GameObject PartofGun = Instantiate(GunParts[i], GunPartPos[i].position, GunPartPos[i].rotation);
            PartofGun.transform.parent = GunPartPos[i];
            ShowGunPart.Add(PartofGun);
        }
    }

    public void Shoot()
    {
        if(AutoFire == true)
        {
            if (ShootCount >= ShootSp)
            {
                ShootCount = 0;
                shootBu(); //Shooting
            }
        }
    }

    void shootBu()
    {
        Quaternion direction = shootposition.transform.rotation;
        direction.x += Random.Range(-Deviation, Deviation);
        direction.y += Random.Range(-Deviation, Deviation);
        direction.z += Random.Range(-Deviation, Deviation);
        GameObject ShootedBullet = Instantiate(chamber, shootposition.transform.position, direction);
        Rigidbody ShootedBulletRB = ShootedBullet.GetComponent<Rigidbody>();
        ShootedBulletRB.AddForce(direction * Vector3.forward * ShootedBullet.GetComponent<Bullet>().ShootForce, ForceMode.Impulse);
        chamber = Magazine.GetComponent<GunPart>().Ammo[0];
        Magazine.GetComponent<GunPart>().PullupAmmo();
        Audio.PlayOneShot(ShootSound);
        Player.GetComponent<ShootSystem>().Recoil(FixedRecoilForce);
    }

    void CheckGunData()
    {
        for (int i = 0; i < GunParts.Count; i++)
        {
            if (ShowGunPart[i].GetComponent<GunPart>().MainPartTag == "Barrel")
            {
                Deviation = ShowGunPart[i].GetComponent<GunPart>().Accuracy;
                shootposition = ShowGunPart[i].GetComponent<GunPart>().ShootPos;
                break;
            }
        }
    }

    void CheckCanUse()
    {
        for (int i = 0; i < GunParts.Count; i++)
        {
            if (GunParts[i] == null && Must[i] == true)
            {
                CanUse = true;
            }
        }
    }
}
    