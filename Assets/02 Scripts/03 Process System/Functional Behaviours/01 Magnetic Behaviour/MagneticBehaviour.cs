using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using MR_Training_ASSAR.ShowcaseCommunications;
using UnityEngine.Events;

namespace MR_Training_ASSAR.ProcessSystem
{
    public class MagneticBehaviour : MonoBehaviour
    {
        #region Variables
        public GameObject TypeofObject;
        private WorkObjectInterface _workObject;
        private HumanToolInterface _humanTool;

        public GameObject Zone { get; set; }
        private Collider _zcollider;
        MeshRenderer _zmeshRenderer;
        CapsuleCollider z_capsuleCollider;
        MagneticCollider _magneticCollider;

        private bool _setParent = false;
        private bool _dinamic = false;
        private bool _normalParent =false;
        public GameObject _ObjectParent;
        private GameObject _initialParent;


        public UnityEvent OnMagnet;
        public UnityEvent AfterMagnet;

        public float _speed = 2;
        public float Offset = 0.01f;
        public bool attached = false;

        private bool _active = false;
        private bool _collision = false;
        private bool _finished = false;
        private bool initialized =false;

        Vector3 initialPos;
        Quaternion initialRot;
        #endregion



        public void Initialize()
        {
            if (gameObject.tag == GLV.WOBehaviourTag)
            {
                gameObject.MustHaveComponent<WorkObjectInterface>(); // Redundant, just in case
                _workObject = gameObject.GetComponent<WorkObjectInterface>();
                TypeofObject = _workObject.WorkObjectGO;
                Zone = _workObject.EndPoint;
                _dinamic = _workObject.Dinamic;
                _ObjectParent = _workObject.WOParent;

                _setParent = _workObject.SetParent;
                _dinamic = _workObject.Dinamic;
                _normalParent = _workObject.NormalParent;

            }
            else if (gameObject.tag == GLV.HTBehaviourTag)
            {
                gameObject.MustHaveComponent<HumanToolInterface>();
                _humanTool = gameObject.GetComponent<HumanToolInterface>();
                TypeofObject = _humanTool.HumanToolGO;
                Zone = _humanTool.EndPoint;
                _dinamic = _humanTool.Dinamic;
                _ObjectParent = _humanTool.HTParent;

                _setParent = _humanTool.SetParent;
                _dinamic = _humanTool.Dinamic;
                _normalParent = _humanTool.NormalParent;
            }
            else
            {
                Debug.LogError("Magnetic Behaviour doesn't recognize its Object Behaviour type");
            }


            _initialParent = TypeofObject.transform.parent.gameObject;
            if (!_setParent)
            {
                _ObjectParent = _initialParent;
            }

            Zone.MustHaveComponent<CapsuleCollider>();
            _zcollider = Zone.GetComponent<Collider>();
            _zmeshRenderer = Zone.GetComponentInChildren<MeshRenderer>();
            z_capsuleCollider = Zone.GetComponent<CapsuleCollider>();
            Zone.MustHaveComponent<MagneticCollider>();
            _magneticCollider = Zone.GetComponent<MagneticCollider>();
            _magneticCollider.behaviourGameObject = gameObject;
            _magneticCollider.Initialize();

            ToggleMagneticBehaviour(false);

            initialPos = TypeofObject.transform.position; //Transform del menu inicial
            initialRot = TypeofObject.transform.rotation;
            this.enabled = false;
            initialized = true;
        }

        public void OnEnable()
        {
            if(initialized)
            {
                ToggleMagneticBehaviour(true);
            }

        }

        public void OnDisable()
        {
            if (initialized)
            {
                ToggleMagneticBehaviour(false);
            }

        }

        public void ToggleMagneticBehaviour(bool enable)
        {
            if(_zmeshRenderer!=null)
            {
                _zmeshRenderer.enabled = enable;
            }
            z_capsuleCollider.enabled = enable;
            _magneticCollider.enabled = enable;
            if(_dinamic)
            {
                _ObjectParent = GameObject.FindGameObjectWithTag(GLV.DinamicParenttag);
            }
        }

