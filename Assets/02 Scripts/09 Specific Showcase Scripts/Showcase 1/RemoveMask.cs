using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveMask : MonoBehaviour
{
    #region Variables
    private Transform _initialParent;
    private Vector3 _initialPos;
    private Quaternion _initialRot;
    #endregion

    private void Awake()
    {
        _initialParent = gameObject.transform.parent;
        _initialPos = transform.localPosition;
        _initialRot = transform.localRotation;
    }

    public void Remove_Mask()
    {
        gameObject.transform.parent = _initialParent;
        gameObject.transform.localPosition = _initialPos;
        gameObject.transform.localRotation = _initialRot;
    }
}