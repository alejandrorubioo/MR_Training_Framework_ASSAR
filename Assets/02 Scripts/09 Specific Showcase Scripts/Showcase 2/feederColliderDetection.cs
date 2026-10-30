using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feederColliderDetection : MonoBehaviour
{

    public GameObject stopper;
    public GameObject Piece;
    private bool spawn;

    // Start is called before the first frame update
    private void Start()
    {
        stopper.gameObject.SetActive(false);
        spawn = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        //In case it detecs a battery
        if (other.gameObject.layer == 25)
        {
            stopper.gameObject.SetActive(true);

            if (spawn)
            {
                other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                Piece.GetComponent<BatteryGenerator>().Spawn();
                spawn = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {

        stopper.gameObject.SetActive(false);
        spawn = true;

    }
}

