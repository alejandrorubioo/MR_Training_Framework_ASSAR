using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MR_Training_ASSAR.ProcessSystem;

namespace MR_Training_ASSAR.ShowcaseCommunications
{
    public class VirtualityCommand : Command
    {
        private ObjectInterface _object;
        private bool _real;

        public VirtualityCommand(ObjectInterface Object, bool real)
        {
            _object = Object;
            _real = real;
        }

        public override void Execute()
        {
            _object.ToggleVirtuality(_real);
        }
    }
}