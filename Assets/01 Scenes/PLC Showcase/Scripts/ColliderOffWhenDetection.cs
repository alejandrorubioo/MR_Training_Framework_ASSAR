using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderOffWhenDetection : MonoBehaviour
{
    public Collider sensor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider == sensor)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
