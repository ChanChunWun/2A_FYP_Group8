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
    public GameObject BulletOj;
    public int MaxAmmoNum = 50;
    int AmmoNum = 0;
    float ShootSp;
    float ShootCount;
    bool CantUse = false;
    public AudioClip ShootSound;
    public AudioClip ReloadSound;

    // Start is called before the first frame update
    void Start()
    {
        ShootSp = 1 / (ShootSpeed / 60);
        AmmoNum = MaxAmmoNum;
        Deviation += BulletOj.GetComponent<Bullet>().AddAccuracy;
    }

    // Update is called once per frame
    void Update()
    {
        if (ShootCount < 5)
        {
            ShootCount += Time.deltaTime;
        }

        if (AmmoNum == 0)
        {
            CantUse = true;
        }
    }

    public void Shoot(GameObject Player, Transform shootposition)
    {
        if (ShootCount >= ShootSp)
        {
            if (AmmoNum > 0)
            {
                if (CantUse != true)
                {
                    for (int i = 0; i < BulletOj.GetComponent<Bullet>().BulletNum; i++)
                    {
                        ShootCount = 0;
                        Quaternion direction = shootposition.transform.rotation;
                        direction.x += Random.Range(-Deviation, Deviation);
                        direction.y += Random.Range(-Deviation, Deviation);
                        direction.z += Random.Range(-Deviation, Deviation);
                        GameObject ShootedBullet = Instantiate(BulletOj, shootposition.transform.position, direction);
                        Rigidbody ShootedBulletRB = ShootedBullet.GetComponent<Rigidbody>();
                        ShootedBulletRB.AddForce(direction * Vector3.forward * ShootedBullet.GetComponent<Bullet>().ShootForce, ForceMode.Impulse);
                        Destroy(ShootedBullet, 5f);
                        AmmoNum--;
                        //Audio.PlayOneShot(ShootSound);
                    }
                }
            }
            else
            {
                Reload();
            }
            
        }
    }

    public void Reload()
    {
        Debug.Log("Reload");
        //Anim.Play("Reload")
        AmmoNum = MaxAmmoNum;
        CantUse = false;
    }

    public void PlayReloadsound()
    {

    }

    
}
