using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using game4automation;
using HiS.XR.Utils;

namespace MR_Training_ASSAR.ProcessSystem
{
    public class WaitBehaviour : MonoBehaviour
    {
        public bool Game4Automation;
        public bool UseCollider;
        WorkObjectInterface _workObjectBehaviour;

        private BoxCollider _waitercollider;
        // Start is called before the first frame update
        public UnityEvent OnCondition;

        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(UseCollider))]
        [Header("Set Collider Trigger: ")]
        public GameObject WaitFor;

        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(Game4Automation))]
        [Header("Game4Automation Trigger: ")]
        [SerializeField] bool PLCOutput;
        [SerializeField] bool PLCInput;
        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(PLCOutput))]
        [SerializeField] PLCOutputBool OutputTrigger;
        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(PLCInput))]
        [SerializeField] PLCInputBool InputTrigger;
        private bool PreviousState=false;


        public void Initialize()
        {
            _workObjectBehaviour = gameObject.GetComponent<WorkObjectInterface>();
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            _waitercollider = gameObject.GetComponent<BoxCollider>();
            this.enabled=false;
            if(!UseCollider)
            {
                _waitercollider.enabled = false;
            }
        }

        private void Update()
        {
            if(Game4Automation)
            {
                Game4AutomationTrigger();
            }
        }

        private void Game4AutomationTrigger()
        {
            if (PLCOutput)
            {
                if (OutputTrigger.Value && !PreviousState)
                {
                    StopWaiting();
                }
                PreviousState = OutputTrigger.Value;
            }
            else if (PLCInput)
            {
                if (InputTrigger.Value && !PreviousState)
                {
                    StopWaiting();
                }
                PreviousState = InputTrigger.Value;
            }
        }

        private void OnEnable()
        {

           gameObject.GetComponent<BoxCollider>().enabled = true;

        }

        private void OnDisable()
        {
                gameObject.GetComponent<BoxCollider>().enabled = false;

        }

        private void OnTriggerEnter(Collider other)
        {
            if(UseCollider && this.enabled && other.gameObject.tag==WaitFor.tag)
            {
                    gameObject.GetComponent<WorkObjectInterface>().TaskFinished();
                    OnCondition.Invoke();

            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (UseCollider && this.enabled && other.gameObject.tag == WaitFor.tag)
            {
                    gameObject.GetComponent<WorkObjectInterface>().TaskFinished();
                    OnCondition.Invoke();
                Debug.Log("triger stay");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(this.enabled && other.gameObject.tag == WaitFor.tag)
            {
                gameObject.GetComponent<WorkObjectInterface>().TaskFinished();
                OnCondition.Invoke();
            }
        }

        //If we want to call manually
        public void StopWaiting()
        {
            if (this.enabled)
            {
                _workObjectBehaviour.TaskFinished();
                OnCondition.Invoke();
            }
        }
    }
}
