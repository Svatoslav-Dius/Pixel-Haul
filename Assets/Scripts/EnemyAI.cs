using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Налаштування Зору")]
    public float viewRadius = 5f;
    private Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            if (CanSeePlayer())
            {
                agent.SetDestination(player.position);
            }
            else
            {
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    agent.ResetPath();
                }
            }

            if (agent.velocity.magnitude > 0.1f)
            {
                Vector2 direction = agent.velocity.normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
            }
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer > viewRadius) return false;

        return true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}