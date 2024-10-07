using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InputController : MonoBehaviour
{
    public GameObject viewContext;

    // Start is called before the first frame update
    public void ChangeView(string gyroData)
    {
        // Split the gyroData string by commas
        string[] gyroValues = gyroData.Split(',');
        float gyroX;
        float gyroY;
        float gyroZ;

        // Parse the string values back into float (or int, depending on your needs)
        if (gyroValues.Length == 3)
        {
            gyroX = float.Parse(gyroValues[0]);
            gyroY = float.Parse(gyroValues[1]);
            gyroZ = float.Parse(gyroValues[2]);

            // Now you can use these values to control the view or perform other actions
            Debug.Log($"Gyroscope data received: X = {gyroX}, Y = {gyroY}, Z = {gyroZ}");

            // Example: Rotate the camera or object based on the gyro data
            // This is just an example of how to apply the values
            viewContext.transform.Rotate(gyroX, gyroY, gyroZ);
        }
        else
        {
            Debug.LogError("Invalid gyroscope data received.");
        }
        // Rotate the view context based on the gyro values

    }

    // Optional: Function to reset the view (useful for the editor button)
    public void ResetView()
    {
        viewContext.transform.rotation = Quaternion.identity;
    }

    // Custom editor code block
#if UNITY_EDITOR
    [CustomEditor(typeof(InputController))]
    public class InputControllerEditor : Editor
    {
        // Variables to input the gyro values in the editor
        float gyroX = 0.0f;
        float gyroY = 0.0f;
        float gyroZ = 0.0f;

        public override void OnInspectorGUI()
        {
            // Draw default inspector first
            DrawDefaultInspector();

            // Add space for custom editor fields
            EditorGUILayout.Space();

            // Display input fields for gyro values
            EditorGUILayout.LabelField("Custom Gyro Rotation");
            gyroX = EditorGUILayout.FloatField("Gyro X", gyroX);
            gyroY = EditorGUILayout.FloatField("Gyro Y", gyroY);
            gyroZ = EditorGUILayout.FloatField("Gyro Z", gyroZ);

            // Add a button that rotates the view when clicked
            if (GUILayout.Button("Rotate View"))
            {
                // Reference the target script
                InputController script = (InputController)target;
                // Call the ChangeView function with custom gyro values
                //use string format to send the gyro values as a string
                string gyroData = string.Format("{0},{1},{2}", gyroX, gyroY, gyroZ);
                script.ChangeView(gyroData);
            }

            // Optional: Add a button to reset the view
            if (GUILayout.Button("Reset View"))
            {
                // Reference the target script
                InputController script = (InputController)target;
                // Reset the view context rotation
                script.ResetView();
            }
        }
    }
#endif
}
