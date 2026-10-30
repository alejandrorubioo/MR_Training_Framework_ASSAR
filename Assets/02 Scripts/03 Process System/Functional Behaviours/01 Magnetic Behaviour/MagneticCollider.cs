using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR_Training_ASSAR.ProcessSystem
{
    public class MagneticCollider : MonoBehaviour
    {
        public GameObject behaviourGameObject { get; set; }
        public bool dynamicParent;
        public bool assemblyDone;
        MagneticBehaviour _magnetic;
        GameObject _object;
        
        public void Initialize()
        {
            _magnetic = behaviourGameObject.GetComponent<MagneticBehaviour>();
            _object = _magnetic.TypeofObject;
           
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag==_object.tag && _magnetic.enabled)
            {
                _magnetic.PullTrigger(other.gameObject);
            }
            if (dynamicParent && other.gameObject.tag == GLV.DinamicParenttag && _magnetic.enabled)
            {
                _magnetic.PullTrigger(other.gameObject);
            }
            if(dynamicParent && other.gameObject.tag == GLV.AssembleDonetag && _magnetic.enabled)
            {
                _magnetic.PullTrigger(other.gameObject);
            }
        }
    }
}