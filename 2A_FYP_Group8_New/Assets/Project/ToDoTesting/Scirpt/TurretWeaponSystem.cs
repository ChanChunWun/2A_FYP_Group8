using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWeaponSystem : MonoBehaviour
{
    #region Audio
    AudioSource Audio;
    #endregion
    Animator anim;
    public bool AiUse = false;
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

    [Header("Missile")]
    public bool missileLuncher;
    public List<Transform> MissilePos;
    public GameObject Missiles;
    List<GameObject> ShowMissile = new List<GameObject>();
    public float MissileRespawnTime = 10f;
    List<float> RespawnCount = new List<float>();
    int NowMissile = 0;
    GameObject target;

    [Header("LazerWeapon")]
    public bool LaserWeapon;
    public float _Extents = 3;
    public Transform shrinker;
    bool m_HitDetect;
    Collider m_Collider;
    RaycastHit m_Hit;
    public VolumetricLines.VolumetricLineBehavior light;
    public float fulllaserTime = 2.5f; 
    float laserCount = 0;
    float damageCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        ShootSp = 1 / (ShootSpeed / 60);
        Deviation += BulletOj.GetComponent<Bullet>().AddAccuracy;
        SetHeatMat();
        if (BulletOj!=null)
        MyBullet = BulletOj;
        if (haveChargeAnim)
        {
            anim = GetComponent<Animator>();
        }
        if (missileLuncher)
        {
            setMissilePos();
        }
        if (LaserWeapon)
        {
            m_Collider = FirePos[0].GetComponent<Collider>();
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

    public void Shoot(GameObject Player, Transform shootposition, Camera cam)
    {
        user = Player;
        if (CantUse != true)
        {
            if (LaserWeapon == true)
            {

            }
        }
        if (ShootCount >= ShootSp)
        {
            if (CantUse != true)
            {
                if (isCharge == true)
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
                        if (shootposition == null)
                            Shoot(FirePos[FireCount]);
                        else
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
                            ChargeAnimCount += Time.deltaTime * (1 / ChargeTime);
                            anim.SetFloat("Charge", ChargeAnimCount);
                        }
                    }
                }
                else if (missileLuncher == true)
                {
                    if (target != null)
                    {
                        if (ShowMissile[NowMissile] != null)
                        {
                            ShootCount = 0;
                            ShowMissile[NowMissile].GetComponent<Bullet>().Fire(target);
                            Audio.PlayOneShot(ShootSound);
                            if (NowMissile < (MissilePos.Count - 1))
                            {
                                NowMissile++;
                            }
                            else
                            {
                                NowMissile = 0;
                            }
                        }

                    }
                }
                else if (LaserWeapon == true)
                {
                    CoolTimeCount = 0;
                    Debug.Log(cam.gameObject.name);
                    RaycastHit hit;
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    float currentAngle = shrinker.transform.localEulerAngles.z;
                    currentAngle = (currentAngle > 180) ? currentAngle - 360 : currentAngle;
                    if (currentAngle > 360)
                    {
                        currentAngle = 360;
                    }
                    else if (currentAngle < -360)
                    {
                        currentAngle = -360;
                    }
                    shrinker.Rotate(0, 0, 600 * Time.deltaTime);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        FirePos[0].LookAt(hit.point);
                        Debug.Log("Did Hit");
                    }
                    else
                    {
                        //Vector3 lookPos = new Vector3(shootposition.localPosition.x + 2000, shootposition.localPosition.y, shootposition.localPosition.z );
                        FirePos[0].localRotation = Quaternion.Euler(0, 0, 0);
                        Debug.Log("Did not Hit");
                    }
                    Vector3 Extents = new Vector3(_Extents / 2, _Extents / 2, FirePos[0].localScale.z);

                    m_HitDetect = Physics.BoxCast(m_Collider.bounds.center, Extents, FirePos[0].forward, out m_Hit, FirePos[0].rotation, 2000);
                    light.LineWidth = _Extents * 7;




                    if (m_HitDetect)
                    {
                        Lasershoot();
                        //light.StartPos = new Vector3(0, 0, m_Hit.distance / 1.5f);
                        light.EndPos = new Vector3(0, 0, m_Hit.distance + 0.5f);
                        Debug.Log("hit range: " + m_Hit.distance);
                        Debug.Log("Hit : " + m_Hit.collider.name);
                    }
                    else
                    {
                        Lasershoot();
                        //light.StartPos = new Vector3(0, 0, 15);
                        light.EndPos = new Vector3(0, 0, 2000);

                    }
                
                }
                else
                {
                    ShootCount = 0;
                    Heat += AddHeat;
                    SetBulletData(1, 1, 1);
                    for (int i = 0; i < BulletNum; i++)
                    {
                        if (shootposition == null)
                            Shoot(FirePos[FireCount]);

                        else
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
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (LaserWeapon)
        {
            if (m_HitDetect)
            {
                //Draw a Ray forward from GameObject toward the hit
                Gizmos.DrawRay(FirePos[0].transform.position, FirePos[0].transform.forward * m_Hit.distance);
                //Draw a cube that extends to where the hit exists
                Gizmos.DrawWireCube(FirePos[0].transform.position + FirePos[0].transform.forward * m_Hit.distance, FirePos[0].transform.localScale);
            }
            //If there hasn't been a hit yet, draw the ray at the maximum distance
            else
            {
                //Draw a Ray forward from GameObject toward the maximum distance
                Gizmos.DrawRay(FirePos[0].transform.position, FirePos[0].transform.forward * 2000);
                //Draw a cube at the maximum distance
                Gizmos.DrawWireCube(FirePos[0].transform.position + FirePos[0].transform.forward * 2000, FirePos[0].transform.localScale);
            }

        }
        //Check if there has been a hit yet
    }

    public void LaserShootCountStop()
    {
        laserCount = 0;
        light.EndPos = new Vector3(0, 0, 0);
        FirePos[0].rotation = new Quaternion(0, 0, 0 ,1);
    }

    void Lasershoot()
    {
        object[] damagedata = new object[2];
        damageCount += Time.deltaTime;
        laserCount += Time.deltaTime;
        if (laserCount >= fulllaserTime)
        {
            damagedata[0] = Damage /10;
            damagedata[1] = TrueDamage /10;
            if (damageCount >= ShootSp)
            {

                m_Hit.collider.gameObject.SendMessage("hit", damagedata);
                damageCount = 0;
                Heat += AddHeat/10;
            }
            
        }
        else
        {
            damagedata[0] = Damage;
            damagedata[1] = TrueDamage;
            m_Hit.collider.gameObject.SendMessage("hit", damagedata);
            Heat += AddHeat;
            damageCount = 0;
            laserCount = 0;
        }
    }

    public void ChargeNotFullShoot(GameObject Player, Transform shootposition)
    {
        if (LaserWeapon)
        {

        }
        else
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
        if (BulletOj != null)
        {
            Quaternion direction = shootpos.transform.rotation;
            direction.x += Random.Range(-Deviation, Deviation);
            direction.y += Random.Range(-Deviation, Deviation);
            direction.z += Random.Range(-Deviation, Deviation);
            GameObject ShootedBullet = Instantiate(MyBullet, shootpos.transform.position, direction);
            ShootedBullet.SendMessage("SetShooter", user, SendMessageOptions.DontRequireReceiver);
            Rigidbody ShootedBulletRB = ShootedBullet.GetComponent<Rigidbody>();
            ShootedBulletRB.AddForce(direction * Vector3.forward * ShootedBullet.GetComponent<Bullet>().ShootForce, ForceMode.Impulse);
            Destroy(ShootedBullet, 5f);
            if (HaveShell == true)
            {
                ShellOut();
            }
        }
        else
        {
            Debug.Log("No Bullet obj");
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
        if (MyBullet != null)
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
        float RespC;
        for (int i = 0; i < MissilePos.Count; i++)
        {
            RespC = 0;
            RespawnCount.Add(RespC);
            GameObject SpMiss = Instantiate(Missiles, MissilePos[i].position, MissilePos[i].rotation);
            SpMiss.transform.parent = MissilePos[i];
            SpMiss.transform.position = new Vector3(0, 0, 0);
            SpMiss.transform.localScale = new Vector3(1,1,1);
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
