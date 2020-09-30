using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;
    public GameObject[] rooms;

    private int direction;
    public float moveAmount;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;

    public float minX;
    public float maxX;
    public float minY;
    private bool stopGeneration = false;

    private void Start()
    {
        //generate random starting positon and create first room
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        Instantiate(rooms[3], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);
    }

    private void Update() {
        if (timeBtwRoom <= 0 && stopGeneration == false) {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        }
        else {
            timeBtwRoom -= Time.deltaTime;
        }
    }

    private void Move() {
        // Move right
        if (direction == 1 || direction == 2) { 
            if (transform.position.x < maxX){
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;

                direction = Random.Range(1, 6);
                if (direction == 3) { direction = 2; }
                else if (direction == 4) { direction = 5; }
            }
            else { direction = 5; }
        }

        // Move left
        else if (direction == 3 || direction == 4) { 
            if (transform.position.x > minX)
            {
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;

                direction = Random.Range(3, 6);
            }
            else { direction = 5; }
        }

        // Move down
        else if (direction == 5) { 
            if (transform.position.y > minY)
            {
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;

                direction = Random.Range(3, 6);
            }
            else { stopGeneration = true; }
        }

        Instantiate(rooms[3], transform.position, Quaternion.identity);
    }
}
