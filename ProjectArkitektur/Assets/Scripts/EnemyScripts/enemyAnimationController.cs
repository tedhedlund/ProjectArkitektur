using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAnimationController : MonoBehaviour
{

    [SerializeField] EnemyScript enemyScript;
    [SerializeField] Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyScript.currentState == ZombieState.Walking)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }             
        else if(enemyScript.currentState == ZombieState.Chasing)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", true);
        }
        else if (enemyScript.currentState == ZombieState.Idle)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }

    }
}
