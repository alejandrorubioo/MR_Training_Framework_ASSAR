using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MR_Training_ASSAR.InstructionSystem
{
    public class InstructionInterface : MonoBehaviour
    {
        private string jsonString;
        private InstructionData[] textArray;

        public List<InstructionData> GetShowcaseInstructions(int showcaseNumber)
        {
            //It will get the informamtion regarding the JSON file
            jsonString = File.ReadAllText(Application.streamingAssetsPath + "/JSON Files/Instructions " + showcaseNumber + ".json");
            Debug.Log("Instructions " + showcaseNumber + ".json");
            textArray = JsonHelper.FromJson<InstructionData>(jsonString);
            return Array2List(textArray);
        }

        private List<InstructionData> Array2List(InstructionData[] _textArray)
        {
            List<InstructionData> instructionList = new List<InstructionData>();

            //It will change the Array type provided to a List
            for (int i = 0; i < _textArray.Length; i++)
            {
                instructionList.Add(_textArray[i]);
            }
            return instructionList;
        }
    }
}


