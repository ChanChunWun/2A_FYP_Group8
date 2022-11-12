using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWeaponSystem : MonoBehaviour
{
    AudioSource Audio;
    Animator anim;
    public List<Transform> FirePos;
    int FireCount = 0;
    [SerializeField]
    private float ShootSpeed = 450f;
    [SerializeField]
    private float Deviation = 0.001f;
    [SerializeField]
    private float Damage = 10f;
    [SerializeField]
    private float TrueDamage = 10f;
    [SerializeField]
    private float FireForce = 15f;
    public int BulletNum = 1;
    public List<GameObject> HeatBarrel;
    public GameObject BulletOj;
    public float AddHeat = 0.01f;
    public float CoolAdd = 0.02f;
    public float FullHeatCoolTime = 3f;
    float Heat = 0;
    float ShootSp;
    float ShootCount;
    float ChargeCount;
    bool CantUse = false;
    public GameObject FireEffect;
    public AudioClip ShootSound;
    public AudioClip ReloadSound;
    float CoolTimeCount = 0;
    public bool isCharge;
    public bool haveChargeAnim;
    public float ChargeTime;
    public float DamageMult = 2f;
    public float TrueDamageMult = 2f;
    public float ForceMult = 1.5f;
    List<Material> HeatMats = new List<Material>();
    bool Charging;

    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        ShootSp = 1 / (ShootSpeed / 60);
        Deviation += BulletOj.GetComponent<Bullet>().AddAccuracy;
        if (haveChargeAnim)
        {
            anim = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ShootCount < 5)
        {
            ShootCount += Time.deltaTime;
        }
        if (CoolTimeCount < 5)
        {
            CoolTimeCount += Time.deltaTime;
        }
        CoolDown();
    }

    public void Shoot(GameObject Player, Transform shootposition)
    {
        if (ShootCount >= ShootSp)
        {
            if (CantUse != true)
            {
                if (isCharge != true)
                {
                    ShootCount = 0;
                    Heat += AddHeat;
                    SetBulletData(1, 1, 1);
                    for (int i = 0; i < BulletNum; i++)
                    {
                        Quaternion direction = shootposition.transform.rotation;
                        direction.x += Random.Range(-Deviation, Deviation);
                        direction.y += Random.Range(-Deviation, Deviation);
                        direction.z += Random.Range(-Deviation, Deviation);
                        GameObject ShootedBullet = Instantiate(BulletOj, shootposition.transform.position, direction);
                        Rigidbody ShootedBulletRB = ShootedBullet.GetComponent<Rigidbody>();
                        ShootedBulletRB.AddForce(direction * Vector3.forward * ShootedBullet.GetComponent<Bullet>().ShootForce, ForceMode.Impulse);
                        Destroy(ShootedBullet, 5f);
                        Audio.PlayOneShot(ShootSound);
                        //Audio.PlayOneShot(ShootSound);
                    }
                    GameObject fire = Instantiate(FireEffect, FirePos[FireCount].transform.position, FirePos[FireCount].transform.rotation);
                    fire.transform.SetParent(FirePos[FireCount]);
                    Destroy(fire, 0.1f);
                    if (FireCount < (FirePos.Count - 1))
                    {
                        FireCount++;
                    }
                    else
                    {
                        FireCount = 0;
                    }
                    for (int i = 0; i < HeatBarrel.Count; i++)
                    {
                        HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color = new Color(HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color.r, HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color.g, HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color.b, Heat);
                    }
                    CoolTimeCount = 0;
                }
                else
                {
                    ChargeCount += Time.deltaTime;
                     
                }
            }
            
        }
    }

    void CoolDown()
    {
        if (Heat < 0)
        {
            Heat = 0;
            for (int i = 0; i < HeatBarrel.Count; i++)
            {
                HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color = new Color(HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color.r, HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color.g, HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color.b, Heat);
            }
        }
        Debug.Log("Cool");
        if (Heat >= 1)
        {
            CantUse = true;
            if (CoolTimeCount >= FullHeatCoolTime)
            {
                if (Heat > 0)
                {
                    Heat -= CoolAdd * 5 * Time.deltaTime;
                    for (int i = 0; i < HeatBarrel.Count; i++)
                    {
                        HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color = new Color(HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color.r, HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color.g, HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color.b, Heat);
                    }
                }
            }
        }
        else if (Heat < 1)
        {
            CoolTimeCount += Time.deltaTime;
            if (CoolTimeCount >= 0.75f)
            {
                if (Heat > 0)
                {
                    Heat -= CoolAdd * 5 * Time.deltaTime;
                    for (int i = 0; i < HeatBarrel.Count; i++)
                    {
                        HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color = new Color(HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color.r, HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color.g, HeatBarrel[i].GetComponent<MeshRenderer>().materials[1].color.b, Heat);
                    }
                }
            }
        }

        if (Heat == 0)
        {
            CantUse = false;
        }
    }

    public void PlayCoolsound()
    {

    }

    public float GetHeat()
    {
        return Heat;
    }

    void SetBulletData(float DamMul, float TDamMul, float ForMul)
    {
        BulletOj.GetComponent<Bullet>().SetData(Damage * DamMul, TrueDamage * TDamMul, FireForce * ForMul);
    }

    void SetHeatMat()
    {
        Material adds;
        for (int i = 0; i < HeatBarrel.Count; i++)
        {
            adds = HeatBarrel[i].GetComponent<MeshRenderer>().materials[1];
            HeatMats.Add(adds);
            HeatBarrel[i].GetComponent<MeshRenderer>().materials[1] = HeatMats[i];
        }
    }
}
