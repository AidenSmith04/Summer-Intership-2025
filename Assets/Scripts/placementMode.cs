using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class placementMode : MonoBehaviour
{

    public int mode;
    [SerializeField] TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        mode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = mode.ToString();

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch)){
            mode = (mode + 1) % 5;
        };
    }
}
