using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MR_Training_ASSAR.ProcessSystem
{
    public class BrushBehaviour : MonoBehaviour
    {
        #region Variables
        // Start is called before the first frame update
        private Dictionary<GameObject, bool> CheckList = new Dictionary<GameObject, bool>();

        Animator animator;

        public GameObject TypeofObject;
        HumanToolInterface _humanTool;

        private bool intialized = false;

        //reset position
        Vector3 initialPos;
        Quaternion initialRot;
        #endregion

        public void Initialize()
        {
            gameObject.MustHaveComponent<HumanToolInterface>();
            _humanTool = gameObject.GetComponent<HumanToolInterface>();
            TypeofObject = _humanTool.HumanToolGO;
            //animator = brush.GetComponent<Animator>();

            //LookForTasks();
            _humanTool = gameObject.GetComponent<HumanToolInterface>();
            //there is no animation for brush
            //brush = _humanTool.HumanToolGO.transform.Find(GLV.BrushName).gameObject;
            //brush.SetActive(false);
            this.enabled = false;
            intialized = true;
        }

        /*void LookForTasks()
        {
            paintPoints = gameObject.transform.Find(GLV.PaintPointsName);
            if (paintPoints == null || paintPoints.childCount == 0)
            {
                Debug.LogError("No Welding points in Holder of " + gameObject.transform.parent.parent.name);
            }
            else
            {
                foreach (Transform child in paintPoints)
                {
                    CheckList.Add(child.gameObject, false);
                    child.gameObject.MustHaveComponent<BrushCollider>();
                }
            }
        }*/

        private void OnEnable()
        {
            if(intialized)
            {
                TypeofObject.GetComponentInChildren<BrushCollider>().Initialize(gameObject);
            }
            
        }
        private void OnDisable()
        {
            if(intialized)
            {
                TypeofObject.GetComponentInChildren<BrushCollider>().TurnOff();
            }
        }
        public void PullTrigger(GameObject obj)
        {
            _humanTool.TaskFinished();
        }
        public void ResetPos()
        {
            TypeofObject.transform.position = initialPos;
            TypeofObject.transform.rotation = initialRot;
        }

        public void FinalPos()
        {
            TypeofObject.transform.position = initialPos;
            TypeofObject.transform.rotation = initialRot;
        }
        private void CheckFinishedtasks()
        {
            bool Welfinished = true;
            foreach (KeyValuePair<GameObject, bool> k in CheckList)
            {
                if (!k.Value)
                {
                    Welfinished = false;
                }
            }
            if (Welfinished)
            {
                _humanTool.TaskFinished();
            }
        }
    }
}