using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR_Training_ASSAR.ShowcaseCommunications
{
    public class Invoker : MonoBehaviour
    {
        public void ExecuteCommand(Command command)
        {
            command.Execute();
        }
    }
}


