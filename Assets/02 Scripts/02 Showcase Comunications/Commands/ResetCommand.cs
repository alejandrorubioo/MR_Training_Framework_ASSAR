using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MR_Training_ASSAR.ProcessSystem;

namespace MR_Training_ASSAR.ShowcaseCommunications
{
    public class ResetCommand : Command
    {
        private ObjectInterface _object;

        public ResetCommand(ObjectInterface Object)
        {
            _object = Object;
        }

        public override void Execute()
        {
            _object.ResetCommand();
        }
    }
}

