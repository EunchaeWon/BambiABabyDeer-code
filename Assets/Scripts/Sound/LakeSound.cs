using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakeSound : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player or main character enters the area
        if (other.CompareTag("Player")) // Ensure your player is tagged "Player"
        {
            audioSource.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Stop the audio when the player leaves the area
        if (other.CompareTag("Player"))
        {
            audioSource.Stop();
        }
    }
}
