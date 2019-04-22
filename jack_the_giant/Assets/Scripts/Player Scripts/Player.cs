using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 8f;
    public float maxVelocity = 4f;

    private Rigidbody2D myBody;
    private Animator anim;

    //this will be called before the Start() method
    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();   //Getting references to the RigidBody2D component within the Player game object
        anim = GetComponent<Animator>();        //Getting references to the Animator component within the Player game object
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //this method will be called every couple of frames, and is suitable for Physics methods
    void FixedUpdate()
    {
        PlayerMoveKeyboard();
    }

    void PlayerMoveKeyboard()
    {
        float forceX = 0f;
        float vel = Mathf.Abs(myBody.velocity.x);

        float h = Input.GetAxisRaw("Horizontal"); //Going to return -1 on left key, 1 on right key and 0 on no key pressed {-1,0,1}

        if(h > 0) //If the player is moving in the right direction h == 1
        {
            if (vel < maxVelocity)
                forceX = speed;
            Vector3 temp = transform.localScale; //Getting the X and Y Scale values from the transform component of the Player game object
            temp.x = 1.3f;
            transform.localScale = temp;

            anim.SetBool("Walk", true); //transition from idle to walk animation
        }else if(h < 0) //If player is moving in the left direction h == -1
        {
            if (vel < maxVelocity)
                forceX = -speed;
            Vector3 temp = transform.localScale;
            temp.x = -1.3f;
            transform.localScale = temp;

            anim.SetBool("Walk", true); //transition from idle to walk animation
        }else
        {
            anim.SetBool("Walk", false);//transition from walk to idle animation
        }

        myBody.AddForce(new Vector2(forceX,0));
    }
}
