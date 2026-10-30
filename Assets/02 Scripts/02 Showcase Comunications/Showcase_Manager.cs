using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MR_Training_ASSAR.ProcessSystem;
using MR_Training_ASSAR.UserInterfaceSystem;
using MR_Training_ASSAR.InstructionSystem;
using HiS.XR.Utils;
using TMPro;
using TwinCAT.PlcOpen;

namespace MR_Training_ASSAR.ShowcaseCommunications
{
    public class Showcase_Manager : MonoBehaviour
    {
        #region Variables

        public bool nextButton = false;

        [Header("Operator Mode")]
        public int operator_index;
        [SerializeField] public int operator_mode;

        [Header("Secuence of Steps")]
        [SerializeField] private bool RealProcessDiffersFromVirtual;
        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(RealProcessDiffersFromVirtual))]
        [SerializeField] private GLV.AssetMenuSelector DependentOn;

        [SerializeField] private int _currentStep = -1; // [SerializeField] for Debug

        [SerializeField] private List<GameObject> m_Steps;
        private List<GameObject> Steps;


        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(RealProcessDiffersFromVirtual))]
        [SerializeField] private List<GameObject> RealStepsToBeSkipped;
        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(RealProcessDiffersFromVirtual))]
        [SerializeField] private List<GameObject> RealStepsToBeCompleted;

        //Timer
        [Header("Timer")]
        private float timer;
        private float initialStepTimer;
        private bool stepRunning = false;
        [SerializeField] private List<float> Times;



        [Header("Game Objects Needed")]
        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(GLV.PLC))] // Not tested
        [SerializeField] private game4automation.PLCOutputBool m_HMIInspecting;

        public GameObject UIMGameObject;
        UIM uiManager;

        public GameObject InstructionPanel;
        InstructionManagement instructionManagement;

        // Empty GameObjects that contains the WO, HT or Robots of a given step
        Transform StepWorkObjectContainer;
        Transform StepHumanToolContainer;
        Transform StepRobotContainer;

        // Lists of WO, HT or Robots of a given step
        List<GameObject> StepWOList;
        List<GameObject> StepHTList;
        List<GameObject> StepRobotList;

        // Dictionaries that contains the Lists of WO or HT of every step
        Dictionary<int, List<GameObject>> WODictionary = new Dictionary<int, List<GameObject>>();
        Dictionary<int, List<GameObject>> HTDictionary = new Dictionary<int, List<GameObject>>();
        Dictionary<int, List<GameObject>> RobotDictionary = new Dictionary<int, List<GameObject>>();

        // Lists of Behaviours of a given step
        List<WorkObjectInterface> StepWOBehaviours;
        List<HumanToolInterface> StepHTBehaviours;
        List<RobotInterface> StepRobotBehaviour = new List<RobotInterface>();

        // Dictionaries that contains the Lists of WO, HT or Robot Behaviour of every step
        Dictionary<int, List<WorkObjectInterface>> WOBehaviourDictionary = new Dictionary<int, List<WorkObjectInterface>>();
        Dictionary<int, List<HumanToolInterface>> HTBehaviourDictionary = new Dictionary<int, List<HumanToolInterface>>();
        Dictionary<int, List<RobotInterface>> RobotBehaviourDictionary = new Dictionary<int, List<RobotInterface>>();

        // Dictionary that controls the finished tasks of a given step
        Dictionary<GameObject, bool> StepWOControlDictionary = new Dictionary<GameObject, bool>();
        Dictionary<GameObject, bool> StepHTControlDictionary = new Dictionary<GameObject, bool>();
        Dictionary<GameObject, bool> StepRobotControlDictionary = new Dictionary<GameObject, bool>();

        // Asset Control
        Dictionary<int, bool> AssetControlDictionary = new Dictionary<int, bool>();



        [SerializeField]
        private TextMeshProUGUI firstMinute;
        [SerializeField]
        private TextMeshProUGUI secondMinute;
        [SerializeField]
        private TextMeshProUGUI separator;
        [SerializeField]
        private TextMeshProUGUI firstSecond;
        [SerializeField]
        private TextMeshProUGUI secondSecond;

        // Number of cicles
        [Header("Cyclic mode")]
        public bool cyclicMode = true;
        [ShowIfAttribute(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(cyclicMode))]
        [SerializeField] private int cycleNumber;


        //auto next
        public bool autoNext = true;

        // Command Varibles
        private Invoker _invoker;
        private Command _start, _next, _reset, _back, _skip, _virtuality, _nextProcess;
        private List<Command> _startList = new List<Command>();
        private List<Command> _nextList = new List<Command>();
        private List<Command> _resetList = new List<Command>();
        private List<Command> _backList = new List<Command>();
        private List<Command> _skipList = new List<Command>();
        private List<Command> _virtualityList = new List<Command>();
        private List<Command> _nextProcessList = new List<Command>();
        #endregion

        #region Instantiations Functions
        private void Awake()
        {
            Steps = m_Steps;

            gameObject.name = GLV.ShowcaseManagerName;
            gameObject.MustHaveComponent<Invoker>();
            _invoker = gameObject.GetComponent<Invoker>();
            cycleNumber = 1;
            for (int i = 0; i < (int)GLV.Assets.NumberofTypes; i++)
            {
                AssetControlDictionary.Add(i, false);
            }
        }

        // Must Execute later than Objects' Awake
        private void Start()
        {
            InitiateAllData();
            uiManager = UIMGameObject.GetComponent<UIM>();
            instructionManagement = InstructionPanel.GetComponent<InstructionManagement>();

            Operator_Condition();

            if (m_HMIInspecting != null) StartProcess();    //If it is Showcase 3

            if (GLV.PLC)
            {
                StartCoroutine(WaitInspectingStart());
            }
        }
        private void Update()
        {
            if (stepRunning)
            {
                timer += Time.deltaTime;
                UpdateTimerDisplay(timer);
                if (nextButton)
                {
                    Next();
                    nextButton = false;
                }
            }
            else if (nextButton)
            {

                StartProcess();
                nextButton = false;
            }


        }

        private void InitiateAllData()
        {
            int index = 0;
            if (Steps.Count == 0)
            {
                Debug.LogError("No Steps in Showcase Manager");
            }
            foreach (GameObject step in Steps)
            {
                StepWorkObjectContainer = step.transform.Find(GLV.WorkObjectContainerName);
                StepHumanToolContainer = step.transform.Find(GLV.HumanToolContainerName);
                StepRobotContainer = step.transform.Find(GLV.RobotToolContainerName);

                // New instances of these lists must be created to avoid problems with the dictionaries
                // storing pointers to these list instead of storing the actual values (GameObjects)
                StepWOList = new List<GameObject>();
                StepHTList = new List<GameObject>();
                StepRobotList = new List<GameObject>();
                StepWOBehaviours = new List<WorkObjectInterface>();
                StepHTBehaviours = new List<HumanToolInterface>();
                StepRobotBehaviour = new List<RobotInterface>();

                if (StepWorkObjectContainer != null)
                {
                    foreach (Transform child in StepWorkObjectContainer)
                    {
                        StepWOList.Add(child.gameObject);
                        child.gameObject.MustHaveComponent<WorkObjectInterface>();
                        StepWOBehaviours.Add(child.gameObject.GetComponent<WorkObjectInterface>());
                    }
                }
                else
                {
                    //Debug.Log("No Work Object Container in " + step.name);
                }

                if (StepHumanToolContainer != null)
                {
                    foreach (Transform child in StepHumanToolContainer)
                    {
                        StepHTList.Add(child.gameObject);
                        child.gameObject.MustHaveComponent<HumanToolInterface>();
                        StepHTBehaviours.Add(child.gameObject.GetComponent<HumanToolInterface>());
                    }
                }
                else
                {
                    //Debug.Log("No Human Tool Container in " + step.name);
                }

                if (StepRobotContainer != null)
                {
                    foreach (Transform child in StepRobotContainer)
                    {
                        StepRobotList.Add(child.gameObject);
                        child.gameObject.MustHaveComponent<RobotInterface>();
                        StepRobotBehaviour.Add(child.gameObject.GetComponent<RobotInterface>());
                    }
                }

                WODictionary.Add(index, StepWOList);
                HTDictionary.Add(index, StepHTList);
                RobotDictionary.Add(index, StepRobotList);

                WOBehaviourDictionary.Add(index, StepWOBehaviours);
                HTBehaviourDictionary.Add(index, StepHTBehaviours);
                RobotBehaviourDictionary.Add(index, StepRobotBehaviour);
                stepRunning = false;

                index++;
            }
        }

        public void StartProcess()
        {
            stepRunning = true;
            if (_currentStep == -1) { _currentStep = 0; }
            else
            {
                for (int i = _currentStep; i > 0; i--)
                {
                    Back();
                }
            }
            // Add Showcase timer
            ResetTimer();

            _currentStep = 0;
            ChangeOfStep(_currentStep);
        }
        #endregion

        #region Step Functions
        private void ChangeOfStep(int step)
        {
            Debug.Log("step: " + step + "out of " + Steps.Count);

            if (step == Steps.Count)
            {
                if (cyclicMode)
                {
                    NextProcessCommand();
                }
                else
                {
                    uiManager.EnableFinishResetShowcaseButton(true);
                }

            }
            else if (step != 0)
            {
                Times.Add(timer - initialStepTimer);
                initialStepTimer = timer;
                InitiateStepData(step);
                InstanciateCommands();
                CheckAutomaticNext();
                StartCommands();
            }
            else
            {
                InitiateStepData(step);
                InstanciateCommands();
                CheckAutomaticNext();
                StartCommands();
            }

        }
        private void InitiateStepData(int step)
        {
            if (step < 0)
            {
                Debug.LogError("Step non existing");
                step = 0;
            }
            else
            {
                StepWOControlDictionary = new Dictionary<GameObject, bool>(); // should we kill old one?
                StepWOList = new List<GameObject>();
                StepWOBehaviours = new List<WorkObjectInterface>();
                foreach (GameObject obj in WODictionary[step])
                {
                    StepWOControlDictionary.Add(obj, false);
                    StepWOList.Add(obj);
                    StepWOBehaviours.Add(obj.GetComponent<WorkObjectInterface>());
                }

                StepHTControlDictionary = new Dictionary<GameObject, bool>();
                StepHTList = new List<GameObject>();
                StepHTBehaviours = new List<HumanToolInterface>();
                foreach (GameObject obj in HTDictionary[step])
                {
                    StepHTControlDictionary.Add(obj, false);
                    StepHTList.Add(obj);
                    StepHTBehaviours.Add(obj.GetComponent<HumanToolInterface>());
                }

                StepRobotControlDictionary = new Dictionary<GameObject, bool>();
                StepRobotList = new List<GameObject>();
                StepRobotBehaviour = new List<RobotInterface>();
                foreach (GameObject obj in RobotDictionary[step])
                {
                    StepRobotControlDictionary.Add(obj, false);
                    StepRobotList.Add(obj);
                    StepRobotBehaviour.Add(obj.GetComponent<RobotInterface>());
                }
            }
        }

        private void InstanciateCommands()
        {
            _startList.Clear();
            _nextList.Clear();
            _resetList.Clear();
            _backList.Clear();
            _skipList.Clear();
            _nextProcessList.Clear();
            foreach (WorkObjectInterface WO in StepWOBehaviours)
            {
                _start = new StartCommand(WO);
                _startList.Add(_start);

                _next = new NextCommand(WO);
                _nextList.Add(_next);

                _reset = new ResetCommand(WO);
                _resetList.Add(_reset);

                _back = new BackCommand(WO);
                _backList.Add(_back);

                _skip = new SkipCommand(WO);
                _skipList.Add(_skip);
            }

            foreach (HumanToolInterface HT in StepHTBehaviours)
            {
                _start = new StartCommand(HT);
                _startList.Add(_start);

                _next = new NextCommand(HT);
                _nextList.Add(_next);

                _reset = new ResetCommand(HT);
                _resetList.Add(_reset);

                _back = new BackCommand(HT);
                _backList.Add(_back);

                _skip = new SkipCommand(HT);
                _skipList.Add(_skip);
            }

            foreach (RobotInterface Robot in StepRobotBehaviour)
            {
                _start = new StartCommand(Robot);
                _startList.Add(_start);

                _next = new NextCommand(Robot);
                _nextList.Add(_next);

                _reset = new ResetCommand(Robot);
                _resetList.Add(_reset);

                _back = new BackCommand(Robot);
                _backList.Add(_back);

                _skip = new SkipCommand(Robot);
                _skipList.Add(_skip);
            }
        }

        // If there are no virtual objects in this step the Next Button is always active
        private void CheckAutomaticNext()
        {
            bool NoVirtualWO = false;
            bool NoVirtualHT = false;
            bool NoVirtualRobot = false;
            if (AssetControlDictionary[(int)GLV.Assets.WorkObject] || StepWOBehaviours == null)
            {
                NoVirtualWO = true;
            }
            if (AssetControlDictionary[(int)GLV.Assets.HumanTool] || StepHTBehaviours == null)
            {
                NoVirtualHT = true;
            }
            if (AssetControlDictionary[(int)GLV.Assets.Robot] || StepRobotBehaviour == null)
            {
                NoVirtualRobot = true;
            }

            uiManager.EnableNextButton(NoVirtualWO && NoVirtualHT && NoVirtualRobot);
        }

        public void taskFinish(GameObject obj, bool finished)
        {
            bool WOreal = false;
            bool HTreal = false;
            bool Robotreal = false;
            switch (obj.tag)
            {
                case GLV.WOBehaviourTag:
                    if (AssetControlDictionary[(int)GLV.Assets.WorkObject])
                    {
                        WOreal = true;
                    }
                    else
                    {
                        StepWOControlDictionary[obj] = finished;
                    }
                    break;
                case GLV.HTBehaviourTag:
                    if (AssetControlDictionary[(int)GLV.Assets.HumanTool])
                    {
                        HTreal = true;
                    }
                    else
                    {
                        StepHTControlDictionary[obj] = finished;
                    }
                    break;
                case GLV.RobotBehaviourTag:
                    if (AssetControlDictionary[(int)GLV.Assets.Robot])
                    {
                        Robotreal = true;
                    }
                    else
                    {
                        StepRobotControlDictionary[obj] = finished;
                    }
                    break;
                default:
                    Debug.LogError("Error in Showcase Manager taskFinish(). Component type not valid");
                    break;
            }
            CheckFinishedTasks(WOreal, HTreal, Robotreal);  // RobotReal if there is no next for robot?? Next could be operator's ack
        }

        private void CheckFinishedTasks(bool WOreal, bool HTreal, bool Robotreal)
        {
            // Check if list is null
            bool WOfinished = true;
            bool HTfinished = true;
            bool Robotfinished = true;
            int index = 0;

            while (!WOreal && WOfinished && index < StepWOList.Count)
            {
                if (!StepWOControlDictionary[StepWOList[index]])
                {
                    WOfinished = false;
                }
                index++;
            }
            index = 0;
            while (!HTreal && WOfinished && HTfinished && index < StepHTList.Count)
            {
                if (!StepHTControlDictionary[StepHTList[index]])
                {
                    HTfinished = false;
                }
                index++;
            }
            index = 0;
            while (!Robotreal && WOfinished && HTfinished && Robotfinished && index < StepRobotList.Count) // !RobotReal??
            {
                if (!StepRobotControlDictionary[StepRobotList[index]])
                {
                    Robotfinished = false;
                }
                index++;
            }

            if (WOfinished && HTfinished && Robotfinished && !autoNext)
            {
                uiManager.EnableNextButton(true);
            }
            else if (WOfinished && HTfinished && Robotfinished && autoNext)
            {
                Next();
            }
            else
            {
                uiManager.EnableNextButton(false);
            }
        }
        #endregion

        private void Operator_Condition()
        {
            if (operator_index % 2 == 0)
            {
                operator_mode = 1;
            }
            else
            {
                operator_mode = 2;
            }

        }

        #region Timer
        private void ResetTimer()
        {
            timer = 0f;
            initialStepTimer = 0f;
            Times.Clear();
        }

        private void UpdateTimerDisplay(float time)
        {
            float minutes = Mathf.FloorToInt(time / 60);
            float seconds = Mathf.FloorToInt(time % 60);

            string currentTime = string.Format("{00:00}{01:00}", minutes, seconds);
            if (firstMinute != null && secondMinute != null && firstSecond != null && secondSecond != null)
            {
                firstMinute.text = currentTime[0].ToString();
                secondMinute.text = currentTime[1].ToString();
                firstSecond.text = currentTime[2].ToString();
                secondSecond.text = currentTime[3].ToString();
            }

        }
        private void PrintStepTimes()
        {
            float minutes;
            float seconds;
            float milliseconds;
            Debug.Log("Step Times");
            int i = 0;
            foreach (float _time in Times)
            {
                minutes = Mathf.FloorToInt(_time / 60);
                seconds = Mathf.FloorToInt(_time % 60);
                milliseconds = (_time - Mathf.Floor(_time)) * 100; // calcular los milisegundos
                string stepTime = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
                Debug.Log("Step " + i + "-> " + stepTime.ToString());
                i++;
            }
            minutes = Mathf.FloorToInt(timer / 60);
            seconds = Mathf.FloorToInt(timer % 60);
            milliseconds = (timer - Mathf.Floor(timer)) * 100; // calcular los milisegundos
            string finalTime = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
            Debug.Log("Step " + i + "-> " + finalTime.ToString());
            ResetTimer();
        }

        #endregion

        #region Asset Control Communication
        public void UpdateAssets(GLV.Assets asset, bool real)
        {
            if ((int)DependentOn == (int)asset) // DependentOn and asset are differente types of Enum so a conversion is needed
            {
                Change2RealProcess();
            }
            AssetControlDictionary[(int)asset] = real;
            VirtualityCommand();
            _virtualityList.Clear();
        }

        private void Change2RealProcess()
        {
            SkipAllVirtualSteps();
            Steps = RealStepsToBeCompleted;
            InitiateAllData();
            StartProcess();
        }

        private void SkipAllVirtualSteps()
        {
            int _stepindex = 0;
            // It would be more efficiente if RealStepsToBeSkipped was an array of ints
            foreach (GameObject step in Steps)
            {
                foreach (GameObject step2skip in RealStepsToBeSkipped)
                {
                    if ((step == step2skip) && (_stepindex >= _currentStep))
                    {
                        ChangeOfStep(_stepindex);
                        SkipWithoutChangeOfStep();
                    }
                }
                _stepindex++;
            }
        }

        public void InstanciateWOVirtuality(bool real)
        {
            for (int i = 0; i < Steps.Count; i++)
            {
                foreach (WorkObjectInterface WO in WOBehaviourDictionary[i])
                {
                    _virtuality = new VirtualityCommand(WO, real);
                    _virtualityList.Add(_virtuality);
                }
            }
        }

        public void InstanciateHTVirtuality(bool real)
        {
            for (int i = 0; i < Steps.Count; i++)
            {
                foreach (HumanToolInterface HT in HTBehaviourDictionary[i])
                {
                    _virtuality = new VirtualityCommand(HT, real);
                    _virtualityList.Add(_virtuality);
                }
            }
        }

        public void InstanciateRobotVirtuality(bool real)
        {
            for (int i = 0; i < Steps.Count; i++)
            {
                foreach (RobotInterface Robot in RobotBehaviourDictionary[i])
                {
                    _virtuality = new VirtualityCommand(Robot, real);
                    _virtualityList.Add(_virtuality);
                }
            }
        }

        public void ToggleAutoNext()
        {
            autoNext = !autoNext;
        }

        #endregion

        #region Reset Process
        IEnumerator WaitInspectingStart()
        {
            yield return new WaitUntil(() => m_HMIInspecting.Value == true);
            StartCoroutine(WaitInspectingOver());
            yield break;
        }

        IEnumerator WaitInspectingOver()
        {
            yield return new WaitUntil(() => m_HMIInspecting.Value == false);
            ResetShowcaseProcess();
            yield break;
        }


        public void ResetShowcaseProcess()
        {
            InstanciateWOResetShowcase();
            NextProcessCommand();

            if (GLV.PLC)
            {
                StartCoroutine(WaitInspectingStart());
            }
            else
            {
                uiManager.EnableStartButton(true);
            }
            //Disable meshrenderers at first
            //StepsGameobject.transform.GetComponent<SC3_MoveToInvisibleRef>().ResetMeshRenderers();
        }

        public void InstanciateWOResetShowcase()
        {
            for (int i = 0; i < Steps.Count; i++)
            {
                foreach (WorkObjectInterface WO in WOBehaviourDictionary[i])
                {
                    _nextProcess = new NewProcessCommand(WO, GLV.PLC_MU_index);
                    _nextProcessList.Add(_nextProcess);
                }
            }
        }

        #endregion

        #region Commands
        private void StartCommands()
        {
            foreach (Command start in _startList)
            {
                _invoker.ExecuteCommand(start);
            }
        }

        public void Next()
        {
            foreach (Command next in _nextList)
            {
                _invoker.ExecuteCommand(next);
            }

            instructionManagement.Next();
            _currentStep++;

            ChangeOfStep(_currentStep);
        }

        public void ResetStep()
        {
            foreach (Command reset in _resetList)
            {
                _invoker.ExecuteCommand(reset);
            }

            ChangeOfStep(_currentStep);
        }

        public void Back()
        {
            foreach (Command reset in _resetList)
            {
                _invoker.ExecuteCommand(reset);
            }

            if (_currentStep > 0)
            {
                instructionManagement.Back();
                _currentStep--;
                timer = Times[_currentStep];
                ChangeOfStep(_currentStep);

                foreach (Command reset in _resetList)
                {
                    _invoker.ExecuteCommand(reset);
                }
                StartCommands();
            }
        }

        public void Skip()
        {
            if (_currentStep < Steps.Count - 1)
            {
                foreach (Command skip in _skipList)
                {
                    _invoker.ExecuteCommand(skip);
                }

                instructionManagement.Next();

                _currentStep++;
                ChangeOfStep(_currentStep);
            }
        }

        private void SkipWithoutChangeOfStep()
        {
            if (_currentStep < Steps.Count - 1)
            {
                foreach (Command skip in _skipList)
                {
                    _invoker.ExecuteCommand(skip);
                }
            }
        }

        private void VirtualityCommand()
        {
            foreach (Command virtuality in _virtualityList)
            {
                _invoker.ExecuteCommand(virtuality);
            }
        }



        private void NextProcessCommand()
        {
            /*foreach (Command nextProcess in _nextProcessList)
            {
                _invoker.ExecuteCommand(nextProcess);
            }*/

            instructionManagement.Reset2FirstInstruction();
            cycleNumber++;
            PrintStepTimes();
            Debug.Log("Next cycle");
            _currentStep = 0;
            ChangeOfStep(_currentStep);
        }
        #endregion
    }
}
