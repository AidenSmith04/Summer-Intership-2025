using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SpatialAnchorManager : MonoBehaviour
{
    public OVRSpatialAnchor anchorPrefab;
    public const string NumUuidsPlayerPref = "numUuids";

    private GameObject controllerReference;
    private Canvas canvas;
    private TextMeshProUGUI uuidText;
    private TextMeshProUGUI savedStatusText;
    private List<OVRSpatialAnchor> anchors = new List<OVRSpatialAnchor>();
    private OVRSpatialAnchor lastCreatedAnchor;
    private AnchorLoader anchorLoader;
    // Start is called before the first frame update
    void Start()
    {
        controllerReference =  GameObject.Find("LeftHandAnchor");
    }

    private void Awake() => anchorLoader = GetComponent<AnchorLoader>();

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            CreateSpatialAnchor();
        }
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            SaveLastCreatedAnchor();
        }   
        /*if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            UnsaveLastCreatedAnchor();
        }*/
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            UnsaveAllAnchors();
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch))
        {
            LoadSavedAnchors();
        }
    }

    public void CreateSpatialAnchor(){
        OVRSpatialAnchor workingAnchor = Instantiate(anchorPrefab,OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch),Quaternion.Euler(0,controllerReference.transform.eulerAngles.y-96,0));

        canvas = workingAnchor.gameObject.GetComponentInChildren<Canvas>();
        uuidText = canvas.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        savedStatusText = canvas.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        StartCoroutine(AnchorCreated(workingAnchor));
    }

    private IEnumerator AnchorCreated(OVRSpatialAnchor workingAnchor){

        while (!workingAnchor.Created && !workingAnchor.Localized){
            yield return new WaitForEndOfFrame();
        }

        Guid anchorGuid = workingAnchor.Uuid;
        anchors.Add(workingAnchor);
        lastCreatedAnchor = workingAnchor;

        uuidText.text = "UUID, Spatial Anchor Manager " + anchorGuid.ToString();
        savedStatusText.text = "Not Saved, SAM";
    }

    private void SaveLastCreatedAnchor(){
        lastCreatedAnchor.Save((lastCreatedAnchor, success) =>{
            if(success){
                savedStatusText.text = "Saved the string for you :)";
            }
        });
        SaveUuidToPlayerPrefs(lastCreatedAnchor.Uuid);
    }

    void SaveUuidToPlayerPrefs(Guid uuid){
        if(!PlayerPrefs.HasKey(NumUuidsPlayerPref)){
            PlayerPrefs.SetInt(NumUuidsPlayerPref, 0);
        }

        int playerNumbUuids = PlayerPrefs.GetInt(NumUuidsPlayerPref);
        PlayerPrefs.SetString("uuid" + playerNumbUuids, uuid.ToString());
        PlayerPrefs.SetInt(NumUuidsPlayerPref, ++playerNumbUuids);
    }

    private void UnsaveLastCreatedAnchor(){
        lastCreatedAnchor.Erase((lastCreatedAnchor, success) => {
        if (success){
            savedStatusText.text = "Not Saved";
            }
        });
    }

    private void UnsaveAllAnchors(){
        foreach (var anchor in anchors){
            UnsaveAnchor(anchor);
        }
        anchors.Clear();
        ClearAllUuidsFromPlayerPrefs();
    }

    private void UnsaveAnchor(OVRSpatialAnchor anchor){
        anchor.Erase((erasedAnchor, success) => {
            if (success){
                var textComponents = erasedAnchor.GetComponentsInChildren<TextMeshProUGUI>();
                if(textComponents.Length>1){
                    var savedStatusText = textComponents[1];
                    savedStatusText.text = "Not daved nuh uhhh";
                }
            }
        });
    }

    private void ClearAllUuidsFromPlayerPrefs(){
        if(PlayerPrefs.HasKey(NumUuidsPlayerPref)){
            int playerNumUuids = PlayerPrefs.GetInt(NumUuidsPlayerPref);
            for(int i = 0; i<playerNumUuids;i++){
                PlayerPrefs.DeleteKey("uuid" + i);
            }
            PlayerPrefs.DeleteKey(NumUuidsPlayerPref);
            PlayerPrefs.Save();
        }
    }

    public void LoadSavedAnchors(){
        anchorLoader.LoadAnchorsByUuid();
    }

}
