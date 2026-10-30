using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Varjo.XR;

public class DepthOclussion : MonoBehaviour
{

    public bool onDepth=false;
    // Start is called before the first frame update
    void Start()
    {
        if (onDepth)
        {
            EnableDepth();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableDepth()
    {
        VarjoMixedReality.EnableDepthEstimation();
    }

    public void DisableDepth()
    {
        VarjoMixedReality.DisableDepthEstimation();
    }
}
