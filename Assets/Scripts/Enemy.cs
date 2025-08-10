using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 150;
    private Animator animator;

    private NavMeshAgent agent; 
    public bool isDead = false;

    public static EventHandler OnZombieSlotFreed;

    [Header("Debug")]
    [SerializeField] private bool staticDummy = false;

    private void Start()
    {
        if(!staticDummy)
        {
            animator = GetComponent<Animator>();
        }

    }

    public void TakeDamage(int damage, DamageType dmgType)
    {
        if (PowerUpManager.Instance.instaKillActive)
        {
            damage = 2147483647;
        }

        HP -= damage;

        if (!staticDummy)
        {
            //this block is for moving zombies
       
            if (HP <= 0)
            {
                if (staticDummy)
                {
                    agent.isStopped = true; 
                }

                SoundMng.Instance.zombieChannel.PlayOneShot(SoundMng.Instance.zombieDeath);
                PowerUpManager.Instance.TryDropPowerUp(transform.position);
                PlayerVoicelineManager.Instance.PlayVoiceline(PlayerVoicelineManager.Instance.zombieKillClips);
                int randomValue = UnityEngine.Random.Range(0, 2);
                if (randomValue == 0)
                {
                    animator.SetTrigger("DIE2");
                    PointsForDeath(dmgType);
                    Destroy(gameObject, 3f);
                }
                else
                {
                    animator.SetTrigger("DIE1");
                    PointsForDeath(dmgType);
                    Destroy(gameObject, 3f);
                }
                OnZombieSlotFreed?.Invoke(this, EventArgs.Empty);
                isDead = true;
                GetComponent<CapsuleCollider>().enabled = false;


            }
            else
            {
                animator.SetTrigger("DAMAGE");

                SoundMng.Instance.zombieChannel.PlayOneShot(SoundMng.Instance.zombieHurt);
            }
        }
        else
        {
            //this block is for idle testing zombies
            if (HP <= 0)
            {
          

                SoundMng.Instance.zombieChannel.PlayOneShot(SoundMng.Instance.zombieDeath);
                PowerUpManager.Instance.TryDropPowerUp(transform.position);
                PlayerVoicelineManager.Instance.PlayVoiceline(PlayerVoicelineManager.Instance.zombieKillClips);

                PointsForDeath(dmgType);
                isDead = true;
                
                Destroy(gameObject);



            }
            else
            {
               

                SoundMng.Instance.zombieChannel.PlayOneShot(SoundMng.Instance.zombieHurt);
            }
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieInteracts"))
        {
            animator.SetBool("isAttacking", true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ZombieInteracts"))
        {
            animator.SetBool("isAttacking", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ZombieInteracts"))
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isChasing", true);
        }
    }
    private void OnDragGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 21f);
    }

    // public float speed = 2f; // Enemy's movement speed
    // public float attackRange = 1.5f; // Range within which the Enemy can attack
    // public int damage = 10; // Damage dealt by the Enemy
    // public float attackCooldown = 1f; // Time between attacks

    // private Transform player; // Reference to the player's transform
    // private float lastAttackTime; // Time when the Enemy last attacked

    // void Start()
    // {
    //     player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
    // }

    // void Update()
    // {
    //     MoveTowardsPlayer(); // Move towards the player
    //     AttackPlayer(); // Check if the Enemy can attack the player
    // }

    // void MoveTowardsPlayer()
    // {
    //     if (player != null)
    //     {
    //         Vector3 direction = (player.position - transform.position).normalized; // Calculate direction to player
    //         transform.position += direction * speed * Time.deltaTime; // Move towards player
    //     }
    // }

    // void AttackPlayer()
    // {
    //     if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange)
    //     {
    //         if (Time.time >= lastAttackTime + attackCooldown)
    //         {
    //             // Deal damage to the player (assuming the player has a method to take damage)
    //             PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
    //             if (playerHealth != null)
    //             {
    //                 playerHealth.TakeDamage(damage); // Call TakeDamage method on player's health script
    //             }
    //             lastAttackTime = Time.time; // Update last attack time
    //         }
    //     }
    // }
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         // Handle collision with player (e.g., stop moving, play attack animation, etc.)
    //     }
    // }

    private void PointsForDeath(DamageType dmgType)
    {
        //this switch gives points
        //Nuke is isolated bc we don't want 400 points PER zombie on map. lol
        if (dmgType != DamageType.Nuke)
        {
                 //Debug.Log("ded");
            GameManager_Scores.Instance.PointsPerKill(dmgType);
        }
    }
}
