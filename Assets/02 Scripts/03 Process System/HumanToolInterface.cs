using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MR_Training_ASSAR.ShowcaseCommunications;
using Leap.Unity.Interaction;
using HiS.XR.Utils;

namespace MR_Training_ASSAR.ProcessSystem
{
    public class HumanToolInterface : ObjectInterface
    {
        public Collider HTcollider { get; set; }
        public InteractionBehaviour HTgraspable { get; set; }
        public Outline HToutline { get; set; }

        [Header("Introduce Tool")]
        [SerializeField] GameObject m_HumanTool;

        public GameObject HumanToolGO { get; set; }

        [Header("Human Tool Functionalities")]
        [SerializeField] bool m_MagneticBehaviour = true;
        [SerializeField] bool m_welderMachine = false;
        [SerializeField] bool m_screwBehaviour = false;
        [SerializeField] bool m_brushBehaviour = false;

        [Header("Magnetic Behaviour")]
        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(m_MagneticBehaviour))]
        [SerializeField] private GameObject m_EndPoint;
        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(m_MagneticBehaviour))]
        [SerializeField] bool m_Dinamic = false;
        [SerializeField] bool m_NormalParent = false;


        private enum ParentSelector
        {
            NormalParent,
            Dinamic,
        }

        [Header("Type of Parent: ")]
        public bool SetParent = false;
        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(SetParent))]
        [SerializeField] private ParentSelector m_ParentSelector;
        [SerializeField] private GameObject m_HTParent;

        private bool _MagneticBehaviour;
        private bool _WelderBehaviour;
        private bool _BrushBehaviour;
        private bool _ScrewBehaviour;

        public GameObject EndPoint { get; set; }
        public bool Dinamic { get; set; } = false;
        public bool NormalParent { get; set; } = false;
        public GameObject HTParent { get; set; }

        private MagneticBehaviour _magneticBehaviour;
        private WeldingBehaviour _weldingBehaviour;
        private ScrewBehaviour _screwBehaviour;
        private BrushBehaviour _brushBehaviour;

        GameObject SCManagerGO;
        private Invoker _invoker;
        private Command _finishCommand;
        private bool _real = false;

        private void Awake()
        {
            gameObject.tag = GLV.HTBehaviourTag;

            HumanToolGO = m_HumanTool;
            _MagneticBehaviour = m_MagneticBehaviour;
            _WelderBehaviour = m_welderMachine;
            _BrushBehaviour = m_brushBehaviour;
            _ScrewBehaviour = m_screwBehaviour;
            if(SetParent)
            {
                if(m_ParentSelector==0)
                {
                    Dinamic = false;
                    NormalParent = true;
                }
                else
                {
                    Dinamic = true;
                    NormalParent = false;
                }
            }
            else
            {
                Dinamic = false;
                NormalParent = false;
            }

            if (HumanToolGO != null)
            {
                if (_MagneticBehaviour)
                {
                    EndPoint = m_EndPoint;
                    HTParent = m_HTParent;
                    gameObject.MustHaveComponent<MagneticBehaviour>();
                    _magneticBehaviour = gameObject.GetComponent<MagneticBehaviour>();
                    _magneticBehaviour.Initialize();
                }
                if (_WelderBehaviour) //for welding
                {
                    gameObject.MustHaveComponent<WeldingBehaviour>();
                    _weldingBehaviour = gameObject.GetComponent<WeldingBehaviour>();
                    _weldingBehaviour.Initialize();
                    
                }
                if (_BrushBehaviour) //for brushing
                {
                    gameObject.MustHaveComponent<BrushBehaviour>();
                    _brushBehaviour = gameObject.GetComponent<BrushBehaviour>();
                    _brushBehaviour.Initialize();
                }
                if (_ScrewBehaviour) //for welding
                {
                    gameObject.MustHaveComponent<ScrewBehaviour>();
                    _screwBehaviour = gameObject.GetComponent<ScrewBehaviour>();
                    _screwBehaviour.Initialize();
                }
                HumanToolGO.MustHaveComponent<InteractionBehaviour>();
                HTgraspable = HumanToolGO.GetComponent<InteractionBehaviour>();
                HTgraspable.enabled = true;

                HTcollider = HumanToolGO.GetComponent<Collider>();
                HTcollider.enabled = true;

                HumanToolGO.MustHaveComponent<Outline>();
                HToutline = HumanToolGO.GetComponent<Outline>();
                HToutline.enabled = false;
                HToutline.OutlineMode = Outline.Mode.OutlineVisible;
                HToutline.OutlineColor = Color.green;
                HToutline.OutlineWidth = 2f;
            }
            else { Debug.LogError("No Tool selected at " + gameObject.name +  " in " + gameObject.transform.parent.parent.name); }
        }

        private void Start()
        {
            SCManagerGO = GameObject.Find(GLV.ShowcaseManagerName);
            _invoker = SCManagerGO.GetComponent<Invoker>();
            _finishCommand = new FinishedCommand(gameObject, true);
        }

        private void EnableComponents(bool enable)
        {
            if (_MagneticBehaviour)
            {
                _magneticBehaviour.enabled = enable;
                _magneticBehaviour.ToggleMagneticBehaviour(enable);

                if(Dinamic && enable)
                {
                    HTParent = _magneticBehaviour._ObjectParent;
                }
                if (!_real)
                {
                    HTgraspable.enabled = enable;
                    HTcollider.enabled = enable;
                    HToutline.enabled = enable;
                }
            }
            else if (_WelderBehaviour)
            {

                _weldingBehaviour.enabled = enable;

            }
            else if (_BrushBehaviour)
            {
                _brushBehaviour.enabled = enable;
            }
            else if (_ScrewBehaviour)
            {
                _screwBehaviour.enabled = enable;
            }
            if (!_real)
            {
                HTgraspable.enabled = true;
                HTcollider.enabled = true;
                HToutline.enabled = enable;
            }
        }

        public override void StartCommand()
        {
            EnableComponents(true);
            if (_real)
            {
                _finishCommand = new FinishedCommand(gameObject, _real);
                TaskFinished();
            }
        }

        public override void NextCommand()
        {
            EnableComponents(false);
        }

        public override void ResetCommand()
        {
            EnableComponents(false);
            if (_MagneticBehaviour) // ResetPos not common?
            {
                _magneticBehaviour.ResetPos();
            }
            else if (_WelderBehaviour)
            {
                _weldingBehaviour.ResetPos();
            }
            else if( _BrushBehaviour)
            {
                _brushBehaviour.ResetPos();
            }
            else if (_ScrewBehaviour)
            {
                _screwBehaviour.ResetPos();
            }
        }

        public override void BackCommand()
        {
            EnableComponents(false);
            if (_MagneticBehaviour)
            {
                _magneticBehaviour.ResetPos();
            }
            else if (_WelderBehaviour)
            {
                _weldingBehaviour.ResetPos();
            }
            else if (_BrushBehaviour)
            {
                _brushBehaviour.ResetPos();
            }
            else if (_ScrewBehaviour)
            {
                _screwBehaviour.ResetPos();
            }
        }

        public override void SkipCommand()
        {
            EnableComponents(false);
            if (_MagneticBehaviour)
            {
                _magneticBehaviour.FinalPos();
            }
            else if (_WelderBehaviour)
            {
                _weldingBehaviour.FinalPos();
            }
            else if (_BrushBehaviour)
            {
                _brushBehaviour.FinalPos();
            }
            else if (_ScrewBehaviour)
            {
                _screwBehaviour.FinalPos();
            }
            // Animations
        }

        public override void TaskFinished()
        {
            _invoker.ExecuteCommand(_finishCommand);
        }

        public override void ResetShowcase(int index)
        {
            
        }

        public override void ToggleVirtuality(bool real)
        {
            _real = real;
            EnableComponents(!_real);
            _finishCommand = new FinishedCommand(gameObject, _real);
            TaskFinished(); // In the user we trust
        }
    }
}
