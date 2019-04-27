using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float speed = 1f;
    private float acceleration = 0.2f;
    private float maxSpeed = 3.2f;

    [HideInInspector]
    public bool moveCamera;
    // Start is called before the first frame update
    void Start()
    {
        moveCamera = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCamera)
        {
            MoveCamera();
        }
    }

    void MoveCamera()
    {
        //Getting the current camera position
        Vector3 temp = transform.position;

        float oldY = temp.y; //Storing the previous camera position in oldY
        float newY = temp.y - (speed * Time.deltaTime);

        //assigning the camera y position a value between oldY and newY
        temp.y = Mathf.Clamp(temp.y, oldY, newY);

        //assigning the camera its new position
        transform.position = temp;

        //To increase the speed over time, we are doing the following
        speed += acceleration * Time.deltaTime; //Time.deltaTime is the time between 2 frames at which the scene is being rendered

        //To check that the speed does not overtake the maximum speed, we do the following. So initially there will be acceleration then the speed will be constant once the max speed is reached
        if (speed > maxSpeed)
            speed = maxSpeed;

    }
}
