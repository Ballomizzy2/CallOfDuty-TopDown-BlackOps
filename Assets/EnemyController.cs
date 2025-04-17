using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyState state;

    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        state = EnemyState.Chasing;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        switch (state)
        {
            case EnemyState.Chasing:
                if (distance > attackRange)
                {
                    agent.SetDestination(target.position);
                    animator.SetBool("IsMoving", true);
                }
                else
                {
                    state = EnemyState.Attacking;
                    agent.ResetPath();
                }
                break;

            case EnemyState.Attacking:
                animator.SetBool("IsMoving", false);
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    animator.SetTrigger("Attack");
                    lastAttackTime = Time.time;
                }

                if (distance > attackRange)
                    state = EnemyState.Chasing;
                break;
        }
    }
}
