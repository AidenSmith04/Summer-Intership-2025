using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bobjectRotator : MonoBehaviour
{
    private GameObject controllerReference;

    private float lastRotation;

    private float lastControllerRotation;

    private bool isRotating;

    private GameObject self;

    // Start is called before the first frame update
    void Start()
    {
        self = gameObject;
        lastRotation = 0;
        lastControllerRotation = 0;
        isRotating = false;
        controllerReference =  GameObject.Find("RightHandAnchor");
    }

    void Update()
    {

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)){
            lastRotation = self.transform.eulerAngles.y;
            lastControllerRotation = controllerReference.transform.eulerAngles.y;
            isRotating = true;
        };

        if (isRotating) {
            float delta = controllerReference.transform.eulerAngles.y - lastControllerRotation;
            self.transform.eulerAngles = new Vector3(self.transform.eulerAngles.x, lastRotation - delta, self.transform.eulerAngles.z);
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)){
            isRotating = false;
        };


        
    }
}
