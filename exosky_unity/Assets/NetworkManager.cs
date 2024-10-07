using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class OnStarsReceived : UnityEvent<List<Star>> { }

public class NetworkManager : MonoBehaviour
{
    string serverUrl = "http://localhost:5000";
    string cloudUrl = "https://soar-exosky.up.railway.app";
    string stars_url = "/stars_rel_to/";
    public string starID = "1234";
    public ui_controller uic;
    public bool local = true;

    public OnStarsReceived onStarsReceived;

    public void Start()
    {
        if (!local)
        {
            serverUrl = cloudUrl;
        }
    }

    // Wrapper class for JSON deserialization
    [System.Serializable]
    private class StarsWrapper
    {
        public List<StarJson> stars;
    }

    // Struct for JSON parsing (matches the response fields)
    [System.Serializable]
    private struct StarJson
    {
        public float dec;
        public float ra;
        public float dist;
    }

    public void RequestStars(string starID)
    {
        StartCoroutine(GetStarsFromServer(starID));
    }

    IEnumerator GetStarsFromServer(string starId)
    {
        string url = serverUrl + stars_url + starId;
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Request error: " + request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Server Response: " + responseText);

            // Parse the JSON response
            StarsWrapper wrapper = JsonUtility.FromJson<StarsWrapper>(responseText);

            List<Star> stars = new List<Star>();

            // Convert the parsed StarJson to Star struct
            foreach (var starJson in wrapper.stars)
            {
                Star star = new Star
                {
                    declination = starJson.dec,
                    ascension = starJson.ra,
                    distance = starJson.dist
                };
                stars.Add(star);
            }

            // Invoke the UnityEvent with the parsed stars
            onStarsReceived?.Invoke(stars);
            uic.ExpoplanetLoded(starId);
        }
    }
}


// Custom Editor for the NetworkManager class
#if UNITY_EDITOR
[CustomEditor(typeof(NetworkManager))]
public class NetworkManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NetworkManager networkManager = (NetworkManager)target;

        // Add a button to trigger the GetStarsFromServer function manually
        if (GUILayout.Button("Get Stars From Server"))
        {
            networkManager.RequestStars(networkManager.starID);
        }
    }
}
#endif