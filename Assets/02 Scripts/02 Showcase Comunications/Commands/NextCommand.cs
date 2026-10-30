using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MR_Training_ASSAR.ProcessSystem;

namespace MR_Training_ASSAR.ShowcaseCommunications
{
    public class NextCommand : Command
    {
        private ObjectInterface _object;

        public NextCommand(ObjectInterface Object)
        {
            _object = Object;
        }

        public override void Execute()
        {
            _object.NextCommand();
        }

    }

}

