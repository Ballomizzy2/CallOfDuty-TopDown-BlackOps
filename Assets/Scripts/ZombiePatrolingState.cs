using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class ZombiePatrolingState : StateMachineBehaviour
{

    float timer;
    public float patrolTime = 10f;

    Transform player;
    NavMeshAgent agent;

    public float detectionRange = 45f;
    public float patrolSpeed = 6f;

    List<Transform> waypointsList = new List<Transform>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = patrolSpeed;
        timer = 0f;

        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        foreach (Transform t in waypointCluster.transform)
        {
            waypointsList.Add(t);
        }

        Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
        agent.SetDestination(nextPosition);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
        }

        timer += Time.deltaTime;
        if (timer > patrolTime)
        {
            animator.SetBool("isPatroling", false);
        }

        float distanceToPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceToPlayer < detectionRange)
        {
            animator.SetBool("isChasing", true);
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       agent.SetDestination(agent.transform.position);
    }
}
