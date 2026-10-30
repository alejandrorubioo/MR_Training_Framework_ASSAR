using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR_Training_ASSAR.GameManagement
{
    public class ShowcaseAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 finalPosition;
        private Vector3 initialPosition;

        private void Awake()
        {
            initialPosition = transform.position;
        }

        private void OnDisable()
        {
            transform.position = initialPosition;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector3.Lerp(transform.position, finalPosition, 0.1f);
        }
    }
}