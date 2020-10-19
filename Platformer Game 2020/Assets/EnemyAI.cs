using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public Transform enemyGFX;

    public float speed = 400f;
    public float nextWaypoint = 3f;

    public float bounce = 2000f;
    public float bounceCooldown = 0.25f;
    public bool bounceUp = true;
    public float nextBounce;

    public float rangeRadius = 60f;
    public LayerMask playerLayers;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath(){
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);

    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        Collider2D[] foundColliders = Physics2D.OverlapCircleAll(enemyGFX.position, rangeRadius, playerLayers);

        bool playerFound = false;

        foreach (Collider2D coll in foundColliders) {
            playerFound = true;
        }

        if (playerFound)
        {
            if (path == null)
            {
                return;
            }

            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypoint)
            {
                currentWaypoint++;
            }

            if (force.x >= 0.01f)
            {
                enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (force.x <= 0.01f)
            {
                enemyGFX.localScale = new Vector3(1f, 1f, 1f);
            }

            if (Time.time > nextBounce)
            {
                bounceUp = true;
            }
        }
        else {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

    }

    void OnCollisionEnter2D(Collision2D colInfo) {
        //make the enemy bounce in the opposite direction of the collision
        if (bounceUp) {
            Vector2 transformPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 direction = (transformPos - colInfo.contacts[0].point).normalized;
            Vector2 force = direction * bounce * Time.deltaTime;
            rb.AddForce(force);

            nextBounce = Time.time + bounceCooldown;
            bounceUp = false;

        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(enemyGFX.position, rangeRadius);
    }
}
