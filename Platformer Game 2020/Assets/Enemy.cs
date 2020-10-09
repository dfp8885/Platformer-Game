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
    public float attackCooldown = 0.5f;
    public bool attackUp = true;
    public float nextAttack;

    public HealthBar healthBar;
    public bool isDead = false;

    void Start() {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
        nextAttack = Time.time;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;

        //update health bar
        //allows for possibility of not having a health bar
        if (healthBar != null) {
            healthBar.SetHealth(currentHealth);
        }

        //set hurt animation


        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        //Play dying animation
        isDead = true;

        //Disable enemy
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    void FixedUpdate() {
        if (Time.time > nextAttack){
            attackUp = true;
        }
    }

    void OnCollisionEnter2D(Collision2D colInfo) {
        Health player = colInfo.collider.GetComponent<Health>();
        if (player != null && attackUp)
        {
            player.TakeDamage(damage);
            nextAttack = Time.time + attackCooldown;
            attackUp = false;
        }
    }

    /**
    void OnDrawGizmosSelected()
    {
        if (meleePoint == null)
            return;
        Gizmos.DrawWireSphere(meleePoint.position, meleeRange);
    }
    **/
}

