using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    float walkTimer = 0f;

   public bool dmgPlayer = false;

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
            walkTimer = 0;
            currentState = ZombieState.Chasing;          
        }
        else
        {
            walkTimer += Time.deltaTime;
            if(currentState != ZombieState.Hitting)
            {
                if(walkTimer >= 2f)
                currentState = ZombieState.Walking;
            }          
        }

        switch (currentState)
        {
            case ZombieState.Idle:
                {
                    agent.speed = 0;
                    //Debug.Log("Idle");
                    dmgPlayer = false;
                    break;
                }

            case ZombieState.Walking:
                {
                    //Debug.Log("Walking");
                    agent.speed = originalSpeed;
                    agent.destination = player.transform.position;
                    dmgPlayer = false;
                    break;
                }

            case ZombieState.Chasing:
                {
                    //Debug.Log("Chasing");
                    agent.speed = chaseSpeed;
                    agent.destination = player.transform.position;
                    dmgPlayer = false;
                    break;
                }

            case ZombieState.Hitting:
                {
                    agent.speed = 1;
                    Vector3 target = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);
                    agent.transform.LookAt(target);
                    if(dmgPlayer)
                    {
                        Debug.Log("Player is hit");
                        dmgPlayer = false;
                    }
                    //Debug.Log("Hitting");
                    break;
                }
        }
    }

    private void OnTriggerEnter(Collider other)
    {       
        if(other.gameObject == player)
        {
            currentState = ZombieState.Hitting;           
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            currentState = ZombieState.Hitting;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            currentState = ZombieState.Walking;
        }
    }

}

public enum ZombieState
{
    Idle,
    Walking,
    Chasing,
    Hitting
}
