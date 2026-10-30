using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MR_Training_ASSAR.ProcessSystem;

namespace MR_Training_ASSAR.ShowcaseCommunications
{
    public class FinishedCommand : Command
    {
        private Showcase_Manager _scManager;
        private GameObject _scManagerGO;

        private GameObject _object;
        private bool _finished;

        public FinishedCommand(GameObject obj, bool finished)
        {
            _scManagerGO = GameObject.Find(GLV.ShowcaseManagerName);
            _scManager = _scManagerGO.GetComponent<Showcase_Manager>();

            _object = obj;
            _finished = finished;
        }

        public override void Execute()
        {
            _scManager.taskFinish(_object, _finished);
        }

    }
}


