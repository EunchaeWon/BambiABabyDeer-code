using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxSc : MonoBehaviour
{
    public FoxCaveColliderSc foxCaveColliderSc;
    public Animator animator;
    public BabyRabbitAnimator rabbitSc;
    public Transform bambiGO;
    public Transform rabitGO;
    public Dictionary<(int index, string character), string> conversationDictionary;
    public GameManager gameManager;
    public int meeting =0;
    public int meetingSubtract = 0;
    public float limitDistance = 20;
    public float runningSpeed = 200;
    public Script_CharacterMovement bambiSc;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        animator = GetComponent<Animator>();
       

    }
    public void FoxAppear()
    {
        gameObject.SetActive(true);        
        animator.SetBool("isWalking", false);
        animator.SetBool("isSitting", true);
    }
    public void FoxCaveColliderEnter()
    {
        if (meeting == 0 && rabbitSc.rabbitMeeting == 3)//  && gameManager.monologueCanvas.activeSelf == false)//gameManager.Quest == 0)
        {

            animator.SetBool("isSitting", false);
            conversationDictionary = new Dictionary<(int, string), string>
            {
                { (0,"Player"), " Come on, baby rabbit! It's a cave! Isn't that your home? " },
                { (1, " Baby Rabbit") , "Yes! Mom! Dad! Are you there?" },
                { (2, " ??") , "Hello, baby dear, \n and baby rabbit." },
                { (3, "Player") , "Oh, it's you, Mr. Fox" },
                { (4, " Mr. Fox") , "I am afraid that the rabbit family is not here now." },
                { (5, "Player") , "Do you know where they are?" },
                { (6, " Mr. Fox") , "Hmm, well, I would suggest to go to the east." },
                { (7, " Mr. Fox") , "Eastern forest was less damaged,\n so maybe they are staying there \n waiting the other members of their family." },
                { (8, "Player") , "Thank you, Mr. Fox." },
                { (9, " Baby Rabbit") , "Thank you." }

            };
            meeting++;////1
            meetingSubtract = 1;
            rabbitSc.rabbitMeeting++;//4
            rabbitSc.rabbitMeetingSubtract = 1;
            gameManager.ConversationData(conversationDictionary);
            
            //sphereCollider.enabled = true;

        }
    }
    public void MonologueAfterFox()
    {
        conversationDictionary = new Dictionary<(int, string), string>
            {
                { (0,"Player"), "It looks like the baby rabbit got depressed a bit." },
                { (1, "Player") , "But at least it is a great information to know where to go." },
                { (2, "Player"), "When all fire extinguishes, everyone will come back to their home in the end." },
                { (3, "Player") , "We can check both north east forest and south east forest." },
                { (4, "Player") , "Let's check the north east forest, first." },


            };

        gameManager.MonologueData(conversationDictionary);
        
    }

    public void AfterBigFireOffMeeting()//////////////bearMeeting==8
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isSitting", false);
        
        StartCoroutine(ComingTowardsBigTree());
        
    }
   
    IEnumerator ComingTowardsBigTree()
    {
        Vector3 targetPosition = bambiGO.transform.position;
        float DistanceX = MathF.Abs(transform.position.x - targetPosition.x);
        float DistanceZ = MathF.Abs(transform.position.z - targetPosition.z);
        while (DistanceX > limitDistance && DistanceZ >limitDistance) //Vector3(-171.15f,16f,196.3f) TREE
        {
            transform.position = Vector3.MoveTowards(transform.position, bambiGO.transform.position, runningSpeed * Time.deltaTime);
            transform.LookAt(bambiGO.transform.position);
            
            targetPosition = bambiGO.transform.position;
            DistanceX = MathF.Abs(transform.position.x - targetPosition.x);
            DistanceZ = MathF.Abs(transform.position.z - targetPosition.z);
            
            yield return null;
        }
        bambiSc.walkingSpeed *= 0.5f;
        RabbitFamilyMessengerFox();
        animator.SetBool("isWalking", false);
        animator.SetBool("isSitting", false);
        

    }

    private void RabbitFamilyMessengerFox()
    {
        conversationDictionary = new Dictionary<(int, string), string>
            {
                { (0," Mr. Fox"), "Hello, I have a good news." },
                { (1, "Player") , "Hello, Mr. Fox. What is it?" },
                { (2, " Mr. Fox") , "Well, I saw a rabbit family come back to home." },
                { (3, "Player") , "Oh, that's great! Thank you for the information!" },
                { (4, " Mr. Fox") , "Don't mention it. \n You saved our oldest tree. " },
                { (5, "Player") , "Hey, baby rabbit, let's go back to north west forest!" }

            };
        meeting++;//2
        meetingSubtract = 1;
        gameManager.ConversationDataWithoutZoomin(conversationDictionary);
        
    }
}
