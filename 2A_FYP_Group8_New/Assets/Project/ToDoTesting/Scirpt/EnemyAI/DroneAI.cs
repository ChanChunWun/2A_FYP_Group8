
using UnityEngine;
using UnityEngine.AI;
//By Ray
public class DroneAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //public GameObject projectile;
    //public Transform projectileSpawnPos;
    public float projectileForce;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //Joe add
    NavMeshAgent nav;
    Animator anim;
    Transform target;
    LifeSystem lfSys;
    float minFlyHeight = 1.5f;
    float fixedFlyHeight;
    RaycastHit hit;

    public Transform hand;
    public GameObject weapon;
    public Transform shootPoint;

    public float speed;
    [Range(1.5f, 7f)]
    public float flyHeightRange = 7f;
    public float canShootRange = 50;


    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        //Wp = hand.GetComponent<TurretWeaponSystem>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        lfSys = GetComponent<LifeSystem>();
        fixedFlyHeight = Random.Range(minFlyHeight, flyHeightRange) + 5;
        agent.baseOffset = fixedFlyHeight;
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        if (lfSys.dead)
        {
            GetComponent<EnemyItemsData>().SetItems();
            Destroy(gameObject);
        }

    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        
        if (Vector3.Distance(player.transform.position, transform.position) > 40)
        {
            agent.speed = speed;
            agent.SetDestination(player.transform.position);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) <= 40)
        {
            agent.speed = 0;
            agent.SetDestination(transform.position);
        }

        float animSpeed = agent.velocity.magnitude / speed;
        anim.SetFloat("Speed", animSpeed);
    }

    private void AttackPlayer()
    {        

        if (Vector3.Distance(player.transform.position, transform.position) > 40)
        {
            agent.speed = speed;
            agent.SetDestination(player.transform.position);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) <= 40)
        {
            agent.speed = 0;
            agent.SetDestination(transform.position);
        }

        float animSpeed = agent.velocity.magnitude / speed;
        anim.SetFloat("Speed", animSpeed);

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.transform.position.z));

        Vector3 playerCenter = new Vector3(player.position.x, player.position.y + 1 , player.position.z);
        hand.transform.LookAt(playerCenter);

        //hand transform.eulerAngles = 
        //hand.transform.rotation = Quaternion.Slerp(hand.transform.rotation, rotation, Time.deltaTime * 2);

        //var pos = player.position - hand.position;
        //var angle = Quaternion.LookRotation(pos, Vector3.up).eulerAngles;
        //hand.transform.rotation = Quaternion.Euler(Vector3.Scale(angle, new Vector3(0, 1, 0)));
        //hand.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(Vector3.Scale(angle, new Vector3(0, 1, 0))));



        Debug.DrawLine(shootPoint.position, shootPoint.transform.forward * canShootRange,Color.red);

            if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward * canShootRange, out hit))
            {
                Debug.Log("Hit" + hit.transform.name);
                if (hit.transform.tag.Equals("Player"))
                {
                    weapon.GetComponent<TurretWeaponSystem>().Shoot(gameObject, shootPoint, null);
                }
            }
        

        

    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
