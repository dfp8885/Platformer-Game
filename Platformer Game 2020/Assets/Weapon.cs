using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator animator;

    public Transform firePoint;
    public int damage = 40;
    public LineRenderer line;

    public Transform meleePoint;
    public float meleeRange = 0.5f;
    public LayerMask enemyLayers;

    // Update is called once per frame
    void Update()
    {
        //Get key press for ranged attack
        if (Input.GetButtonDown("Fire1")) {
            StartCoroutine(Shoot());
        }

        //Get key press for melee attack
        if (Input.GetButtonDown("Fire2")) {
            Melee();
        }
    }

    IEnumerator Shoot() {
        //Send raycast laser out
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
        
        //If ray hits anything send the Line from the fire point to the hit point
        if (hitInfo)
        {
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            //If an enemy was hit deal damage to the enemy
            if (enemy != null) {
                enemy.TakeDamage(damage);
            }
            //Send line from fire point to hit point
            line.SetPosition(0, firePoint.position);
            line.SetPosition(1, hitInfo.point);
        }
        else {
            //Send line out from fire point out 100 units
            line.SetPosition(0, firePoint.position);
            line.SetPosition(1, firePoint.position + firePoint.right * 100);
        }
        
        //Make Line flash to only appear for a single frame
        line.enabled = true;
        yield return new WaitForSeconds(0.02f);
        line.enabled = false;
    }

    void Melee() {
        //Play attack animation

        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleePoint.position, meleeRange, enemyLayers);

        //Damage enemies
        foreach (Collider2D enemy in hitEnemies) {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected() {
        if (meleePoint == null)
            return;
        Gizmos.DrawWireSphere(meleePoint.position, meleeRange);
    }
}
