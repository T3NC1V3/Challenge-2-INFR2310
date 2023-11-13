using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavPatrol : MonoBehaviour
{
    public GameObject target;
    public GameObject target1;
    private NavMeshAgent agent;
    bool isWalking = true;
    private Animator animator;

    public float rotateSpeed;

    // FSM
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        animator.speed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.magnitude - target.transform.position.magnitude) < sightRange)
        {
            playerInSightRange = true;
        }
        else
        {
            playerInSightRange = false;
        }

        if (Mathf.Abs(transform.position.magnitude - target.transform.position.magnitude) < attackRange)
        {
            playerInAttackRange = true;
        }
        else
        {
            playerInAttackRange = false;
        }

        // Patrol State
        if (!playerInSightRange && !playerInAttackRange && isWalking) Patrol();
        // Chase State
        if (playerInSightRange && !playerInAttackRange && isWalking) Chase();
        // Attack State
        if (playerInSightRange && playerInAttackRange && !isWalking) Attack();

    }

    private void Patrol()
    {
        
    }

    private void Chase()
    {
        agent.destination = target.transform.position;
    }

    private void Attack()
    {
        agent.destination = transform.position;
        RotateTowardsTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Red")
        {
            isWalking = false;
            animator.SetTrigger("Attack");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Red")
        {
            isWalking = true;
            animator.SetTrigger("Walk");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void RotateTowardsTarget()
    {
        float stepSize = rotateSpeed * Time.deltaTime;

        Vector3 targetDir = target1.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
