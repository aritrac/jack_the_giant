using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] clouds;

    private float distanceBetweenClouds = 3f;

    private float minX, maxX; //cloud spawning x axis range of minimum and maximum

    private float lastCloudPositionY;

    private float controlX;

    [SerializeField]
    private GameObject[] collectibles;

    private GameObject player;

    void Awake()
    {
        controlX = 0f;
        SetMinAndMaxX();
        CreateClouds();
        //Doing this to have a reference to the player for positioning it
        player = GameObject.Find("Player");
    }

    void Start()
    {
        //We will position the player on the first cloud
        PositionThePlayer();
    }

    void SetMinAndMaxX()
    {
        Vector3 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,0));

        maxX = bounds.x - 0.5f;
        minX = -bounds.x + 0.5f;
    }

    void Shuffle(GameObject[] arrayToShuffle)
    {
        for(int i = 0; i < arrayToShuffle.Length; i++)
        {
            GameObject temp = arrayToShuffle[i];
            int random = Random.Range(i, arrayToShuffle.Length);
            arrayToShuffle[i] = arrayToShuffle[random];
            arrayToShuffle[random] = temp;
        }
    }

    void CreateClouds()
    {
        Shuffle(clouds);

        float positionY = 0f; //Starting to put clouds from y = 0 and then proceeding downwards

        for(int i = 0; i < clouds.Length; i++)
        {
            Vector3 temp = clouds[i].transform.position;

            temp.y = positionY;

            //toggling between left side cloud spawn and right side cloud spawn using the code below to create a zig zag pattern for cloud formation
            if(controlX == 0)
            {
                temp.x = Random.Range(0.0f, maxX);
                controlX = 1;
            }else if(controlX == 1)
            {
                temp.x = Random.Range(0.0f, minX);
                controlX = 2;
            }else if(controlX == 2)
            {
                temp.x = Random.Range(1.0f, maxX);
                controlX = 3;
            }else if(controlX == 3)
            {
                temp.x = Random.Range(-1.0f, minX);
                controlX = 0;
            }

            lastCloudPositionY = positionY;

            clouds[i].transform.position = temp;

            positionY -= distanceBetweenClouds;
        }
    }

    void PositionThePlayer()
    {
        GameObject[] darkClouds = GameObject.FindGameObjectsWithTag("Deadly");
        GameObject[] cloudsInGame = GameObject.FindGameObjectsWithTag("Cloud");

        //Code to make sure we are not stepping initially on a dark cloud but on a white one. Checking all the dark clouds if they will be spawned @ y = 0
        for(int i = 0; i < darkClouds.Length; i++)      
        {
            if(darkClouds[i].transform.position.y == 0f)
            {
                Vector3 t = darkClouds[i].transform.position;

                //Swapping dark cloud with a white cloud if the initial cloud is a dark cloud
                darkClouds[i].transform.position = new Vector3(cloudsInGame[0].transform.position.x, cloudsInGame[0].transform.position.y, cloudsInGame[0].transform.position.z);
                cloudsInGame[0].transform.position = t;
            }
        }

        //Now after the above fix we are going to position the player
        Vector3 temp = cloudsInGame[0].transform.position;

        //In this code we are trying to find the cloud which is nearest or equal to y = 0
        for(int i = 1; i < cloudsInGame.Length; i++)
        {
            if(temp.y < cloudsInGame[i].transform.position.y)
            {
                temp = cloudsInGame[i].transform.position;
            }
        }

        //Adding this so as to position the player above the cloud and so the player should not fall off the cloud right at the start
        temp.y += 0.8f;
        player.transform.position = temp;

    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if(target.tag == "Cloud" || target.tag == "Deadly")
        {
            if(target.transform.position.y == lastCloudPositionY)
            {
                Shuffle(clouds);
                Shuffle(collectibles);

                Vector3 temp = target.transform.position;

                for(int i = 0; i < clouds.Length; i++)
                {
                    //randomize the position of only those clouds which are not currently visible in the game
                    if (!clouds[i].activeInHierarchy)
                    {
                        //toggling between left side cloud spawn and right side cloud spawn using the code below to create a zig zag pattern for cloud formation
                        if (controlX == 0)
                        {
                            temp.x = Random.Range(0.0f, maxX);
                            controlX = 1;
                        }
                        else if (controlX == 1)
                        {
                            temp.x = Random.Range(0.0f, minX);
                            controlX = 2;
                        }
                        else if (controlX == 2)
                        {
                            temp.x = Random.Range(1.0f, maxX);
                            controlX = 3;
                        }
                        else if (controlX == 3)
                        {
                            temp.x = Random.Range(-1.0f, minX);
                            controlX = 0;
                        }

                        //maintaining the distance of the new cloud which will be spawned from the previous active cloud
                        temp.y -= distanceBetweenClouds;

                        //saving the new last cloud position as this is the new last cloud
                        lastCloudPositionY = temp.y;

                        //assign the position to the ith cloud
                        clouds[i].transform.position = temp;

                        //setting the cloud to be now visible in the hierarchy
                        clouds[i].SetActive(true);


                    }
                }  
            }
        }
    }
}
