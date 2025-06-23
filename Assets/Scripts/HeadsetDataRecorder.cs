using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR;

public class HeadsetDataRecorder : MonoBehaviour
{
    private InputDevice leftHand;
    private bool isRecording = false;
    private StreamWriter writer;
    private string logDir;
    private float logStartTime;

    private bool wasGripPressedLastFrame = false;

    void Start()
    {
        logDir = Path.Combine(Application.persistentDataPath, "Logs");
        Directory.CreateDirectory(logDir);
        InitializeLeftHand();
    }

    void InitializeLeftHand()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        if (devices.Count > 0)
            leftHand = devices[0];
    }

    void Update()
    {
        if (!leftHand.isValid)
            InitializeLeftHand();

        if (leftHand.TryGetFeatureValue(CommonUsages.gripButton, out bool gripPressed))
        {
            if (gripPressed && !wasGripPressedLastFrame)
            {
                // Button was just pressed
                ToggleLogging();
            }

            wasGripPressedLastFrame = gripPressed;
        }

        if (isRecording)
        {
            Vector3 position = Camera.main.transform.position;
            Quaternion rotation = Camera.main.transform.rotation;
            float elapsedTime = Time.time - logStartTime;

            string logLine = $"{elapsedTime:F4},{position.x:F4},{position.y:F4},{position.z:F4}," +
                             $"{rotation.x:F4},{rotation.y:F4},{rotation.z:F4},{rotation.w:F4}";

            writer.WriteLine(logLine);
            writer.Flush();
        }
    }

    void ToggleLogging()
    {
        if (isRecording)
        {
            Debug.Log("🔴 Stopping recording.");
            StopLogging();
        }
        else
        {
            Debug.Log("🟢 Starting new recording.");
            StartLogging();
        }
    }

    void StartLogging()
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string filename = $"session_{timestamp}.csv";
        string filepath = Path.Combine(logDir, filename);

        try
        {
            writer = new StreamWriter(filepath, false);
            writer.WriteLine("Time,PosX,PosY,PosZ,RotX,RotY,RotZ,RotW");
            Debug.Log("Log file created at: " + filepath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to create log file: " + e.Message);
        }

        logStartTime = Time.time;
        isRecording = true;
    }


    void StopLogging()
    {
        isRecording = false;
        writer?.Close();
    }

    void OnApplicationQuit()
    {
        if (isRecording)
            writer?.Close();
    }
}
