using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Animator animator;

    public int maxHealth = 100;
    int currentHealth;

    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //update health bar
        healthBar.SetHealth(currentHealth);

        //set hurt animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Reset the scene
        Application.LoadLevel(Application.loadedLevel);
        
    }
}
