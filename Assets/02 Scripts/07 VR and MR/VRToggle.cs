using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR_Training_ASSAR
{
    public class VRToggle : MonoBehaviour
    {
        [SerializeField] private GameObject VirtualEnvironment;
        [SerializeField] private GameObject AluminiumTable;

        public List<Transform> movableObjects;

        private List<Vector3> initialPos;
        private List<Quaternion> initialRot;

        private DepthOclussion depthOclussion;
        private VarjoMarkerManager varjoMarkerManager;

        // Start is called before the first frame update
        void Awake()
        {
            depthOclussion = GetComponent<DepthOclussion>();
            varjoMarkerManager = GetComponent<VarjoMarkerManager>();
            initialPos = new List<Vector3>();
            initialRot = new List<Quaternion>();

            foreach (Transform obj in movableObjects)
            {
                initialPos.Add(obj.position);
                initialRot.Add(obj.rotation);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ButtonPressed()
        {
            if (VirtualEnvironment) VirtualEnvironment.SetActive(true);
            if (AluminiumTable) AluminiumTable.SetActive(true);
            depthOclussion.DisableDepth();
            varjoMarkerManager.enabled = false;

            for (int i=0; i<movableObjects.Count; i++)
            {
                movableObjects[i].transform.position = initialPos[i];
                movableObjects[i].transform.rotation = initialRot[i];
            }
        }
    }
}

