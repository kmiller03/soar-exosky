using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using textmesh pro
using TMPro;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class ui_controller : MonoBehaviour
{
    public string exoplanetNameString;
    //text object for exoplanet name
    public TextMeshProUGUI exoplanetName;
    public NetworkManager networkManager;
    public void SelectExoplanet(string planetName)
    {
        //Change the text of the exoplanet name
        exoplanetName.text = "LOADING ...";
        //Invoke the event
        Debug.Log("planetName: " + planetName);
        networkManager.RequestStars(planetName);
    }
    public void ExpoplanetLoded(string name){
        exoplanetName.text = name;

    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(ui_controller))]
public class ui_controllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ui_controller myScript = (ui_controller)target;
        if (GUILayout.Button("Select Exoplanet"))
        {
            myScript.SelectExoplanet(myScript.exoplanetNameString);
        }
    }
}
#endif


