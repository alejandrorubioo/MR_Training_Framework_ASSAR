using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR_Training_ASSAR.ProcessSystem
{
    public class AttachToInvisible : MonoBehaviour
    {
        // Start is called before the first frame update
        public List<GameObject> invisibleList = new List<GameObject>();
        public List<GameObject> ReferenceList = new List<GameObject>();

        private void Start()
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Invisible"))
            {
                invisibleList.Add(obj);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == GLV.BottomTag)
            {
                foreach (Transform child in other.transform.Find("BottomReference"))
                {
                    ReferenceList.Add(child.gameObject);
                }
            }
        }

        public void ObtainReference()
        {
            if (invisibleList.Count == ReferenceList.Count)
            {
                foreach (GameObject obj in invisibleList)
                {
                    foreach (GameObject Obj in ReferenceList)
                    {
                        if (obj.name.Contains(Obj.name))
                        {
                            obj.transform.position = Obj.transform.position;
                            obj.transform.rotation = Obj.transform.rotation;
                        }
                    }
                }
            }
        }
    }
}

