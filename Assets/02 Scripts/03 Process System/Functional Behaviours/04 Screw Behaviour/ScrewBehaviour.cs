using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;


namespace MR_Training_ASSAR.ProcessSystem
{
    public class ScrewBehaviour : MonoBehaviour
    {
        #region Variables
        public float _speed = 2;
        public float screwHeight = 0.01f;
        public float Offset = 0.01f;
        public bool attached = false;
        private bool initialize;

        private bool collision = false;
        private Transform initialParent;
        HumanToolInterface _humanTool;
        ScrewCollider screwCollider;

        private Dictionary<GameObject, bool> CheckList = new Dictionary<GameObject, bool>();

        public GameObject TypeofObject { get; set; }
        Transform ScrewDriverPoints;

        Vector3 initialPos;
        Quaternion initialRot;
        #endregion


        public void Initialize()
        {
            _humanTool = gameObject.GetComponent<HumanToolInterface>();
            TypeofObject = _humanTool.HumanToolGO;
            screwCollider = TypeofObject.GetComponentInChildren<ScrewCollider>();
            initialPos = TypeofObject.transform.position; //Transform del menu inicial
            initialRot = TypeofObject.transform.rotation;
            initialize = true;
            this.enabled = false;
        }

        // Start is called before the first frame update
        void OnEnable()
        {
            if(initialize)
            {
                LookForTasks();
                screwCollider.Initialize(gameObject);
            }
        }

        public void OnDisable()
        {
            if (initialize)
            {
                screwCollider.enabled = false;
            }
        }

        void LookForTasks()
        {
            CheckList.Clear();
            if (GameObject.FindGameObjectsWithTag(GLV.ScrewAssambledTag).Length == 0)
            {
                Debug.Log("No Screws to drill " + gameObject.transform.parent.parent.name);
            }
            else
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag(GLV.ScrewAssambledTag))
                {
                    Debug.Log(obj.name);
                    CheckList.Add(obj, false);
                    obj.GetComponent<Collider>().enabled = true;
                    obj.GetComponent<Outline>().enabled = true;
                }
            }
        }

        // Update is called once per frame

        public void PullTrigger(GameObject obj)
        {
            if (CheckList.ContainsKey(obj) && this.enabled)
            {
                Debug.Log("Trigger");

                CheckList[obj] = true;
                //Disable colliders
                obj.GetComponent<Collider>().enabled = false;
                obj.GetComponent<Outline>().enabled = false;
                //Activate collision sensor 
                ActivateScrew(obj);
                //check finish task
                CheckFinishedtasks();         
            }
        }

        private void CheckFinishedtasks()
        {
            bool Welfinished = true;
            foreach(KeyValuePair<GameObject, bool> k in CheckList)
            {
                if (!k.Value)
                {
                    Welfinished = false;
                }
            }
            if (Welfinished)
            {
                _humanTool.TaskFinished();
                screwCollider.enabled=false;
            }
        }

        private void ActivateScrew(GameObject obj)
        {
            obj.GetComponentInChildren<Animator>().enabled = true;
        }

        public void ResetValues()
        {
            ResetPos();

            gameObject.GetComponent<Collider>().enabled = false;
            Debug.Log("Reset");
            //Disable variables
            collision = false;
            attached = false;

            gameObject.GetComponent<ScrewBehaviour>().enabled = false;
        }

        public void ResetPos()
        {
            transform.position = initialPos;
            transform.rotation = initialRot;
        }
        public void FinalPos()
        {
            TypeofObject.transform.position = initialPos;
            TypeofObject.transform.rotation = initialRot;
        }
        }
    
}
