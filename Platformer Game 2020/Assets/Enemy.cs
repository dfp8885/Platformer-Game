using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxHealth = 100;
    int currentHealth;

    void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;

        //set hurt animation

        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Debug.Log("Enemy died!");
        //Set dying animation

        //Disable enemy
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
