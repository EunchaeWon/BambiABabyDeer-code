using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearColliderSc : MonoBehaviour
{
    public BearSc bearSc;
    public GameManager gameManager;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))// && gameManager.monologueCanvas.activeSelf == false)
        { 
            bearSc.OnBearCollisionEnter(); 
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.monologueCanvas.activeSelf == false)
        { 
            bearSc.OnBearCollisionStay(other); 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.monologueCanvas.activeSelf == false)
        { 
            bearSc.OnBearCollisionExit(other); 
        }
    }
}
