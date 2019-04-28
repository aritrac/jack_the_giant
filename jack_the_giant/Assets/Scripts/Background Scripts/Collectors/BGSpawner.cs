using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSpawner : MonoBehaviour
{
    private GameObject[] backgrounds;

    private float lastY;

    void Start()
    {
        GetBackgroundAndSetLastY();
    }

    void GetBackgroundAndSetLastY()
    {
        //Getting all the background game objects with the tag Background
        backgrounds = GameObject.FindGameObjectsWithTag("Background");

        //Taking a random Y position to find the lastY position
        lastY = backgrounds[0].transform.position.y;

        //Here we are going to compare the above y position value with all other Y position of background game objects and find the last Y position
        for (int i = 1; i < backgrounds.Length; i++)
        {
            //We are comparing the lastY position, if it is higher than the next background Y position, then this next background Y position is the latest lastY position
            if (lastY > backgrounds[i].transform.position.y)
                lastY = backgrounds[i].transform.position.y;
        }
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if(target.tag == "Background")
        {
            //if only we are touching the last background game object, while scrolling down
            if(target.transform.position.y == lastY)
            {
                Vector3 temp = target.transform.position;
                //getting the height of the background
                float height = ((BoxCollider2D)target).size.y;

                for(int i = 0; i < backgrounds.Length; i++)
                {
                    if (!backgrounds[i].activeInHierarchy)
                    {
                        temp.y -= height;

                        //Assigining the new temp.y as the lastY for the lowest background
                        lastY = temp.y;

                        //Making the backgrounds[i] spawn at temp position
                        backgrounds[i].transform.position = temp;
                        //Making the newly spawned background game object visible
                        backgrounds[i].SetActive(true);
                    }
                }
            }
        }
    }
}
