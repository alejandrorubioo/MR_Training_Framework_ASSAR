using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MR_Training_ASSAR.ProcessSystem;

namespace MR_Training_ASSAR.ShowcaseCommunications
{
    public class NewProcessCommand : Command
    {
        private ObjectInterface _object;
        private int _index;

        public NewProcessCommand(ObjectInterface obj, int index)
        {
            _object = obj;
            _index = index;
        }

        public override void Execute()
        {
            _object.ResetShowcase(_index);
        }
    }

}