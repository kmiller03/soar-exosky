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
    public float r_mag;
    public float g_mag;
    public float b_mag;
}

public enum StarColor{
    white,
    red,
    blue,
    yellow,
    orange
}

public enum StarType{
    O,
    B,
    A,
    F,
    G,
    K,
    M
}

public class StarGenerator : MonoBehaviour
{

    public GameObject player;
    public Vector3 lastPlayerPosition;
    public List<Star> stars;

    public bool randomizeStars = false;
    public bool starsInitialized = false;
    
    public GameObject starWhiteObj;
    public GameObject starYellowObj;
    public GameObject starRedObj;
    public GameObject starBlueObj;
    public GameObject starOrangeObj;

    GameObject GetStarObject(StarType starType)
    {
        switch (starType)
        {
            case StarType.O:
            case StarType.B:
                return starBlueObj;  // Blue star for O and B types

            case StarType.A:
            case StarType.F:
            case StarType.G:
                return starWhiteObj;  // White star for A and F types

            case StarType.K:
                return starYellowObj;  // Yellow star for G and K types

            case StarType.M:
                return starRedObj;  // Red star for M type

            default:
                return starWhiteObj;  // Return null if starType is unknown
        }
    }

    GameObject GetStarObject(float r_mag, float g_mag, float b_mag)
    {
        //Get max from r_mag, g_mag, b_mag
        float max = Math.Max(r_mag, Math.Max(g_mag, b_mag));
        if(max == r_mag)
        {
            return starRedObj;
        }
        else if(max == g_mag)
        {
            return starYellowObj;
        }
        else
        {
            return starBlueObj;
        }
    }
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
            float[] max_rgb = {0,0,0};
            for (int i = 0; i < 1000; i++)
            {
                Star star = new Star();
                star.declination = UnityEngine.Random.Range(-90, 90);
                star.ascension = UnityEngine.Random.Range(0, 360);
                star.scaleFactor = scaleFactor;
                star.distance = UnityEngine.Random.Range(100, 1000);
                star.r_mag = UnityEngine.Random.Range(15,23);
                star.b_mag = UnityEngine.Random.Range(15,23);
                star.g_mag = UnityEngine.Random.Range(15,23);
                if(star.r_mag> max_rgb[0]) max_rgb[0] = star.r_mag;
                if(star.g_mag> max_rgb[1]) max_rgb[1] = star.g_mag;
                if(star.b_mag> max_rgb[2]) max_rgb[2] = star.b_mag;
                stars.Add(star);
            }
            foreach (Star star in stars)
            {
                Vector3 position = starPostion(star.declination, star.ascension, star.scaleFactor);
                GameObject baseObj = GetStarObject(ClassifyStar(star.r_mag, star.g_mag, star.b_mag));
                // GameObject baseObj = GetStarObject(star.r_mag, star.g_mag, star.b_mag);

                //Make position relative to player
                position += player.transform.position;
                GameObject starObj = Instantiate(baseObj, position, Quaternion.identity);
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

    public void PassStars(List<Star> stars, float[] max_rgb)
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
            // string colorname = color == StarColor.blue ? "blue": color == StarColor.red ? "red": color == StarColor.yellow?"yellow": "white";
            // Debug.Log("Star with: (" + star.r_mag + ","+star.g_mag+","+star.b_mag+") > " +colorname);
            GameObject baseObj = GetStarObject(ClassifyStar(star.r_mag, star.g_mag, star.b_mag));
            // GameObject baseObj = GetStarObject(star.r_mag, star.g_mag, star.b_mag);

            //Make position relative to player
            position += player.transform.position;
            GameObject starObj = Instantiate(baseObj, position, Quaternion.identity);
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
     // Function to classify a color based on raw RGB values
    public StarColor GetStarColor(float phot_rp_mean_mag, float phot_g_mean_mag, float phot_bp_mean_mag)
    {
        return StarColor.white;
    }

    public static StarType ClassifyStar(float phot_rp_mean_mag, float phot_g_mean_mag, float phot_bp_mean_mag)
    {
        // Calculate the color index: bp_rp
        float bp_rp = phot_bp_mean_mag - phot_rp_mean_mag;

        // Calculate effective temperature in Kelvin using the formula
        float temperature = 5600 * (1 / (0.92f * (bp_rp + 1)));

        // Classify star based on its temperature
        if (temperature >= 30000)
        {
            return StarType.O;  // O-Type: 30,000 K and above
        }
        else if (temperature >= 10000 && temperature < 30000)
        {
            return StarType.B;  // B-Type: 10,000 K to 30,000 K
        }
        else if (temperature >= 7500 && temperature < 10000)
        {
            return StarType.A;  // A-Type: 7,500 K to 10,000 K
        }
        else if (temperature >= 6000 && temperature < 7500)
        {
            return StarType.F;  // F-Type: 6,000 K to 7,500 K
        }
        else if (temperature >= 5200 && temperature < 6000)
        {
            return StarType.G;  // G-Type: 5,200 K to 6,000 K
        }
        else if (temperature >= 2000 && temperature < 5200)
        {
            return StarType.K;  // K-Type: 3,700 K to 5,200 K
        }
        else
        {
            return StarType.M;  // M-Type: Below 3,700 K
        }
    }

}
