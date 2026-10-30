using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR_Training_ASSAR
{
    public static class ExtensionMethods
    {
        public static bool HasComponent<T>(this GameObject obj) where T : Component
        {
            return obj.GetComponent<T>() != null;
        }

        public static void MustHaveComponent<T>(this GameObject obj) where T : Component
        {
            if (!obj.HasComponent<T>())
            {
                obj.AddComponent<T>();
            }
        }

        public static void MustHaveGameObject(GameObject obj) //Not yet Implemented
        {
            if (obj == null)
            {
                obj = GameObject.Find(obj.name);
            }
        }

        public static List<GameObject> GetChildrenWithTag(this GameObject obj, string _tag)
        {
            List<GameObject> ChildrenListWithTag = new List<GameObject>();

            foreach (Transform child in obj.transform)
            {
                if (child.gameObject.tag == _tag)
                {
                    ChildrenListWithTag.Add(child.gameObject);
                }
            }

            return ChildrenListWithTag;
        }

    }
}