        private void SetParentship2ObjectParent(bool enable)
        {
            if(_ObjectParent!=null)
            {
                if (enable)
                {
                    TypeofObject.transform.SetParent(_ObjectParent.transform);
                }
                else
                {
                    TypeofObject.transform.SetParent(_initialParent.transform);
                }
            }
            else
            {
                Debug.LogError("Not Dinamic Parent Found in " + gameObject.transform.parent.parent.name);
            }

        }

        #region Magnetic Functions
        private void Update()
        {
            if(_collision)
            {
                ActivateMagnet(); // It has to be in an Update function because Lerp and Slerp are loop time-dependant functions
            }
        }

        /*private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == _ObjectParent.tag && this.enabled)
            {
                _ObjectParent = collision.gameObject;
            }
        }*/

        private void ActivateMagnet()
        {
            TypeofObject.transform.position = Vector3.Lerp(TypeofObject.transform.position, Zone.transform.position, _speed * Time.deltaTime);
            TypeofObject.transform.rotation = Quaternion.Slerp(TypeofObject.transform.rotation, Zone.transform.rotation, _speed * 1.5f * Time.deltaTime);
            if ((Mathf.Abs(Zone.transform.position.x - TypeofObject.transform.position.x) < Offset) && ((Mathf.Abs(Zone.transform.position.z - TypeofObject.transform.position.z) < Offset)))
            {
                Debug.Log("Attached");

                _collision = false;
                attached = true;

                //PutOn object invisible
                ToggleMagneticBehaviour(false);

                _finished = true;
                SetParentship2ObjectParent(_setParent);
                TypeofObject.transform.position = Zone.transform.position;
                TypeofObject.transform.rotation = Zone.transform.rotation;
                if(gameObject.tag==GLV.WOBehaviourTag)
                {
                    if(!_workObject.changeScale) TypeofObject.transform.localScale = Zone.transform.localScale;
                }
                

                if(AfterMagnet!=null)
                {
                    AfterMagnet.Invoke();
                }

                if (gameObject.tag == GLV.WOBehaviourTag)
                {
                    _workObject.TaskFinished();
                }
                else if (gameObject.tag == GLV.HTBehaviourTag)
                {
                    _humanTool.TaskFinished();
                }
            }
        }

        public void PullTrigger(GameObject obj)
        {
            Debug.Log(gameObject.name + " collision with: " + obj.name);

            _zcollider.enabled = false;
            TypeofObject = obj;
            if (gameObject.tag == GLV.WOBehaviourTag)
            {
                obj.GetComponent<Collider>().enabled = false;
                obj.GetComponent<InteractionBehaviour>().enabled = false;
                obj.GetComponent<Outline>().enabled = false;
                obj.GetComponent<Rigidbody>().isKinematic = true;
            }
            else if (gameObject.tag == GLV.HTBehaviourTag)
            {
                obj.GetComponent<Collider>().enabled = false;
                obj.GetComponent<InteractionBehaviour>().enabled = false;
                obj.GetComponent<Outline>().enabled = false;
            }
            if(OnMagnet!=null)
            {
                OnMagnet.Invoke();
            }
            
            //Activate collision sensor
            _collision = true;
        }
        #endregion

        #region Commanded Functions



        #endregion


        public void ResetValues()
        {
            ResetPos();
            Debug.Log("Reset");
            //Returns to original parent
            //typeofObject.transform.parent = initialParent;
            //Enable colliders
            _workObject.WOcollider.enabled = true;
            _workObject.WOcollider.enabled = true;
            //Enable grasping
            _workObject.WOgraspable.enabled = true;
            //Disable variables
            _collision = false;
            attached = false;
            //PutOn object visible
            ToggleMagneticBehaviour(true);
        }

        public void ResetPos()
        {
            SetParentship2ObjectParent(_setParent);
            TypeofObject.transform.position = initialPos;
            TypeofObject.transform.rotation = initialRot;
        }

        public void FinalPos()
        {
            TypeofObject.transform.position = Zone.transform.position;
            TypeofObject.transform.rotation = Zone.transform.rotation;
            SetParentship2ObjectParent(_setParent);
        }
    }
}
