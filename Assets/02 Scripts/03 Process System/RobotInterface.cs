using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Globalization;
using MR_Training_ASSAR.ShowcaseCommunications;
using game4automation;
using UnityEngine.Events;

namespace MR_Training_ASSAR.ProcessSystem
{
    public class RobotInterface : ObjectInterface
    {
        [HideInInspector] public bool DigitalTwin = false;
        public string m_message;
        public GameObject Axis1;
        public GameObject Axis2;
        public GameObject Axis3;
        public GameObject Axis4;
        public GameObject Axis5;
        public GameObject Axis6;

        private List<float> storedJointValues;
        private TCPTestClient _TCP;

        public GameObject TCPCom;

        public PLCInputBool enableStep;
        public PLCOutputBool robotEnd;
        public PLCOutputBool ack;

        GameObject SCManagerGO;
        private Invoker _invoker;
        private Command _finishCommand;
        private bool _real = false;

        public UnityEvent RobotFinished;

        private void Awake()
        {
            gameObject.tag = GLV.RobotBehaviourTag;
        }

        // Start is called before the first frame update
        void Start()
        {
            TCPCom.SetActive(false);
            TCPCom.GetComponent<TCPTestClient>().enableSync = false;
            _TCP = TCPCom.GetComponent<TCPTestClient>();

            SCManagerGO = GameObject.Find(GLV.ShowcaseManagerName);
            _invoker = SCManagerGO.GetComponent<Invoker>();
            _finishCommand = new FinishedCommand(gameObject, true);

            //StartRobotInteraction(m_message);
        }


        // Update is called once per frame
        private void Update()
        {
            if (_real && _TCP.enableSync)
            {
                _TCP.enableSync = false;
                AssignJointValues();
            }
        }

        #region Commands
        public override void StartCommand()
        {
            if (_real)
            {
                StartRobotInteraction(m_message);
            }
            else
            {

                StartCoroutine(WaitToDisable());
                enableStep.Value = true;
                StartCoroutine(WaitToSetFalse());

            }
        }

        public override void TaskFinished()
        {
            _finishCommand = new FinishedCommand(gameObject, true);
            RobotFinished.Invoke();
            _invoker.ExecuteCommand(_finishCommand);
        }

        public override void NextCommand()
        {
            //Next?
        }

        public override void BackCommand()
        {
            // ??
        }

        public override void SkipCommand()
        {
            // ??
        }

        public override void ToggleVirtuality(bool real)
        {
            _real = real;
        }
        #endregion

        #region Communication Methods with Real Robot
        public void StartRobotInteraction(string message)
        {
            TCPCom.SetActive(true);
            _TCP.SetMessage(message);
            _TCP.ConnectToTcpServer();
            StartCoroutine(WaitForDone());
        }
        private IEnumerator WaitForDone()
        {
            yield return new WaitUntil(() => _TCP.GetMessage() == "done");
            TCPCom.SetActive(false);
            TaskFinished();
            yield break;
        }
        public void AssignJointValues()
        {
            ReadJointValues();
            //Depending on the motion angle they will acquire one coordinate or other
            Axis1.transform.localEulerAngles = new Vector3(0, 0, -storedJointValues[0]);
            Axis2.transform.localEulerAngles = new Vector3(0, -storedJointValues[1], 0);
            Axis3.transform.localEulerAngles = new Vector3(0, -storedJointValues[2], 0);
            Axis4.transform.localEulerAngles = new Vector3(storedJointValues[3], 0, 0);
            Axis5.transform.localEulerAngles = new Vector3(0, -storedJointValues[4], 0);
            Axis6.transform.localEulerAngles = new Vector3(storedJointValues[5], 0, 0);
        }
        private void ReadJointValues()
        {
            string jointValues = _TCP.rapidPos;
            string storeValue = "";

            storedJointValues = new List<float>();

            for (int i = 0; i < jointValues.Length; i++)
            {

                if (';' != jointValues[i] && i < jointValues.Length - 1)
                {
                    storeValue += jointValues[i];

                }
                else
                {
                    //Stores the values inside a float list for later being assigned to the propper axis
                    storedJointValues.Add(float.Parse(storeValue, CultureInfo.InvariantCulture));
                    storeValue = "";

                }
            }
        }
        #endregion

        #region Communication Methods with Virtual Robot
        private IEnumerator WaitToSetFalse()
        {
            yield return new WaitUntil(() => ack.Value == true);
            enableStep.Value = false;
            yield break;
        }
        private IEnumerator WaitToDisable()
        {
            yield return new WaitUntil(() => robotEnd.Value == true);
            TaskFinished();
            yield break;
        }
        #endregion
    }
}
