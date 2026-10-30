using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR_Training_ASSAR.InstructionSystem
{
    [System.Serializable]
    public class InstructionData
    {
        public int tag;
        public int step;
        public string infoContent;

        //Constructor empty
        public InstructionData()
        {
            tag = 0;
            step = 0;
            infoContent = "";
        }

        //Constructor
        public InstructionData(int _tag, int _step, string _infoContent)
        {
            tag = _tag;
            step = _step;
            infoContent = _infoContent;
        }
    }

    //This class is needed for write arrays in JSON
    public static class JsonHelper//Json Class helper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }
        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }
        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }
        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }

}