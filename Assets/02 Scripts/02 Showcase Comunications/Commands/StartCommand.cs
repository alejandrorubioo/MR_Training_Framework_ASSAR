using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MR_Training_ASSAR.ProcessSystem;

namespace MR_Training_ASSAR.ShowcaseCommunications
{
    public class StartCommand : Command
    {
        private ObjectInterface _object;

        public StartCommand(ObjectInterface Object)
        {
            _object = Object;
        }

        public override void Execute()
        {
            _object.StartCommand();
        }
    }
}


