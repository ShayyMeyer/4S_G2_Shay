using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{

    private NavMeshAgent agent;
    [SerializeField] private Transform agentTarget;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(agentTarget.position);
    }
}


  
