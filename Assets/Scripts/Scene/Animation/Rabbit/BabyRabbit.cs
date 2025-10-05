using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyRabbit : MonoBehaviour
{
    public Animator anim;
    public BabyRabbitAnimator bRAnimatorscript;

    // Start is called before the first frame update
    void Start()
    {

        anim.SetBool("start", true);
        //bRAnimatorscript.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
