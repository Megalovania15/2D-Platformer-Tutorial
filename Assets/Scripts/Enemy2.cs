using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public Transform patrolPoint1;
    public Transform patrolPoint2;
    public float speed = 5f;

    private Rigidbody2D rb;

    private bool movingRight;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.right * speed;
        movingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
       //checking to see if the enemy has reached the first patrol point
        if (transform.position.x <= patrolPoint1.position.x)
        {
            //set the boolean to true as the enemy must move right
            movingRight = true;
        }
        //checking to see if the enemy has reached the second patrol point
        else if (transform.position.x >= patrolPoint2.position.x)
        {
            //set the boolean to false as the enemy must move left
            movingRight = false;   
        }

        //if the enemy is moving right
        if (movingRight)
        {
            //set it's velocity to the right
            rb.velocity = Vector2.right * speed;
        }
        //if the enemy is moving left
        else 
        {
            //set the velocity to the left
            rb.velocity = Vector2.left * -speed;
        }
    }
}
