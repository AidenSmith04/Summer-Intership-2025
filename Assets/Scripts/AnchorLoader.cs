using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class AnchorLoader : MonoBehaviour
{

    private OVRSpatialAnchor anchorPrefab;
    private SpatialAnchorManager spatialAnchorManager;

    Action<OVRSpatialAnchor.UnboundAnchor, bool> _onLoadAnchor;

    public void Awake(){
        spatialAnchorManager = GetComponent<SpatialAnchorManager>();
        anchorPrefab = spatialAnchorManager.anchorPrefab;
        _onLoadAnchor = OnLocalized;
    }

    public void LoadAnchorsByUuid(){
        if(!PlayerPrefs.HasKey(SpatialAnchorManager.NumUuidsPlayerPref)){
            PlayerPrefs.SetInt(SpatialAnchorManager.NumUuidsPlayerPref,0);
        }

        var playerUuidCount = PlayerPrefs.GetInt(SpatialAnchorManager.NumUuidsPlayerPref);
        if(playerUuidCount == 0)
            return;
        
        var uuids = new Guid[playerUuidCount];
        for(int i = 0; i<playerUuidCount;++i){
            var uuidKey = "uuid" + i;
            var currentUuid = PlayerPrefs.GetString(uuidKey);
            uuids[i] = new Guid(currentUuid);
        }

        Load(new OVRSpatialAnchor.LoadOptions{
            Timeout = 0,
            StorageLocation = OVRSpace.StorageLocation.Local,
            Uuids = uuids
        });
    }

    private void Load(OVRSpatialAnchor.LoadOptions options){
        OVRSpatialAnchor.LoadUnboundAnchors(options,anchors =>
        {
            if(anchors==null){
                return;
            }
            foreach(var anchor in anchors){
                if(anchor.Localized){
                    _onLoadAnchor(anchor, true);
                }
                else if(!anchor.Localizing){
                    anchor.Localize(_onLoadAnchor);
                }
            }
        });
    }

    private void OnLocalized(OVRSpatialAnchor.UnboundAnchor unboundAnchor,bool succes){
        if(!succes)
            return;
        
        var pose = unboundAnchor.Pose;
        var spatialAnchor = Instantiate(anchorPrefab,pose.position,pose.rotation);

        unboundAnchor.BindTo(spatialAnchor);

        if(spatialAnchor.TryGetComponent<OVRSpatialAnchor>(out var anchor)){
            var uuidText = spatialAnchor.GetComponentInChildren<TextMeshProUGUI>();
            var savedStatusText = spatialAnchor.GetComponentsInChildren<TextMeshProUGUI>()[1];

            uuidText.text = "UUID: " + spatialAnchor.Uuid.ToString();
            savedStatusText.text = "Loaded from device"; 
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
