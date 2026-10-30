using System.Collections.Generic;
using MR_Training_ASSAR.ShowcaseCommunications;
using MR_Training_ASSAR.GameManagement;
using MR_Training_ASSAR.ProcessSystem;
using UnityEngine;
using HiS.XR.Utils;
using TMPro;

namespace MR_Training_ASSAR.UserInterfaceSystem
{
    public class UIM : MonoBehaviour
    {
        #region Variables

        [Header("Scripts Needed")]
        [SerializeField] private GameObject _scManagerGO;
        [SerializeField] private bool m_Dinamic_Menu = true;

        [Header("Menus and Buttons")]
        [SerializeField] private GameObject m_ExitMenu;
        [SerializeField] private GameObject m_Interaction_StartButton; // Interaction Cube
        [SerializeField] private GameObject m_Interaction_NextButton; // Interaction Cube
        [SerializeField] private GameObject m_WorkObjectButton;
        [SerializeField] private GameObject m_HumanToolButton;
        [SerializeField] private GameObject m_RobotButton;
        [SerializeField] private GameObject m_RobotToolButton;
        //[SerializeField] private GameObject m_FinishButton;
        [SerializeField] private GameObject m_ResetButton;

        private MeshRenderer _startRenderer;
        private MeshRenderer _nextRenderer;
        private MeshRenderer _finishRenderer;
        private MeshRenderer _resetRenderer;
        private TextMeshPro _startText;
        private TextMeshPro _nextText;
        private TextMeshPro _finishText;
        private TextMeshPro _resetText;
        private bool nextenabled = false;

        [Header("Materials")]
        [SerializeField] private Material m_EnabledButtonMaterial;
        [SerializeField] private Material m_DisabledButtonMaterial;

        private List<GameObject> assetControlButtons = new List<GameObject>();

