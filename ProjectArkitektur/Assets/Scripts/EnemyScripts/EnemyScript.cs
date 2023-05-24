using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;
using UnityEngine.InputSystem.XR.Haptics;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject player;
    [SerializeField] GameObject canvas;

    public ZombieState currentState;

    [SerializeField] float originalSpeed = 4f;
    [SerializeField] float chaseSpeed = 8f;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private float timeBetweenSounds;

    float walkTimer = 0f;

    float zombieHealth = 100f;
     bool isDead = false;
    float deathTimer = 0f;
    float soundTimer;

   public bool dmgPlayer = false;

    // Start is called before the first frame update
    void Start()
    {        
        currentState = ZombieState.Idle;
        agent.speed = originalSpeed;
    }

    private void Awake()
    {
        //audioManager.zombieSpawn.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug Code
        if(Input.GetKeyDown(KeyCode.F))
        {
            zombieHealth = 0f;
        }

        if(zombieHealth <= 0f)
        {                      
            currentState = ZombieState.Death;
            deathTimer += Time.deltaTime;

            if(deathTimer >= 20f)
            {
                Destroy(gameObject);
            }
        }
        else if (!isDead)
        {
            soundTimer += Time.deltaTime;
            
            if (soundTimer >= timeBetweenSounds)
            {
                int rnd = Random.Range(0, audioManager.ZombieRandom.Length - 1);

                 audioManager.ZombieRandom[rnd].Play();
                 soundTimer = 0;
                
            }

            if (Vector3.Distance(agent.transform.position, player.transform.position) >= 10)
            {
                walkTimer = 0;
                currentState = ZombieState.Chasing;
            }
            else
            {
                walkTimer += Time.deltaTime;
                if (currentState != ZombieState.Hitting)
                {
                    if (walkTimer >= 2f)
                        currentState = ZombieState.Walking;
                }
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
                    //int rnd = Random.Range(0, audioManager.ZombieRandom.Length - 1);
                    //if (!audioManager.ZombieRandom[rnd].isPlaying)
                    //{
                    //    audioManager.ZombieRandom[rnd].Play();
                    //}
                   
                    break;
                }

            case ZombieState.Chasing:
                {
                    //Debug.Log("Chasing");
                    agent.speed = chaseSpeed;
                    agent.destination = player.transform.position;
                    dmgPlayer = false;
                    //if (!audioManager.zombieChase.isPlaying)
                    //{
                    //    audioManager.zombieChase.Play();
                    //}
                    
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
                        player.GetComponent<Player_Controller>().TakeDamage(10);
                        dmgPlayer = false;

                        //if (!audioManager.zombieAttack.isPlaying)
                        //{
                        //    audioManager.zombieAttack.Play();
                        //}
                    }
                    //Debug.Log("Hitting");
                    break;
                }

            case ZombieState.Death:
                {
                    if(!isDead)
                    {                        
                        Debug.Log("Zombie dead");

                        agent.speed = 0;
                        dmgPlayer = false;
                        gameObject.GetComponent<Collider>().enabled = false;
                        canvas.GetComponent<UIModelScript>().curKills++;
                    }                   
                    isDead = true;
                    break;
                }
        }
    }

    private void OnTriggerEnter(Collider other)
    {       
        if(other.gameObject == player)
        {
            if(currentState != ZombieState.Death)
            currentState = ZombieState.Hitting;           
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            if(currentState != ZombieState.Death)
            currentState = ZombieState.Hitting;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            if(currentState != ZombieState.Death)
            currentState = ZombieState.Walking;
        }
    }

}

public enum ZombieState
{
    Idle,
    Walking,
    Chasing,
    Hitting,
    Death
}
