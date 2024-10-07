using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Star
{
    public float declination;
    public float ascension;
    public float distance;
    public float scaleFactor;
}

public class StarGenerator : MonoBehaviour
{

    public GameObject player;
    public Vector3 lastPlayerPosition;
    public List<Star> stars;

    public bool randomizeStars = false;
    public bool starsInitialized = false;
    
    public GameObject starBaseObj;

    public float scaleFactor = 10000;

    public List<GameObject> starObjects;

    // Start is called before the first frame update
    void Start()
    {
        //Generate stars
        if(randomizeStars)
        {
            //clear stars
            stars.Clear();
            for (int i = 0; i < 1000; i++)
            {
                Star star = new Star();
                star.declination = UnityEngine.Random.Range(-90, 90);
                star.ascension = UnityEngine.Random.Range(0, 360);
                star.scaleFactor = scaleFactor;
                star.distance = UnityEngine.Random.Range(100, 1000);
                stars.Add(star);
            }
            foreach (Star star in stars)
            {
                Vector3 position = starPostion(star.declination, star.ascension, star.scaleFactor);
                //Make position relative to player
                position += player.transform.position;
                GameObject starObj = Instantiate(starBaseObj, position, Quaternion.identity);
                //change scale of star
                float scale = starScale(star.distance);
                starObj.transform.localScale = new Vector3(scale, scale, scale);

                starObj.transform.parent = this.transform;
                //activate star
                starObj.SetActive(true);
                starObjects.Add(starObj);
            }
            starsInitialized = true;
        }
    }

    public void PassStars(List<Star> stars)
    {
        //destroy old stars
        foreach (GameObject star in starObjects)
        {
            Destroy(star);
        }
        starObjects.Clear();
        this.stars.Clear();
        this.stars = stars;
        foreach (Star star in stars)
        {
            Vector3 position = starPostion(star.declination, star.ascension, scaleFactor);
            //Make position relative to player
            position += player.transform.position;
            GameObject starObj = Instantiate(starBaseObj, position, Quaternion.identity);
            //change scale of star
            float scale = starScale(star.distance);
            starObj.transform.localScale = new Vector3(scale, scale, scale);
            starObj.transform.parent = this.transform;
            //activate star
            starObj.SetActive(true);
            starObjects.Add(starObj);
        }
        starsInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if player changes it's position move stars
        if (starsInitialized && lastPlayerPosition != player.transform.position)
        {
            foreach (GameObject star in starObjects)
            {
                star.transform.position += player.transform.position - lastPlayerPosition;
            }
            lastPlayerPosition = player.transform.position;
        }
        
    }

    public float starScale(float distance){
        if(distance == 0)
        {
            return 1;
        }
        return 10000 / distance;
    }

    public Vector3 starPostion(float starDeclination, float starAscension, float scaleFactor)
    {
        //convert star declination to radians
        starDeclination = starDeclination * Mathf.Deg2Rad;
        //convert star ascension to radians
        starAscension = starAscension * Mathf.Deg2Rad;

        //calcculate x,y,z position of star
        float yPosition = Mathf.Sin(starAscension);
        float zPosition = Mathf.Sin(starDeclination);
        float xPosition = Mathf.Cos(starAscension);

        float sphereCorrection = Mathf.Cos(starDeclination);
        xPosition *= sphereCorrection;
        yPosition *= sphereCorrection;

        return new Vector3(xPosition * scaleFactor, zPosition * scaleFactor,yPosition * scaleFactor);
    }
}
