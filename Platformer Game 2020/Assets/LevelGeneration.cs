using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;
    public GameObject[] rooms; // index 0 --> LR, Index 1 --> LRB, Index 2 --> LRT, Index 3 --> LRTB

    public GameObject player;
    private Transform playerTransform;
    public HealthBar playerHealth;

    public CinemachineVirtualCamera vcam;

    private int direction;
    public float moveAmount;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;

    public float minX;
    public float maxX;
    public float minY;
    public bool stopGeneration = false;

    public LayerMask room;

    private int downCounter = 0;
    private bool built = false;

    private void Start()
    {
        //generate random starting positon and create first room
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        int rand = Random.Range(0, rooms.Length-1);
        Instantiate(rooms[rand], transform.position, Quaternion.identity);

        //spawn player and link the health bar UI with the player health
        transform.position = new Vector2(transform.position.x, transform.position.y + 2);
        GameObject playerInstance = Instantiate(player, transform.position, Quaternion.identity);
        transform.position = new Vector2(transform.position.x, transform.position.y - 2);
        playerInstance.GetComponent<Health>().healthBar = playerHealth;

        playerTransform = playerInstance.transform;
        vcam.Follow = playerTransform;
        vcam.LookAt = playerTransform;

        // set game to pause so that player cannot move until map is built
        PauseMenu.GameIsPaused = true;

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
        
        //resume game once generation is complete and we have not already built
        if (stopGeneration == true && !built) { 
            PauseMenu.GameIsPaused = false;
            built = true;
        }
    }

    private void Move() {
        //Room detection
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);

        // Move right
        if (direction == 1 || direction == 2) { 
            if (transform.position.x < maxX){
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;

                // Generate room
                int rand = Random.Range(0, rooms.Length-1);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                //Generate new direction, exclude the possiblity of moving left
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
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;


                // Generate room
                int rand = Random.Range(0, rooms.Length-1);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                //Generate new direction, exclude possibility of moving right
                direction = Random.Range(3, 6);
            }
            else { direction = 5; }
        }

        // Move down
        else if (direction == 5) {

            downCounter++;

            if (transform.position.y > minY)
            {
                if (roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3) {
                    roomDetection.GetComponent<RoomType>().RoomDestruction();

                    int randBottomRoom = Random.Range(1, 4);
                    if (downCounter >= 2)
                    {
                        randBottomRoom = 3;
                    }
                    else {
                        if (randBottomRoom == 2)
                        {
                            randBottomRoom = 1;
                        }
                    }
                    
                    Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
                }
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;

                // Generate room
                int rand = Random.Range(2, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                // Generate new direction
                direction = Random.Range(1, 6);
            }
            else {
                //Remake last room into the final room
                roomDetection.GetComponent<RoomType>().RoomDestruction();
                Instantiate(rooms[rooms.Length-1], transform.position, Quaternion.identity);
                stopGeneration = true; }
        }
    }
}
