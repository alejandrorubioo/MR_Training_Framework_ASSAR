using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MR_Training_ASSAR.ShowcaseCommunications;

public class SC3_HMIOutputPLC : MonoBehaviour
{
    public game4automation.PLCOutputBool HMIOutput;
    public bool isAssemblyDoneButton = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
        if (isAssemblyDoneButton)
        {
            StartCoroutine(WaitForAssemblyDone());
        }
        else
        {
            StartCoroutine(WaitForTrueSignal());
        }
            
    }

    IEnumerator WaitForAssemblyDone()
    {
        yield return new WaitUntil(() => HMIOutput.Value == true);  //Aqui se tendria que poner algo para esperar al ultimo step
            foreach (Transform child in gameObject.transform)
            child.gameObject.SetActive(true);

        StartCoroutine(WaitForFalseSignal());
        yield break;
    }
    IEnumerator WaitForTrueSignal()
    {
        yield return new WaitUntil(() => HMIOutput.Value == true);
        foreach (Transform child in gameObject.transform)
            child.gameObject.SetActive(true);

        StartCoroutine(WaitForFalseSignal());
        yield break;
    }

    IEnumerator WaitForFalseSignal()
    {
        yield return new WaitUntil(() => HMIOutput.Value == false);
        foreach (Transform child in gameObject.transform)
            child.gameObject.SetActive(false);

        if (isAssemblyDoneButton)
        {
            StartCoroutine(WaitForAssemblyDone());
        }
        else
        {
            StartCoroutine(WaitForTrueSignal());
        }
    yield break;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
