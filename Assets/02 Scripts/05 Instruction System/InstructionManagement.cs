using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MR_Training_ASSAR.GameManagement;
using UnityEngine.SceneManagement;

namespace MR_Training_ASSAR.InstructionSystem
{
    public class InstructionManagement : MonoBehaviour
    {
        #region variables
        public GameObject TextCanvas;
        private TextMeshPro _text;

        private InstructionInterface _instInterface;

        private string instructionText;
        private int _index = 0;
        private int _showcaseNumber;
        private List<InstructionData> _instructionList;
        #endregion

        private void Awake()
        {
            _text = TextCanvas.GetComponent<TextMeshPro>();
            _instInterface = gameObject.GetComponent<InstructionInterface>();

        }

        //Because of problems with the JSON, the list must be done in the start and  not in the awake
        private void Start()
        {

            _instructionList = _instInterface.GetShowcaseInstructions(SceneManager.GetActiveScene().buildIndex);
            ChangeInstruction();
        }

        private void ChangeInstruction()
        {
            if (_index < 0 )
            {
                _index = 0;
            }
            else if (_index > _instructionList.Count)
            {
                _index = _instructionList.Count - 1;
            }
            
            instructionText = _instructionList[_index].infoContent;
            _text.SetText(instructionText);
        }

        public void Next()
        {
            _index++;
            ChangeInstruction();
        }

        public void Reset2FirstInstruction()
        {
            _index = 0;
            ChangeInstruction();
        }

        public void Back()
        {
            _index--;
            ChangeInstruction();
        }
    }
}


