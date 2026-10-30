using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class DebugPressInteractionButton : MonoBehaviour
{
    public bool PressButton = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PressButton)
        {
            PressButton = false;
            GetComponent<InteractionButton>().OnPress.Invoke();
        }
    }
}
