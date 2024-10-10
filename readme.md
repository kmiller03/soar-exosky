# SOAR EXOSKY ✨

## INPUT DEVICE
Innovative input device provides a reference for the chosen exoplanet and leverages an IMU, a ESP32, and bluetooth technology to control the view of the night sky.

![image](https://github.com/user-attachments/assets/83cdaf0f-0a3b-4875-99f6-9ad4ef70bbf8)
![image](https://github.com/user-attachments/assets/4ce7ef1e-ce16-41e2-9ce8-2a12fbdc2bbb)

## DEVELOPMENT

Clone the repository.

### Exosky Server
`cd` into the `exosky_server` directory and run the following commands:
```
python -m venv venv
./venv/Scrips/activate
python -m pip install -r requirements.txt
```
To run the server, run the following:
```
python app.py
```
Access the server at `http://localhost:5000`

Make sure the virtual environment is always activated before attempting to run the server.

### Exosky Unity

Install **[Unity Hub](https://unity.com/unity-hub)**, complete all the setup and once finished attempt to install **[Unity 2022.3.41f1](https://unity.com/releases/editor/whats-new/2022.3.41)**. You will be asked for which modules to install, select the following:
- [x] Microsoft Visual Studio Community 2022
- [x] WebGL Build Support

Make sure to install **[blender](https://download.blender.org/release/Blender3.4/blender-3.4.0-windows-x64.msi)**

In the `Projects` tab of the Unity Hub, click on: `Add > Add project from disk`. Browse and select the `exosky_unity` folder and click *Add Project*. Once the project is added, click on the project to open it in Unity. The first time loading will be the longest, the subsequent loads will be faster.

### Displaying Stars
By default when playing the Unity Scene, the stars will not be displayed. To display the stars either:
1. **Randomly generate them**: Before playing the scene, select the StarGenerator object in the hierarchy and check the *Randomize Stars* checkbox in the inspector.
2. **Load them from the server**: Make sure the server is running. Play the scene, select the NetworkManager object in the hierarchy, in the inspector enter the name of the exoplanet (e.g: "Proxima Cen b") and click the *Get Stars From Server* button.

Good luck, *MTFBWY*