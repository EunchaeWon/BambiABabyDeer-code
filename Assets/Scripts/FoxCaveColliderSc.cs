using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxCaveColliderSc : MonoBehaviour
{
    public FoxSc foxSc;
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)//Conversation and quest start
    {

        //Debug.Log("Entered the collider zone");
        if (other.CompareTag("Player"))
        {

            foxSc.FoxCaveColliderEnter();
        }
    }

    // Update is called once per frame

}