        [Header("Assets Control Dependencies")]
        [SerializeField] private bool m_Robot = true;
        [Space(10)]
        [SerializeField] private GLV.AssetMenuSelector m_WorkObjectDependence;
        [SerializeField] private GLV.AssetMenuSelector m_HumanToolsDependence;

        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(m_Robot))]
        [SerializeField] private GLV.AssetMenuSelector m_RobotDependence;
        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(m_Robot))]
        [SerializeField] private GLV.AssetMenuSelector m_RobotToolsDependence = GLV.AssetMenuSelector.Robot;

        private List<GLV.AssetMenuSelector> assetMenuSelectors = new List<GLV.AssetMenuSelector>();
        private List<GameObject> WODependentButtons = new List<GameObject>();
        private List<GameObject> HTDependentButtons = new List<GameObject>();
        private List<GameObject> RobotDependentButtons = new List<GameObject>();
        private List<GameObject> RobotToolDependentButtons = new List<GameObject>();

        private AssetControl _assetControl;
        Showcase_Manager _showcaseManager;

        GameObject FinalObjectGO;

        public bool m_Single { get; } = true;
        private bool _exitMenu = false;

        private bool _WOreal = false;
        private bool _HTreal = false;
        private bool _Robotreal = false;
        private bool _RobotToolreal = false;
        #endregion

        private void Awake()
        {
            _showcaseManager = _scManagerGO.GetComponent<Showcase_Manager>();
            _assetControl = _scManagerGO.GetComponent<AssetControl>();
            if (GameObject.Find(GLV.FinalEventName) != null) { FinalObjectGO = GameObject.Find(GLV.FinalEventName); }

            _startRenderer = m_Interaction_StartButton.transform.Find("UHI_Cube_Interactable").GetComponent<MeshRenderer>();
            _nextRenderer = m_Interaction_NextButton.transform.Find("UHI_Cube_Interactable").GetComponent<MeshRenderer>();
            //_finishRenderer = m_FinishButton.GetComponent<MeshRenderer>();
            _resetRenderer = m_ResetButton.GetComponent<MeshRenderer>();

            _startText = m_Interaction_StartButton.transform.Find("Text").GetComponent<TextMeshPro>();
            _nextText = m_Interaction_NextButton.transform.Find("Text").GetComponentInChildren<TextMeshPro>();
            //_finishText = m_FinishButton.GetComponent<TextMeshPro>();
            _resetText = m_ResetButton.GetComponent<TextMeshPro>();

            InitiateAssetControlDependences();

            if (m_Dinamic_Menu)
            {
                gameObject.SetActive(false);
                gameObject.MustHaveComponent<SetFromUser>();
            }
        }

        private void InitiateAssetControlDependences()
        {
            // Posible Improvement: Dictionary<GameObject, int>. Conversion from AssetSelector to int and viceversa
            assetControlButtons.Add(m_WorkObjectButton);
            assetControlButtons.Add(m_HumanToolButton);
            assetControlButtons.Add(m_RobotButton);
            assetControlButtons.Add(m_RobotToolButton);

            assetMenuSelectors.Add(m_WorkObjectDependence);
            assetMenuSelectors.Add(m_HumanToolsDependence);
            assetMenuSelectors.Add(m_RobotDependence);
            assetMenuSelectors.Add(m_RobotToolsDependence);

            if (assetControlButtons.Count == assetMenuSelectors.Count)
            {
                for (int i = 0; i < assetControlButtons.Count; i++)
                {
                    FillDependentButtonsLists(assetControlButtons[i], assetMenuSelectors[i]);
                }
            }
            else
            {
                Debug.LogError("The size of the lists of asset button and asset selector are not the same");
            }
        }

        private void FillDependentButtonsLists(GameObject button, GLV.AssetMenuSelector asset)
        {
            switch (asset)
            {
                case GLV.AssetMenuSelector.Independent:
                    button.SetActive(true);
                    break;
                case GLV.AssetMenuSelector.WorkObject:
                    WODependentButtons.Add(button);
                    button.SetActive(false);
                    break;
                case GLV.AssetMenuSelector.HumanTool:
                    HTDependentButtons.Add(button);
                    button.SetActive(false);
                    break;
                case GLV.AssetMenuSelector.Robot:
                    RobotDependentButtons.Add(button);
                    button.SetActive(false);
                    break;
                case GLV.AssetMenuSelector.RobotTools:
                    RobotToolDependentButtons.Add(button);
                    button.SetActive(false);
                    break;
            }
        }

        private void UpdateAssetControlButtons(List<GameObject> AssetDependentButtons, bool real)
        {
            foreach (GameObject button in AssetDependentButtons)
            {
                button.SetActive(real);
            }
            //finishEnabled.SetActive(false);
            //finishDisabled.SetActive(true);
        }

        public void AutoNextButton()
        {
            if (!_showcaseManager.autoNext)
            {
                _showcaseManager.autoNext = true;
            }
            else
            {
                _showcaseManager.autoNext = false;
            }

        }

        // Communicate with the rest of the Architecture

        #region Showcase Manager Communication
        public void EnableStartButton(bool enable)
        {
            if (enable)
            {
                _startRenderer.materials[0] = m_EnabledButtonMaterial;
                _startText.color = Color.white;
            }
            else
            {
                _startRenderer.materials[0] = m_DisabledButtonMaterial;
                _startText.color = Color.red;
            }
            
            //finalEvent._FinalEvent(enable);
            //finishEnabled.SetActive(enable);
            //finishDisabled.SetActive(!enable);
        }

        public void StartProcess()
        {
            _showcaseManager.StartProcess();
            EnableStartButton(false);
            //finishEnabled.SetActive(false);
            //finishDisabled.SetActive(true);
        }

        public void RepeatShowcase()
        {
            GameManager.gameManager.ResetShowcase();
            //finishEnabled.SetActive(false);
            //finishDisabled.SetActive(true);
        }

        public void EnableNextButton(bool enable)
        {
            if (enable)
            {
                _nextRenderer.materials[0] = m_EnabledButtonMaterial;
                _nextText.color = Color.white;
            }
            else
            {
                _nextRenderer.materials[0] = m_DisabledButtonMaterial;
                _nextText.color = Color.red;
            }
            nextenabled = enable;
        }

        public void EnableFinishResetShowcaseButton(bool enable)
        {
            //finalEvent._FinalEvent(enable);
            FinalObjectGO.SetActive(enable);
            //finishButtonEnabled.SetActive(enable);
            //finishButtonDisabled.SetActive(!enable);
            //resetButtonEnabled.SetActive(enable);
            //resetButtonDisabled.SetActive(!enable);
        }

        public void FinishShowCase()
        {
            //finishButtonEnabled.SetActive(false);
            //finishButtonDisabled.SetActive(true);

            // Finish Showcase
        }

        public void ResetShowcase()
        {
            //finishButtonEnabled.SetActive(false);
            //finishButtonDisabled.SetActive(true);
            //resetButtonEnabled.SetActive(false);
            //resetButtonDisabled.SetActive(true);

            _showcaseManager.ResetShowcaseProcess();
            GameManager.gameManager.ResetShowcase();
        }
        #endregion

        #region Command Calls
        // Call GameManager For Application.Quit

        public void NextButton()
        {
            if(nextenabled)
            {
                _showcaseManager.Next();
            }
        }

        public void ResetButton()
        {
            _showcaseManager.ResetStep();
        }

        public void BackButton()
        {
            _showcaseManager.Back();
        }

        public void SkipButton()
        {
            _showcaseManager.Skip();
        }

        #endregion

        #region Asset Control Methods
        private void UIMToggleAssets(GLV.Assets asset, bool real)
        {
            _assetControl.ToggleAssets(asset, real);
        }

        public void WorkObjectButton()
        {
            _WOreal = !_WOreal;
            UIMToggleAssets(GLV.Assets.WorkObject, _WOreal);
            UpdateAssetControlButtons(WODependentButtons, _WOreal);
        }

        public void DissapearWorkObjects()
        {
            _WOreal = !_WOreal;
            UIMToggleAssets(GLV.Assets.WorkObject, _WOreal);
        }

        public void HumanToolButton()
        {
            _HTreal = !_HTreal;
            UIMToggleAssets(GLV.Assets.HumanTool, _HTreal);
            UpdateAssetControlButtons(HTDependentButtons, _HTreal);
        }

        public void RobotButton()
        {
            _Robotreal = !_Robotreal;
            UIMToggleAssets(GLV.Assets.Robot, _Robotreal);
            UpdateAssetControlButtons(RobotDependentButtons, _Robotreal);
        }

        public void RobotToolButton()
        {
            _RobotToolreal = !_RobotToolreal;
            UIMToggleAssets(GLV.Assets.RobotTool, _RobotToolreal);
            UpdateAssetControlButtons(RobotToolDependentButtons, _RobotToolreal);
        }
        #endregion

        #region Exit Game Calls
        public void ExitButton()
        {
            _exitMenu = !_exitMenu;
            m_ExitMenu.SetActive(_exitMenu);
            // Exit Menu Animation and Pause
        }

        public void ConfirmExit()
        {
            GameManager.gameManager.QuitShowcase();
        }

        private void Update()
        {
            if (Input.GetKeyDown("escape"))
            {
                Debug.Log("escape");
                ExitButton();
            }

            if (m_ExitMenu.activeSelf && Input.GetKeyDown("enter"))
            {
                Debug.Log("enter");
                ConfirmExit();
            }
        }
        #endregion
    }
}
