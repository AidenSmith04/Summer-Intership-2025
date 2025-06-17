using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectHider : MonoBehaviour
{
    private GameObject userPositionReference;

    private int hidingDistance = 40;

    private GameObject self;

    private Vector3 initialScale;

    // Start is called before the first frame update
    void Start()
    {
        self = gameObject;
        userPositionReference = GameObject.Find("CenterEyeAnchor");
        initialScale = self.transform.localScale;
    }

    void Update()
    {
        var distanceFromReference = (userPositionReference.transform.position-self.transform.position).magnitude;
        // not the most elegant way of hiding but this way is independent from the meh renderer
        if (distanceFromReference > hidingDistance) {
            self.transform.localScale = new Vector3(0, 0, 0);
        } else {
            self.transform.localScale = initialScale;
        }
    }

}