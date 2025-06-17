using System.Collections;
using System.Collections.Generic;
using Meta.XR.BuildingBlocks;
using UnityEngine;

public class ModelChanger : MonoBehaviour
{
    public GameObject spatialAnchorController;
    public List<GameObject> prefabs = new List<GameObject>();

    public void SwitchModel(int modelID)
    {
        SpatialAnchorManager spatialAnchorScript = spatialAnchorController.GetComponent<SpatialAnchorManager>();
        spatialAnchorScript.anchorPrefab = prefabs[modelID].GetComponent<OVRSpatialAnchor>();
    }
}
