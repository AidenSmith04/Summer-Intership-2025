using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alwaysFaceUser : MonoBehaviour
{

    private GameObject userReference;
    private GameObject self;
    // Start is called before the first frame update
    void Start()
    {
        self = gameObject;
        userReference = GameObject.Find("CenterEyeAnchor");
    }

    // Update is called once per frame
    void Update()
    {
        self.transform.eulerAngles = new Vector3(self.transform.eulerAngles.x, userReference.transform.eulerAngles.y+180, self.transform.eulerAngles.z);
    }
}
