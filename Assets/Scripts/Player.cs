using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float force;
    public float maxSpeed;
    public float jumpSpeed;
    public float raycastLength = 1f;
    public float fireRate = 1f;
    public int maxAmmo = 10;
    public int currentAmmo;
    public Transform currentRespawn;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public GameObject deathParticle;
    public AudioClip shootSound;
    public AudioClip jumpSound;

    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource source;
    //private UIHandler ui;
    private bool isGrounded = false;
    private float currentBulletTime;
    

    // Start is called before the first frame update
    void Start()
    {
        //we can find the rigidbody component by using the GetComponent method. This will
        //search the gameobject that the script is attached to for the specific component
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        currentBulletTime = fireRate;
        currentAmmo = maxAmmo;
        Debug.Log(currentAmmo + " X");
    }

    // Update is called once per frame
    void Update()
    {
        //IsGrounded();
        Jump();
        Shoot();

    }

    //in order for the Move function to work, it needs to be called somewhere. This can either
    //be in the Update function, which runs every frame, or in the FixedUpdate function
    //The FixedUpdate function runs in step with the physics engine. So if you want to apply
    //physics to a rigidbody, it's best to do it here
    void FixedUpdate()
    {
        Move();
    }

    /*void IsGrounded()
    {
        //a raycast returns a boolean, where it is either true or false depending on whether it hits 
        //we can then draw a raycast of a specified length from the player's origin, in a direction
        //between several layers
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, 1 << 8);
        //you can then draw a line that will show in the scene with an origin, a length and a colour
        Debug.DrawLine(transform.position, new Vector2(transform.position.x, -raycastLength), Color.green);

        //dependent on whether the raycast hits something, switch the boolean isGrounded from true to
        //false
        if (hitInfo.collider != null)
        {
            isGrounded = true;
        }
        else
            isGrounded = false;
    }*/

    void Move()
    {
        //to get an input from the keyboard, you can use Input.GetKey(KeyCode.Letter on keyboard)
        //to make our object move using a rigidbody, we need to apply a force to it
        //we can also use transform.position += Vector to make our object move, but this means that
        //physics will not affect the movement of our object

        if (Input.GetKey(KeyCode.A)) //left
        {
            
            rb.AddForce(new Vector2(-force, 0), ForceMode2D.Impulse);
            transform.eulerAngles = new Vector3(0, -180, 0);
            anim.SetBool("isRunning", true);
        }

        //to move in the opposite direction, we need to apply an opposite force. The player also
        //cannot move both left and right at the same time, so we can use an else if here

        else if (Input.GetKey(KeyCode.D)) //right
        {
            rb.AddForce(new Vector2(force, 0), ForceMode2D.Impulse);
            transform.eulerAngles = new Vector3(0, 0, 0);
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        //checking to see if the player is moving too fast horizontally. If they are, then we
        //"clamp" their velocity to a maximum speed
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
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

    void Jump()
    {
        //check for if the player has pressed space and if the boolean isGrounded is true
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            //add an upwards force of the jump speed
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            source.PlayOneShot(jumpSound);
            anim.SetBool("isJumping", true);
            isGrounded = false;
        }
        else {
            anim.SetBool("isJumping", false);
        }
    }

    void Shoot()
    {
        currentBulletTime -= Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            if (currentBulletTime <= 0 && currentAmmo != 0)
            {
                Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
                source.PlayOneShot(shootSound);
                currentAmmo -= 1;
                Debug.Log(currentAmmo + " X");
                currentBulletTime = fireRate;
            }

        }
        
    }

    void Death()
    {
        Instantiate(deathParticle, transform.position, transform.rotation);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine (RespawnTime(3f));
    }

    int GetCollectible(int maxValue, int currentValue, int addedValue)
    {
        //return Mathf.Min(currentValue + addedValue, maxValue);
        if (currentValue + addedValue < maxValue)            //checks the current value of an object against the maximum value, if the player does not have enough of the maximum value, then the amount can be added
        {
            currentValue += addedValue;
            Debug.Log(currentValue + " X");
            return currentValue;
        }
        return maxValue;
    }

    private IEnumerator RespawnTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.transform.position = currentRespawn.position;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Death Obstacle")
        {
            Death();
        }

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }

        if (other.gameObject.tag == "Ammo")
        {
            //can set the ammo value, ideally make this specific to each ammo type

            //check to see if the combined value exceeds the max amount we have
            /*if (currentAmmo + ammoValue < maxAmmo)
            {
                //add more ammo to the amount displayed
                currentAmmo += ammoValue;
            }
            else {
                currentAmmo = maxAmmo;
            }*/

            print("Ammo: " + currentAmmo);

            Destroy(other.gameObject);
        }
    }


}
