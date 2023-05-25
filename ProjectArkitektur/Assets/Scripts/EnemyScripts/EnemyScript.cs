using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    Player_Controller player;
    [SerializeField] GameObject playerGO;


    public ZombieState currentState;

    [SerializeField] float originalSpeed = 4f;
    [SerializeField] float chaseSpeed = 8f;
    //[SerializeField] private AudioManager audioManager;
    [SerializeField] private float timeBetweenSounds;
    [SerializeField] private AudioSource[] zombieSounds;
    [SerializeField] GameObject UI;
    UIModelScript UiModel;

    float walkTimer = 0f;

    float zombieHealth = 100f;
     bool isDead = false;
    float deathTimer = 0f;
    float soundTimer;

   public bool dmgPlayer = false;

    public float playerDist;
    public float newVolume;

    GameObject ammoBoxToSpawn;
    bool hasInstantiatedAmmoBox;
    bool hasIncrementedDeathCounter;
    private static int deathCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentState = ZombieState.Idle;
        agent.speed = originalSpeed;

        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player_Controller>();
        UI = GameObject.FindGameObjectWithTag("UI");
        UiModel = UI.GetComponent<UIModelScript>();

        ammoBoxToSpawn = GameObject.FindGameObjectWithTag("AmmoBox");
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
            Debug.Log($"Deathcounter: {deathCounter}");
            HandleAmmoBoxSpawn();
            
            currentState = ZombieState.Death;
            deathTimer += Time.deltaTime;

            if(deathTimer >= 20f)
            {
                Destroy(gameObject);
            }
        }
        else if (!isDead)
        {
            HandleZombieSound();

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
                        player.TakeDamage(10);
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
                        UiModel.curKills++;
                    }
                    isDead = true;
                    break;
                }
        }
    }

    private void HandleZombieSound()
    {
        soundTimer += Time.deltaTime;
        newVolume = Mathf.Clamp01(1f - (playerDist / 50));
        playerDist = Vector3.Distance(transform.position, player.transform.position);

        if (soundTimer >= timeBetweenSounds)
        {
            int rnd = Random.Range(0, zombieSounds.Length - 1);

            //playerDist = Vector3.Distance(transform.position, player.transform.position);
            //if (dist <= 10f)
            //{
            //    float newVolume = 1.0f / dist;
            //    audioManager.ZombieRandom[0].volume = newVolume;
            //    audioManager.ZombieRandom[0].Play();
            //    soundTimer = 0;
            //}
            //else
            //{
            //    audioManager.ZombieRandom[0].Stop();
            //    soundTimer = 0;
            //}
            // newVolume = Mathf.Clamp01(1f - (playerDist / 100f));
            zombieSounds[rnd].volume = newVolume;
            zombieSounds[rnd].Play();
            soundTimer = 0;

        }
    }

    public void TakeDamage(float damage)
    {
        zombieHealth -= damage;
    }

    private void HandleAmmoBoxSpawn()
    {
        if (!hasIncrementedDeathCounter)
        {
            deathCounter++;
            hasIncrementedDeathCounter = true;
        }

        int ammoSpawnInterval = 2;
        if (!hasInstantiatedAmmoBox && deathCounter == ammoSpawnInterval)
        {
            float yOffset = 1.5f;
            Vector3 deathPosition = transform.position;
            deathPosition.y -= yOffset;
            GameObject ammoBox = Instantiate(ammoBoxToSpawn, deathPosition, Quaternion.identity);
            hasInstantiatedAmmoBox = true;
            deathCounter = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {       
        if(other.gameObject.tag == "Player")
        {
            if(currentState != ZombieState.Death)
            currentState = ZombieState.Hitting;           
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(currentState != ZombieState.Death)
            currentState = ZombieState.Hitting;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
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
