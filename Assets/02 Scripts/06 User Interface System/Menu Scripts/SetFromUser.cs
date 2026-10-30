using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFromUser : MonoBehaviour
{
    public GameObject CorrectPosition;
    public float Offset_x;
    public float Offset_y;
    public float Offset_z;
    public Vector3 rotacion;

    public void Set()
    {
        //gameObject.transform.position = new Vector3(Camera.transform.position.x + Offset_x, Camera.transform.position.y + Offset_y, Camera.transform.position.z + Offset_z);
        //gameObject.transform.eulerAngles = new Vector3(Camera.transform.eulerAngles.x + rotacion.x, Camera.transform.eulerAngles.y + rotacion.y, 0 + rotacion.z);

        gameObject.SetActive(true);

        gameObject.transform.position = CorrectPosition.transform.position;
        gameObject.transform.eulerAngles = new Vector3(CorrectPosition.transform.rotation.x, rotacion.y, rotacion.z);
    }

    public void UnSet()
    {
        gameObject.SetActive(false);
    }
}
