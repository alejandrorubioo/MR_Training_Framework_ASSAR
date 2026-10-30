using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR_Training_ASSAR.ProcessSystem
{ 
    public class FinalEvent : MonoBehaviour
    {
        GameObject LeftFirework;
        GameObject RightFirework;
        GameObject Ballons;
        public GameObject Menu_End;

        private void Start()
        {
            LeftFirework = gameObject.transform.GetChild(0).gameObject;
            RightFirework = gameObject.transform.GetChild(1).gameObject;
            Ballons = gameObject.transform.GetChild(2).gameObject;
            _FinalEvent(false);

        }

        public void _FinalEvent(bool enable)
        {
            Menu_End.SetActive(true);

            foreach(Transform Child in LeftFirework.transform)
            {
                Child.gameObject.SetActive(enable);
            }
            foreach (Transform Child in RightFirework.transform)
            {
                Child.gameObject.SetActive(enable);
            }
            foreach (Transform Child in Ballons.transform)
            {
                Child.gameObject.SetActive(enable);
            }
        }
    }
}
