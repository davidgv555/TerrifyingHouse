using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState { Patrol, Alerted, Chase }
    private EnemyState currentState = EnemyState.Patrol;

    [Header("Patrol points")]
    public List<Transform> patrolPoints;

    [Header("Player Detection")]
    public Transform player;
    public float viewDistance = 5f;
    public float viewAngle = 90f;
    public LayerMask obstacleLayer;

    [Header("Chase")]
    public float forgetTime = 5f; 
    private float timeSinceLastSeen;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoNewPoint();
    }

    private void Update()
    {
        bool canSeePlayer = CanSeePlayer();
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                if (canSeePlayer)
                {
                    currentState = EnemyState.Chase;
                    timeSinceLastSeen = 0f;
                }
                break;

            case EnemyState.Alerted:
                Alerted();
                if (canSeePlayer)
                {
                    currentState = EnemyState.Chase;
                    timeSinceLastSeen = 0f;
                }
                break;

            case EnemyState.Chase:
                Chase();
                if (canSeePlayer)
                {
                    timeSinceLastSeen = 0f;
                }
                else
                {
                    timeSinceLastSeen += Time.deltaTime;
                    if (timeSinceLastSeen >= forgetTime)
                    {
                        currentState = EnemyState.Patrol;
                        GoNewPoint();
                    }
                }
                break;
        }
        Debug.Log("ESTADO: "+currentState);
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoNewPoint();
        }
    }
    public void CheckAlert()
    {
        if (currentState != EnemyState.Chase)
        {
            currentState = EnemyState.Alerted;
            agent.destination = player.position;
        }
    } 

    void Alerted()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = EnemyState.Patrol;
            GoNewPoint();
        }
    }

    void GoNewPoint()
    {
        int indexPatrol = Random.Range(0, patrolPoints.Count);
        agent.destination = patrolPoints[indexPatrol].position;
    }

    void Chase()
    {
        if (player != null)
        {
            agent.destination = player.position;
        }
    }

    bool CanSeePlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= viewDistance)
        {
            float angle = Vector3.Angle(transform.forward, dirToPlayer);

            if (angle < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, obstacleLayer))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewDistance);
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
