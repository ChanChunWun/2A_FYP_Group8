using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //public CharacterController controller;
    AudioSource AudioS;
    CapsuleCollider CapCol;
    Rigidbody rb;
    public float speed = 6;
    public float runspeed = 8;
    public float slowspeed = 3;
    float orgspeed;
    public float ms = 100f;
    public float aimms = 20f;
    float orgms;
    public Transform heads;
    public float HP = 100f;
    bool dead = false;
    float xRotat = 0f;
    Transform chest;
    public float SlowSoundRange = 5;
    public float WalkSoundRange = 10;
    public float RunSoundRange = 15;
    public float JumpSoundRange = 15;
    //public Transform target;
    public float H;
    public float V;
    float walkmod = 0;
    float x;
    float y;
    float MouseX;
    float MouseY;
    Animator anim;
    public bool running;
    bool OnGround = true;
    AudioClip[] FootStepWalk;
    AudioClip[] FootStepRun;
    AudioClip FootStepJump;
    GameObject footstepTools;
    // Start is called before the first frame update
    void Start()
    {
        orgms = ms;
        //FootToolsFind();
        footstepTools = GameObject.Find("FootSound");
        rb = GetComponent<Rigidbody>();
        AudioS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        //chest = anim.GetBoneTransform(HumanBodyBones.Chest);
        CapCol = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        orgspeed = speed;
        //AudioS.loop = true;
        AudioS.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            dead = true;
        }
        else
        {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            MouseX = Input.GetAxis("Mouse X") * ms * Time.deltaTime;
            MouseY = Input.GetAxis("Mouse Y") * ms * Time.deltaTime;
            H = Input.GetAxis("Horizontal");
            V = Input.GetAxis("Vertical");
            //chest.transform.LookAt(target);
            //chest.transform.Rotate(new Vector3(0, -90, -90));
            Vector3 MoveDir = new Vector3(H, 0, V);
            float walkmod = Mathf.Clamp01(MoveDir.magnitude);
            if (Input.GetKeyDown(KeyCode.C))
            {

            }
            if (x == 0 && y == 0)
            {
                speed = orgspeed;
                AudioS.clip = null;
                //SoundOut(1);
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    anim.SetFloat("Blend", 0);
                }
                
            }
            else if (x != 0 || y != 0)
            {
                walkmod /= 2;
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
                {
                    walkmod *= 2;
                    speed = runspeed;
                    running = true;
                    //AudioS.loop = true;
                    //RunSound();
                    AudioS.clip = null;
                    //SoundOut(RunSoundRange);
                }
                else
                {
                    //AudioS.loop = true;
                    //WalkSound();
                    //SoundOut(WalkSoundRange);
                    AudioS.clip = null;
                    if (!Input.GetKeyDown(KeyCode.Space))
                    {
                        speed = orgspeed;
                        running = false;
                    }
                }
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                //AudioS.loop = false;
                walkmod = -1;
                speed = slowspeed;
                //SoundOut(SlowSoundRange);
            }
            anim.SetFloat("Blend", walkmod, 0.1f, Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.Space) && OnGround == true)
            {
                Debug.Log("Jump");
                walkmod *= 2;
                running = true;
                //SoundOut(JumpSoundRange);
                //AudioS.loop = false;
                if (AudioS.clip != FootStepJump)
                {
                    Debug.Log("Change Jump");
                    //AudioS.clip = FootStepJump;
                    //AudioS.PlayOneShot(AudioS.clip);
                }
                rb.AddForce(transform.up * 5, ForceMode.Impulse);
            }
        }
    }

    void WalkSound()
    {
        int n = Random.Range(1, FootStepWalk.Length);
        AudioS.clip = FootStepWalk[n];
        AudioS.PlayOneShot(AudioS.clip);
        // move picked sound to index 0 so it's not picked next time
        FootStepWalk[n] = FootStepWalk[0];
        FootStepWalk[0] = AudioS.clip;
    }

    void RunSound()
    {
        int n = Random.Range(1, FootStepRun.Length);
        AudioS.clip = FootStepRun[n];
        AudioS.PlayOneShot(AudioS.clip);
        // move picked sound to index 0 so it's not picked next time
        FootStepRun[n] = FootStepRun[0];
        FootStepRun[0] = AudioS.clip;
    }

    private void FixedUpdate()
    {
        if (dead != true)
        {
            MoveMent();
        }

    }

    void MoveMent()
    {
        float mF = V * speed;
        float mS = H * speed;
        //Vector3 move = transform.right * x + transform.forward * y;
        //controller.Move(move * speed * Time.deltaTime);
        rb.velocity = (transform.forward * mF) + (transform.right * mS) + (transform.up * rb.velocity.y);
        xRotat -= MouseY;
        xRotat = Mathf.Clamp(xRotat, -80, 70);
        heads.transform.localRotation = Quaternion.Euler(xRotat, 0f, 0f);
        transform.Rotate(Vector3.up * MouseX);
    }

    void hit(float damage)
    {
        HP -= damage;
    }

    void GradientFloat(float target)
    {
        if (walkmod < target)
        {
            while(walkmod < target - 0.1f)
            {
                walkmod += 0.001f;
            }
        }
        else if (walkmod > target + 0.1f)
        {
            while (walkmod > target)
            {
                walkmod -= 0.001f;
            }
        }
        else
        {
            
        }
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject)
        {
            OnGround = true;
        }
        else
        {
            OnGround = false;
        }
    }

    private void OnCollisionExit(Collision col)
    {
            OnGround = false;
    }

    /*public void FootToolsFind()
    {
        footstepTools = null;
        footstepTools = GameObject.Find("FootSound");
        FootStepWalk = footstepTools.GetComponent<FootStepSound>().FootWalk;
        FootStepRun = footstepTools.GetComponent<FootStepSound>().FootRun;
        FootStepJump = footstepTools.GetComponent<FootStepSound>().FootJump;
    }*/

    /*void SoundOut(float soundrange)
    {
        //GameObject[] AllAnimals = GameObject.FindGameObjectsWithTag("animal");
        for (int i = 0; i < AllAnimals.Length; i++)
        {
            if (Vector3.Distance(transform.position, AllAnimals[i].transform.position) <= soundrange)
            {
                //AllAnimals[i].GetComponent<GroundAnimal>().RunAway(gameObject);
            }
        }
    }*/

    public void aimMsOn()
    {
        ms = aimms;
    }

    public void aimMsOff()
    {
        ms = orgms;
    }
}
