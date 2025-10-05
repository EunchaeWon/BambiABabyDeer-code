using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static Unity.VisualScripting.Round<TInput, TOutput>;

public class BucketSc : MonoBehaviour
{
    public BigTreeSc bigTreeSc;
    public Crow crowSc;
    public Transform bambiT;
    public Transform bearT;
    public BearSc bearSc;
    public Animator animator;
    public Transform bigTreeT;
    public AudioSource audioSource;
    public AudioClip fillingWaterAudio;
    public AudioClip waterSplashFireExtinguishingAudio;
    private bool soundOn;
    public float animDelay=160f;
    //private int meeting = 0;
    // Start is called before the first frame update

    private void Start()
    {
      //  gameObject.SetActive(false);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.Pause();
        soundOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (crowSc.crowMeeting == 6 && bearSc.meeting == 5)
        {

            transform.position = bambiT.position;
            if(soundOn == false)
            {
                soundOn = true;
                audioSource.clip = fillingWaterAudio;
                audioSource.volume = 0.3f;
                audioSource.Play();
            }
        }

    }
    public void PouringWaterAnim()
    {
        transform.localPosition = new Vector3(bearT.localPosition.x, bearT.localPosition.y + 70f, bearT.localPosition.z);
        Vector3 directionToTarget = -bigTreeT.position;
        directionToTarget.y = 0;

        Quaternion turnAroundRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        transform.rotation = turnAroundRotation;
        new WaitForSeconds(2f*Time.deltaTime);
        animator.SetTrigger("Pouring");
        new WaitForSeconds(animDelay* Time.deltaTime);
        Invoke("SplashSound", animDelay * Time.deltaTime);

      
    }
    void SplashSound()
    {
        bigTreeSc.audioSource.Stop();
        audioSource.clip = waterSplashFireExtinguishingAudio;
        audioSource.Play();
    }


}
