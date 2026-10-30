using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HiS.XR.Utils;
using TMPro;

namespace MR_Training_ASSAR.ShowcaseCommunications
{
    public class AssetControl : MonoBehaviour
    {
        #region Variables
        [SerializeField] private GameObject AssetPanel;
        [SerializeField] private GameObject ToggleModel;
        private GameObject TextCanvas;
        private float PositionOffsetStep = 0.05F;
        private float ScaleOffsetStep = 0.10F; // Not Implemented

        [Header("Assets' Organization")]
        [SerializeField] private List<GameObject> WorkObjects;
        [SerializeField] private List<GameObject> HumanTools;
        [SerializeField] private List<GameObject> VirtualRobotTools;
        [SerializeField] private List<GameObject> RealRobotTools;

        private List<GameObject> VirtualRobotVisibleParts;
        private List<GameObject> RealRobotVisibleParts;
        private List<MeshRenderer> VirtualRobotMeshRendererParts = new List<MeshRenderer>();
        private List<MeshRenderer> RealRobotMeshRendererParts = new List<MeshRenderer>();

        private GameObject _currentAsset;

        private Showcase_Manager _scManager;
        private GameObject _scManagerGO;
        public static GLV.Assets AssetType;

        #endregion

        //In the future we may want to deactivate mor objects than the ones in the parent so we create the lists in case we want to add a new object like the table.
        //In the process, the function deletes the parent but it could deactive the whole list

        #region Init Methods
        public void Start()
        {
            _scManagerGO = GameObject.Find(GLV.ShowcaseManagerName);
            _scManager = _scManagerGO.GetComponent<Showcase_Manager>();
            TextCanvas = AssetPanel.transform.Find(GLV.TextName).gameObject;
            TextCanvas.MustHaveComponent<TextMeshPro>();
            
            VirtualRobotVisibleParts = new List<GameObject>(GameObject.FindGameObjectsWithTag(GLV.VirtualRobotInvisiblePartsTag));
            RealRobotVisibleParts = new List<GameObject>(GameObject.FindGameObjectsWithTag(GLV.RealRobotInvisiblePartsTag));
            foreach (GameObject obj in VirtualRobotVisibleParts)
            {
                VirtualRobotMeshRendererParts.Add(obj.GetComponent<MeshRenderer>());
            }

            foreach (GameObject obj in RealRobotVisibleParts)
            {
                RealRobotMeshRendererParts.Add(obj.GetComponent<MeshRenderer>());
            }

            ToggleRobots(false);
            //DisplayAssetsInCheckList(WorkObjects);
        }

        /*private void FindWithTags() // Not Implemented
        {
            WorkObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag(GLV.WOTag));
            HumanTools = new List<GameObject>(GameObject.FindGameObjectsWithTag(GLV.HTTag));
            VirtualRobots = new List<GameObject>(GameObject.FindGameObjectsWithTag(GLV.RobotTag));
            VirtualRobotTools = new List<GameObject>(GameObject.FindGameObjectsWithTag(GLV.RobotToolsTag));
        }*/
        #endregion

        #region Checklist Methods
        private void DisplayAssetsInCheckList(List<GameObject> Assets)
        {
            string assetList = "";
            TextMeshPro textMeshPro = TextCanvas.GetComponent<TextMeshPro>();

            float PositionOffset = 0F;
            float ScaleOffset = 0F;
            foreach (GameObject asset in Assets)
            {
                assetList += asset.name + "\n";

                Vector3 TogglePosition = new Vector3(ToggleModel.transform.position.x, ToggleModel.transform.position.y - PositionOffset, ToggleModel.transform.position.z);
                Instantiate(ToggleModel, TogglePosition, ToggleModel.transform.rotation, AssetPanel.transform);
                PositionOffset += PositionOffsetStep;

                ScaleOffset += ScaleOffsetStep;
            }
            textMeshPro.SetText(assetList);
        }
        #endregion

        #region Toggle Methods
        public void ToggleAssets(GLV.Assets assets, bool real)
        {
            switch (assets)
            {
                case GLV.Assets.WorkObject:
                    ToggleAssets(WorkObjects, real);
                    //_scManager.InstanciateWOVirtuality(real);
                    break;
                case GLV.Assets.HumanTool:
                    ToggleAssets(HumanTools, real);
                    _scManager.InstanciateHTVirtuality(real);
                    break;
                case GLV.Assets.Robot:
                    ToggleRobots(real); // Robots cannot become inactive but invisble so the comms still work
                    _scManager.InstanciateRobotVirtuality(real);
                    break;
                case GLV.Assets.RobotTool:
                    ToggleAssets(RealRobotTools, real);
                    break;
                default:
                    Debug.LogError("Asset not implemented in " + this.name);
                    break;
            }
            _scManager.UpdateAssets(assets, real);
        }

        private void ToggleAssets(List<GameObject> Assets, bool real)
        {
            foreach (GameObject Asset in Assets)
            {
                Asset.SetActive(!real);
            }
        }

        private void ToggleRobots(bool real)
        {
            foreach (MeshRenderer mesh in VirtualRobotMeshRendererParts)
            {
                mesh.enabled = !real;
            }
            foreach (MeshRenderer mesh in RealRobotMeshRendererParts)
            {
                mesh.enabled = real;
            }
            ToggleAssets(VirtualRobotTools, real);
            ToggleAssets(RealRobotTools, !real);
        }

        #endregion
    }
}
