using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MR_Training_ASSAR.UserInterfaceSystem
{
    public class MRToggle : MonoBehaviour
    {
        [Header("Game Objects Needed")]
        [SerializeField] private GameObject UserInterfaceMenuGameObject;
        [SerializeField] private GameObject AssetControlButton;
        [Header("Virtual Elements")]
        [SerializeField] private GameObject VirtualEnvironment;
        [SerializeField] private GameObject AluminiumTable;
        [SerializeField] private GameObject WorkBench;
        [SerializeField] private GameObject RobotBase;

        public UnityEvent QRScanning;
        public UnityEvent QRDetected;

        private UIM uim;
        private DepthOclussion depthOclussion;
        private VarjoMarkerManager varjoMarkerManager;

        // Start is called before the first frame update
        void Start()
        {
            depthOclussion = GetComponent<DepthOclussion>();
            varjoMarkerManager = GetComponent<VarjoMarkerManager>();
            uim = UserInterfaceMenuGameObject.GetComponent<UIM>();
            AssetControlButton.SetActive(false);
        }

        public void VR2MR(bool RealEnvironment)
        {
            QRScanning.Invoke();
            StartCoroutine(WaitSomeTime());
            if (VirtualEnvironment)
            {
                VirtualEnvironment.SetActive(!RealEnvironment);
            }
            if (AluminiumTable)
            {
                AluminiumTable.SetActive(!RealEnvironment);
            }
            if (RobotBase)
            {
                RobotBase.SetActive(!RealEnvironment);
            }

            //varjoMarkerManager.enabled = RealEnvironment;
            AssetControlButton.SetActive(RealEnvironment);
            if (RealEnvironment)
            {
                //depthOclussion.EnableDepth();
            }
            else
            {
                //depthOclussion.DisableDepth();
            }

            StartCoroutine(WaitSomeTime());
        }

        public void ARTogggle()
        {
            VR2MR(true);
            uim.WorkObjectButton();
            uim.HumanToolButton();
            uim.RobotButton();
            uim.RobotToolButton();
            AssetControlButton.SetActive(false);
            if (WorkBench)
            {
                WorkBench.SetActive(false);
            }
        }

        private IEnumerator WaitSomeTime()
        {
            yield return new WaitForSeconds(5F);
            QRDetected.Invoke();
            varjoMarkerManager.enabled = false;
            yield break;
        }
    }
}
