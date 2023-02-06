using UnityEngine;
using UnityEngine.AI;

public class NavAgentDemo : MonoBehaviour
{
    public Transform destination;

    private NavMeshAgent navMeshAgent;

    void Awake()
        => navMeshAgent = GetComponent<NavMeshAgent>();

    private void Start()
    {
        if (destination != null)
        {
            bool possible = navMeshAgent.SetDestination(destination.position);
            if (!possible)
                Debug.LogWarning("Path is not possible");
        }
    }
}
