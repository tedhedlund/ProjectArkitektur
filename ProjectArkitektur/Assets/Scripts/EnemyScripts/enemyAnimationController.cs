using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAnimationController : MonoBehaviour
{

    [SerializeField] EnemyScript enemyScript;
    [SerializeField] Animator animator;

    bool isDead = false;

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
            animator.SetBool("isAttacking", false);
        }             
        else if(enemyScript.currentState == ZombieState.Chasing)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", true);
            animator.SetBool("isAttacking", false);
        }
        else if (enemyScript.currentState == ZombieState.Idle)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
        }
        else if(enemyScript.currentState == ZombieState.Hitting)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);
        }
        else if (enemyScript.currentState == ZombieState.Death)
        {
            //animator.SetBool("isRunning", false);
            //animator.SetBool("isWalking", false);
            //animator.SetBool("isAttacking", false);
            //animator.SetBool("isDying", true);
            if(!isDead)
            animator.SetTrigger("isDying");

            isDead = true;
        }

    }

    public void TakeDamage()
    {
        enemyScript.dmgPlayer = true;
    }

}
