using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AnimateInRange : MonoBehaviour
{

    [SerializeField] Animator AC; // 

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
            AC.SetBool("IsInRange", true);  // Once the Player Enters the Range of the object, IsInRange = true
    }

    private void OnTriggerExit(Collider other) { 
        if(other.CompareTag("Player"))
            AC.SetBool("IsInRange", false); // Once we exit, Range is false
    }

}
