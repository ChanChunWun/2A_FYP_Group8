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
    public AudioClip ChargeSound;
    float CoolTimeCount = 0;
    float ChargeCoolTimeCount = 0;
    public bool isCharge;
    public bool haveChargeAnim;
    public float ChargeTime;
    public float DamageMult = 2f;
    public float TrueDamageMult = 2f;
    public float ForceMult = 1.5f;
    List<Material> HeatMats = new List<Material>();
    bool Charging;
    Transform Sposition;
    GameObject MyBullet;
    GameObject user;
    float ChargeAnimCount = 0;
    public Transform ShellOutPos;
    public GameObject Shell;
    public bool HaveShell;
    public bool missileLuncher;
    public List<Transform> MissilePos;
    public GameObject Missiles;
    List<GameObject> ShowMissile;
    public float MissileRespawnTime = 10f;
    List<float> RespawnCount;
    int NowMissile = 0;
    GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        ShootSp = 1 / (ShootSpeed / 60);
        Deviation += BulletOj.GetComponent<Bullet>().AddAccuracy;
        SetHeatMat();
        MyBullet = BulletOj;
        if (haveChargeAnim)
        {
            anim = GetComponent<Animator>();
        }
        if (missileLuncher)
        {
            setMissilePos();
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
        if (isCharge == true)
        {
            ChargeCoolDown();
        }
        if (missileLuncher)
        {
            MissileReSpawnCount();
        }
        else
        {
            CoolDown();
        }
        
    }

    public void Shoot(GameObject Player, Transform shootposition)
    {
        user = Player;
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
                        Shoot(shootposition);
                        //Audio.PlayOneShot(ShootSound);
                    }
                    Audio.PlayOneShot(ShootSound);
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
                else if (missileLuncher == true)
                {
                    if (target != null)
                    {
                        ShowMissile[NowMissile].GetComponent<Bullet>().Fire(target);
                    }
                }
                else
                {
                    ChargeCount += Time.deltaTime;
                    ChargeCoolTimeCount = 0;


                    if (ChargeCount >= ChargeTime)
                    {
                        if (haveChargeAnim)
                        {
                            //anim.SetBool("Charging", false);
                        }
                        SetBulletData(DamageMult, TrueDamageMult, ForceMult);
                        Shoot(shootposition);
                        ChargeCount = 0;
                        Heat += 1f;
                        CantUse = true;
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
                        Audio.PlayOneShot(ShootSound);
                    }
                    else
                    {
                        if (!Audio.isPlaying)
                        {
                            Audio.clip = ChargeSound;
                            Audio.Play();
                        }
                        if (haveChargeAnim)
                        {
                            //anim.SetBool("Charging", true);
                            ChargeAnimCount += Time.deltaTime * ( 1 / ChargeTime );
                            anim.SetFloat("Charge", ChargeAnimCount);
                        }
                    }
                }
            }
        }
    }

    public void ChargeNotFullShoot(GameObject Player, Transform shootposition)
    {
        if (ChargeCount < 0.2f && ChargeCount > 0)
        {
            if (haveChargeAnim)
            {
                anim.SetBool("Charging", false);
            }
            Audio.clip = null;
            Audio.Stop();
            ChargeCount = 0;
            ShootCount = 0;
            SetBulletData(1, 1, 1);
            Shoot(shootposition);
            Heat += AddHeat;
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
            Audio.PlayOneShot(ShootSound);
        }
    }

    void ChargeCoolDown()
    {
        ChargeCoolTimeCount += Time.deltaTime;
        if (ChargeCoolTimeCount >= 0.1f)
        {
            if(ChargeCount > 0)
            {
                ChargeCount -= Time.deltaTime * ChargeTime;
            }
            else if(ChargeCount < 0)
            {
                ChargeCount = 0;
            }
            
            if (haveChargeAnim)
            {
                if (ChargeAnimCount > 0)
                {
                    ChargeAnimCount -= Time.deltaTime * (1 / ChargeTime / 2);
                    anim.SetFloat("Charge", ChargeAnimCount);
                }
                else if (ChargeAnimCount < 0)
                {
                    ChargeAnimCount = 0;
                    anim.SetFloat("Charge", ChargeAnimCount);
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
        //Debug.Log("Cool");
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

    public void SetTarget (GameObject tar)
    {
        target = tar;
    }
    void Shoot(Transform shootpos)
    {
        Quaternion direction = shootpos.transform.rotation;
        direction.x += Random.Range(-Deviation, Deviation);
        direction.y += Random.Range(-Deviation, Deviation);
        direction.z += Random.Range(-Deviation, Deviation);
        GameObject ShootedBullet = Instantiate(MyBullet, shootpos.transform.position, direction);
        Rigidbody ShootedBulletRB = ShootedBullet.GetComponent<Rigidbody>();
        ShootedBulletRB.AddForce(direction * Vector3.forward * ShootedBullet.GetComponent<Bullet>().ShootForce, ForceMode.Impulse);
        Destroy(ShootedBullet, 5f);
        if (HaveShell == true)
        {
            ShellOut();
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
        MyBullet.GetComponent<Bullet>().SetData(Damage * DamMul, TrueDamage * TDamMul, FireForce);
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

    void setMissilePos()
    {
        Debug.Log("Missile Seting");
        float RespC = 0;
        for (int i = 0; i < MissilePos.Count; i++)
        {
            RespawnCount.Add(RespC);
            GameObject SpMiss = Instantiate(Missiles, MissilePos[i].position, MissilePos[i].rotation);
            SpMiss.transform.parent = MissilePos[i];
            ShowMissile.Add(SpMiss);
        }

    }

    void MissileReSpawnCount()
    {
        for (int i = 0; i < RespawnCount.Count; i++)
        {
            if (ShowMissile[i] == null)
            {
                if (RespawnCount[i] < MissileRespawnTime)
                {
                    RespawnCount[i] += Time.deltaTime;
                }
                if (RespawnCount[i] >= MissileRespawnTime)
                {
                    GameObject SpMiss = Instantiate(Missiles, MissilePos[i].position, MissilePos[i].rotation);
                    SpMiss.transform.parent = MissilePos[i];
                    ShowMissile[i] = SpMiss;
                    RespawnCount[i] = 0;
                }
            }
        }
        
    }

    void ShellOut()
    {
        Quaternion direction = ShellOutPos.transform.rotation;
        //direction.x = -direction.x;
        direction.y = direction.y + 180;
        //direction.z = -direction.z;

        GameObject ShellOj = Instantiate(Shell, ShellOutPos.transform.position, ShellOutPos.transform.rotation);
        Rigidbody ShellOjRB = ShellOj.GetComponent<Rigidbody>();
        ShellOjRB.AddForce(-Vector3.up * 5, ForceMode.Impulse);
        Destroy(ShellOj, 4.5f);
    }

    public void SetData(object[] datas)
    {
        ShootSpeed = (float)datas[0];
        Deviation = (float)datas[1];
        Damage = (float)datas[2];
        TrueDamage = (float)datas[3];
        AddHeat = (float)datas[4];
        CoolAdd = (float)datas[5];
        FullHeatCoolTime = (float)datas[6];
        DamageMult = (float)datas[7];
        TrueDamageMult = (float)datas[8];
    }
}
