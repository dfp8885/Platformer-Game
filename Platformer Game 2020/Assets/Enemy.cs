using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    public int maxHealth = 100;
    int currentHealth;

    public Transform meleePoint;
    public float meleeRange = 0.5f;
    public LayerMask playerLayers;

    public int damage = 25;
    public int meleeRate = 1;
    float nextFire;

    void Start() {
        currentHealth = maxHealth;
        nextFire = Time.time;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;

        //set hurt animation

        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        //Play dying animation

        Destroy(gameObject);

        //Disable enemy
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    void FixedUpdate() {
        if (Time.time > nextFire)
        {
            //Detect enemies in range of attack
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(meleePoint.position, meleeRange, playerLayers);

            //Damage enemies
            foreach (Collider2D player in hitPlayers)
            {
                player.GetComponent<Health>().TakeDamage(damage);
            }
            nextFire = Time.time + meleeRate;
            animator.SetBool("Attack", true);
        }
        else { 
            animator.SetBool("Attack", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (meleePoint == null)
            return;
        Gizmos.DrawWireSphere(meleePoint.position, meleeRange);
    }
}

