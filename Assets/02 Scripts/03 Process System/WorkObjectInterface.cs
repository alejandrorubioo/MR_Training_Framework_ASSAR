using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MR_Training_ASSAR.ShowcaseCommunications;
using Leap.Unity.Interaction;
using HiS.XR.Utils;

namespace MR_Training_ASSAR.ProcessSystem
{
    public class WorkObjectInterface : ObjectInterface
    {
        public Collider WOcollider { get; set; }
        public InteractionBehaviour WOgraspable { get; set; }
        public Outline WOoutline { get; set; }

        [Header("Introduce Work Object Characteristics")]
        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(m_MagneticBehaviour))]
        [SerializeField] GameObject m_WorkObject;
        [SerializeField] bool m_Source = false;

        public GameObject WorkObjectGO { get; set; }

        [Header("Work Object Functionalities")]
        [SerializeField] bool m_MagneticBehaviour = true;
        [SerializeField] bool m_WaitUntil = false;
        [Header("Magnetic Behaviour")]
        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(m_MagneticBehaviour))]
        [SerializeField] private GameObject m_EndPoint;
        private enum ParentSelector
        {
            NormalParent,
            Dinamic,
        }

        public bool changeScale = false;
        [Header("Type of Parent: ")]
        public bool SetParent = false;
        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(SetParent))]
        [SerializeField] private ParentSelector m_ParentSelector;
        [SerializeField] private GameObject m_HTParent;


        private bool _MagneticBehaviour;
        private bool _WaitBehaviour;
        private MagneticBehaviour _magneticBehaviour;
        private WaitBehaviour _waitBehaviour;
        public GameObject EndPoint { get; set; }
        public bool Dinamic { get; set; } = false;
        public bool NormalParent { get; set; } = false;
        public GameObject WOParent { get; set; }

        GameObject SCManagerGO;
        private Invoker _invoker;
        private Command _finishCommand;
        private bool _real = false;

        // Must Execute earlier than Showcase Manager's
        private void Awake()
        {
            gameObject.tag = GLV.WOBehaviourTag;
            _WaitBehaviour = m_WaitUntil;
            _MagneticBehaviour = m_MagneticBehaviour;

            if (SetParent)
            {
                if (m_ParentSelector==0)
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

            if (m_WorkObject != null)
            {
                WorkObjectGO = m_WorkObject;
                if (_MagneticBehaviour)
                {
                    EndPoint = m_EndPoint;

                    gameObject.MustHaveComponent<MagneticBehaviour>();
                    _magneticBehaviour = gameObject.GetComponent<MagneticBehaviour>();
                    _magneticBehaviour.Initialize();

                    WorkObjectGO.MustHaveComponent<InteractionBehaviour>();
                    WOgraspable = WorkObjectGO.GetComponent<InteractionBehaviour>();
                    WOcollider = WorkObjectGO.GetComponent<Collider>();


                    WorkObjectGO.MustHaveComponent<Outline>();
                    WOoutline = WorkObjectGO.GetComponent<Outline>();
                    WOoutline.enabled = false;
                    WOoutline.OutlineMode = Outline.Mode.OutlineVisible;
                    WOoutline.OutlineColor = Color.green;
                    WOoutline.OutlineWidth = 2f;
                }
                if (_WaitBehaviour) // In case the user puts a Work Object with Wait Beahaviour
                {
                    gameObject.MustHaveComponent<WaitBehaviour>();
                    _waitBehaviour = gameObject.GetComponent<WaitBehaviour>();
                    _waitBehaviour.Initialize();
                }
            }
            else if (_WaitBehaviour)
            {
                gameObject.MustHaveComponent<WaitBehaviour>();
                _waitBehaviour = gameObject.GetComponent<WaitBehaviour>();
                _waitBehaviour.Initialize();
            }
            else
            {
                Debug.LogError("No Work Object selected at" + gameObject.name + " in " + gameObject.transform.parent.parent.name);
            }
        }

        private void Start()
        {
            SCManagerGO = GameObject.Find(GLV.ShowcaseManagerName);
            _invoker = SCManagerGO.GetComponent<Invoker>();
            _finishCommand = new FinishedCommand(gameObject, true); // check
        }

        private void EnableComponents(bool enable)
        {
            if (_MagneticBehaviour)
            {
                _magneticBehaviour.enabled = enable;

                if (!_real)
                {
                    //WOgraspable.enabled = enable;
                    //WOcollider.enabled = enable;
                    //WOoutline.enabled = enable;
                    if(Dinamic && enable)
                    {
                        WOParent = _magneticBehaviour._ObjectParent;
                    }

                    if (enable)
                    {
                        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(WorkObjectGO.tag))
                        {
                            obj.GetComponent<Outline>().enabled = enable;
                            obj.GetComponent<InteractionBehaviour>().enabled = enable;
                            obj.GetComponent<Collider>().enabled = enable;
                        }
                    }
                    else
                    {
                        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(WorkObjectGO.tag))
                        {
                            obj.GetComponent<Outline>().enabled = enable;
                        }
                    }
                }
            }
            if(_WaitBehaviour)
            {
                _waitBehaviour.enabled = enable;
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

            if (m_Source)
            {
                WOoutline.enabled = false;
            }
        }

        public override void NextCommand()
        {
            EnableComponents(false);
        }

        public override void ResetCommand()
        {
            EnableComponents(false);
            if (_MagneticBehaviour)
            {
                _magneticBehaviour.ResetPos();
            }
        }

        public override void BackCommand()
        {
            EnableComponents(false);
            if(_MagneticBehaviour)
            {
                _magneticBehaviour.ResetPos();
            }
            if (_WaitBehaviour)
            {
                _waitBehaviour.Initialize();
            }
        }

        public override void SkipCommand()
        {
            EnableComponents(false);
            // Change pos to next
            if (_MagneticBehaviour)
            {
                _magneticBehaviour.FinalPos();
            }
            if(_WaitBehaviour)
            {
                _waitBehaviour.StopWaiting();
            }
        }

        public override void TaskFinished()
        {
            _invoker.ExecuteCommand(_finishCommand);
        }

        public override void ResetShowcase(int index)
        {
            // When Game4automation generates MU it adds -[index] to the name of the GameObject Generated
            WorkObjectGO = GameObject.Find(WorkObjectGO.name + "-" + index);

            if (_MagneticBehaviour)
            {
                EndPoint = GameObject.Find(EndPoint.name + "-" + index);
                if (Dinamic && WOParent.tag == GLV.WOTag)
                {
                    WOParent = GameObject.Find(WOParent.name + "-" + index);
                }
                _magneticBehaviour.Initialize();
                _magneticBehaviour.enabled = false;
            }
            if (_WaitBehaviour)
            {
                _waitBehaviour.Initialize();
            }
        }

        public override void ToggleVirtuality(bool real)
        {
            _real = real;
            //EnableComponents(!_real);
            _finishCommand = new FinishedCommand(gameObject, _real);
            TaskFinished(); // In the user we trust
        }
    }
}
