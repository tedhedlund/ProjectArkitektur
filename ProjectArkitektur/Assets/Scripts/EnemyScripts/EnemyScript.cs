using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject player;

    public ZombieState currentState;

    [SerializeField] float originalSpeed = 4f;
    [SerializeField] float chaseSpeed = 8f;

    // Start is called before the first frame update
    void Start()
    {
        currentState = ZombieState.Idle;
        agent.speed = originalSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(agent.transform.position, player.transform.position) >= 10)
        {
            currentState = ZombieState.Chasing;          
        }
        else
        {
            currentState = ZombieState.Walking;
        }

        switch (currentState)
        {
            case ZombieState.Idle:
                {
                    agent.speed = 0;
                    Debug.Log("Idle");
                    break;
                }

            case ZombieState.Walking:
                {
                    Debug.Log("Walking");
                    agent.speed = originalSpeed;
                    agent.destination = player.transform.position;
                    break;
                }

            case ZombieState.Chasing:
                {
                    Debug.Log("Chasing");
                    agent.speed = chaseSpeed;
                    agent.destination = player.transform.position;
                    break;
                }
        }
    }
}

public enum ZombieState
{
    Idle,
    Walking,
    Chasing
}
