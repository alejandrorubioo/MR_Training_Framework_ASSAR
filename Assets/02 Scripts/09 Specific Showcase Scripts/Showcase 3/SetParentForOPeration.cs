using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace MR_Training_ASSAR.ProcessSystem
{
    public class SetParentForOPeration : MonoBehaviour
    {
        public GameObject dinamicParent;
        public GameObject currentDinamicParent;
        public string _tag;
        public UnityEvent Input;
        public UnityEvent Output;

        // Start is called before the first frame update
        private void Start()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == dinamicParent.tag)
            {
                _tag = other.tag;
                other.tag = GLV.DinamicParenttag;
                Input.Invoke();
                currentDinamicParent = other.gameObject;

                Debug.Log("Current dinamic parent: " + other.name);
            } 
        }

        public GameObject DinamicParent()
        {
            return currentDinamicParent;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == GLV.DinamicParenttag)
            {
                other.tag = GLV.AssembleDonetag;
                Output.Invoke();
            }
        }
    }
}
