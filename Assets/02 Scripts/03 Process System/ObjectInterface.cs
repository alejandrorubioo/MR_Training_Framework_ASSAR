using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

namespace MR_Training_ASSAR.ProcessSystem
{
    public abstract class ObjectInterface : MonoBehaviour
    {
        public virtual void StartCommand()
        {
            // Enable Object for this step
        }

        public virtual void NextCommand()
        {
            // Change parentship to main WorkObject
        }

        public virtual void ResetCommand()
        {
            // Reset intial step pos
        }

        public virtual void BackCommand()
        {
            // Reset to previous step pos
        }

        public virtual void SkipCommand()
        {
            // Change pos to next
        }

        public virtual void TaskFinished()
        {

        }

        public virtual void ResetShowcase(int index)
        {

        }

        public virtual void ToggleVirtuality(bool real)
        {
            
        }
    }
}
