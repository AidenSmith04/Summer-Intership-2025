using UnityEngine;

public class VRMenuController : MonoBehaviour
{
    public GameObject menuCanvas;  // The menu Canvas
    public float distanceInFront = 2.0f;  // Distance from the user's view
    private Transform headTransform; // Centre position for the head
    public OVRCameraRig cameraRig;

    void Start()
    {
        // Get the head transform from the OVRCameraRig
        if (cameraRig != null)
        {
            headTransform = cameraRig.centerEyeAnchor;  // Use centerEyeAnchor for head position
        }

        // Ensure the menu starts hidden
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // Toggle menu visibility when pressing the A button
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            ToggleMenu();
        }

        // If the menu is active, position it in front of the user
        if (menuCanvas.activeSelf && headTransform != null)
        {
            PositionMenu();
        }
    }

    void ToggleMenu()
    {
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }

    void PositionMenu()
    {
        // Position the menu in front of the camera (user's head)
        menuCanvas.transform.position = headTransform.position + headTransform.forward * distanceInFront;

        // Rotate the menu to face the user
        menuCanvas.transform.rotation = Quaternion.LookRotation(headTransform.forward);
    }
}
