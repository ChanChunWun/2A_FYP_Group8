
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
    [Range(1.5f, 4f)]
    public float flyHeightRange = 4.5f;
    public float canShootRange = 50;


    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        //Wp = hand.GetComponent<TurretWeaponSystem>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        lfSys = GetComponent<LifeSystem>();
        fixedFlyHeight = Random.Range(minFlyHeight, flyHeightRange);
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
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        if (!lfSys.dead)
            return;

        agent.SetDestination(transform.position);

        transform.LookAt(player);

        hand.transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

        if (target != null)
        {

            if (Physics.Raycast(shootPoint.position, target.transform.position, out hit, canShootRange))
            {
                if (hit.transform.gameObject == target)
                {
                    weapon.GetComponent<TurretWeaponSystem>().Shoot(gameObject, shootPoint, null);
                }
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
