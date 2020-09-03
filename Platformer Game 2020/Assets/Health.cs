using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Animator animator;

    public int maxHealth = 100;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

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
