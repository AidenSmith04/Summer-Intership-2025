using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AnchorPlacement : MonoBehaviour
{
    public GameObject anchorPrefa;
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            CreateSpatialAnchor();
        }
    }

    public void CreateSpatialAnchor()
    {
        GameObject prefab = Instantiate(anchorPrefa, OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch), OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch));
        //addComponent adds component to relative game object
        prefab.AddComponent<OVRSpatialAnchor>();
    }
}