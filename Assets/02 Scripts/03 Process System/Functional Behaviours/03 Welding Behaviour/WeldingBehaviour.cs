using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MR_Training_ASSAR.ProcessSystem
{
    public class WeldingBehaviour : MonoBehaviour
    {
        #region Variables
        public GameObject spark;
        // Start is called before the first frame update
        private Dictionary<GameObject, bool> CheckList = new Dictionary<GameObject, bool>();

        public GameObject TypeofObject { get; set; }
        public Transform WeldingPoints;
        HumanToolInterface _humanTool;

        //reset position
        Vector3 initialPos;
        Quaternion initialRot;
        #endregion

        public void Initialize()
        {
            _humanTool = gameObject.GetComponent<HumanToolInterface>();
            TypeofObject = _humanTool.HumanToolGO;
            initialPos = TypeofObject.transform.position;
            initialRot = TypeofObject.transform.rotation;

            LookForTasks();
            spark = _humanTool.HumanToolGO.transform.Find(GLV.SparkName).gameObject;
            spark.SetActive(false);
            this.enabled = false;

        }


        public void OnEnable()
        {
            foreach (Transform child in gameObject.transform.Find(GLV.WeldingPointsName))
            {
                child.GetComponent<WeldCollider>().enabled = true;
                child.GetComponent<SimpleLoading>().enabled = true;
            }
        }

        public void OnDisable()
        {
            foreach (Transform child in gameObject.transform.Find(GLV.WeldingPointsName))
            {
                child.GetComponent<WeldCollider>().enabled = false;
                child.GetComponent<SimpleLoading>().enabled = false;
            }
        }


        void LookForTasks()
        {
            WeldingPoints = gameObject.transform.Find(GLV.WeldingPointsName);
            if (WeldingPoints == null || WeldingPoints.childCount == 0)
            {
                Debug.LogError("No Welding points in Holder of " + gameObject.transform.parent.parent.name);
            }
            else
            {
                foreach (Transform child in WeldingPoints)
                {
                    CheckList.Add(child.gameObject, false);
                    child.gameObject.MustHaveComponent<WeldCollider>();
                    child.gameObject.MustHaveComponent<SimpleLoading>();
                }
            }
        }
        public void ReleseTrigger(GameObject obj)
        {
            //spark.SetActive(false);
            //WeldingTime = Time.realtimeSinceStartup - timer;

            if (CheckList.ContainsKey(obj)) //&& WeldingTime >= 0.5)
            {
                CheckList[obj] = true;
                //timer = 0f;
                //WeldingTime = 0f;
                CheckFinishedtasks();
            }
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