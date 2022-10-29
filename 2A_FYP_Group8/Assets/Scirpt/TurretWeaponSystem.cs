using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWeaponSystem : MonoBehaviour
{
    AudioSource Audio;
    public Transform FirePos;
    [SerializeField]
    private float ShootSpeed = 450f;
    [SerializeField]
    private float Deviation = 0.001f;
    public List<GameObject> HeatBarrel;
    public GameObject BulletOj;
    public float AddHeat = 0.01f;
    float Heat = 0;
    float ShootSp;
    float ShootCount;
    bool CantUse = false;
    public GameObject FireEffect;
    public AudioClip ShootSound;
    public AudioClip ReloadSound;
    float CoolTimeCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        ShootSp = 1 / (ShootSpeed / 60);
        Deviation += BulletOj.GetComponent<Bullet>().AddAccuracy;
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
                ShootCount = 0;
                Heat += AddHeat;
                for (int i = 0; i < BulletOj.GetComponent<Bullet>().BulletNum; i++)
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
                    GameObject fire = Instantiate(FireEffect, FirePos.transform.position, FirePos.transform.rotation);
                    fire.transform.SetParent(FirePos);
                    Destroy(fire, 0.1f);
                    //Audio.PlayOneShot(ShootSound);
                }
                for (int i = 0; i < HeatBarrel.Count; i++)
                {
                    HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color = new Color(HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color.r, HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color.g, HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color.b, Heat);
                }
                CoolTimeCount = 0;
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
                HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color = new Color(HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color.r, HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color.g, HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color.b, Heat);
            }
        }
        Debug.Log("Cool");
        if (Heat >= 1)
        {
            CantUse = true;
            if (CoolTimeCount >= 3f)
            {
                if (Heat > 0)
                {
                    Heat -= AddHeat * 5 * Time.deltaTime;
                    for (int i = 0; i < HeatBarrel.Count; i++)
                    {
                        HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color = new Color(HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color.r, HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color.g, HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color.b, Heat);
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
                    Heat -= AddHeat * 5 * Time.deltaTime;
                    for (int i = 0; i < HeatBarrel.Count; i++)
                    {
                        HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color = new Color(HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color.r, HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color.g, HeatBarrel[i].GetComponent<MeshRenderer>().materials[0].color.b, Heat);
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

    
}
