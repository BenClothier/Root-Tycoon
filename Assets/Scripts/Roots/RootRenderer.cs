using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootRenderer : MonoBehaviour
{
    public Transform lowerBone;
    public Transform upperBone;

    private RootAttributes attributes;

    public void Inititialise(RootAttributes attributes) {
        this.attributes = attributes;
        upperBone.localScale = new Vector3(attributes.OverallGirth, 1, attributes.OverallGirth);
        lowerBone.localScale = new Vector3(attributes.LowerGirth, attributes.Length, attributes.LowerGirth);
    }

    public RootAttributes GetAttributes () {
        return this.attributes;
    }
}
