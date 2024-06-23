using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveForce;
    public float maxSpeed = 15f;
    public Transform groundDetect;
    public float raycastLength = 1f;

    private Rigidbody2D rb;
    private bool movingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        //checking if the ground is there
        RaycastHit2D hitInfo = Physics2D.Raycast(groundDetect.position, Vector2.down, raycastLength, 1 << 8);
        Debug.DrawLine(groundDetect.position, new Vector2(groundDetect.position.x, -raycastLength), Color.green);

        //checking if there are walls
        /*RaycastHit2D forwardInfo = Physics2D.Raycast(transform.position, transform.right, raycastLength, 1 << 8);
        Debug.DrawLine(transform.position, transform.position + transform.right * raycastLength, Color.white);*/

        if (hitInfo.collider == false && movingRight) //if the collider doesn't hit something then we flip the sprite so it faces left
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            movingRight = false;
        }
        else if (hitInfo.collider == false && !movingRight) //if the collider doesn't hit something then we flip the sprite again so it faces right
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            movingRight = true;
        }

        if (movingRight) //enemy moves right
        {
            rb.AddForce(Vector2.right * moveForce);
        }
        else //enemy moves left
        {
            rb.AddForce(Vector2.right * -moveForce);
        }

        if (Mathf.Abs(rb.velocity.x) > maxSpeed)    //checking to see if the enemy is moving too fast horizontally
        {
            if (rb.velocity.x < 0) //left movement
            {
                rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
            }
            else if (rb.velocity.x > 0) //right movement
            {
                rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            }
        }

    }

}
