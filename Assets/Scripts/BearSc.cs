using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BearSc : MonoBehaviour
{
    public float limitDistance = 1f;
    public Crow crowSc;
    public BucketSc bucketSc;
    public Script_CharacterMovement bambiSc;
    public Animator animator;
    public Transform bambiGO;
    public Dictionary<(int index, string character), string> conversationDictionary;
    public GameManager gameManager;
    public float meeting;
    public float bearAnimDelay;
    public float bearRunningSpeed;
    public CharacterController characterController;
    private Vector3 runningSpot;
    private bool isAnimating;
    public float meetingAdding;
    public int meetingSubtract =0;
    private float distanceX = 0;
    private float distanceZ = 0;
    public BigTreeSc bigTreeSc;
    public Transform bigTreeT;
    public FoxSc foxSc;
    public FireSc bigFire1;
    public FireSc bigFire2;
    public FireSc bigFire3;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", false);
        animator.ResetTrigger("Standing");
        characterController = GetComponent<CharacterController>();
        

    }
    private void FixedUpdate()
    {
        if(bigTreeSc.meeting==1 && meeting == 2)
        {
            animator.SetBool("isWalking", true);

        }


    }

    public void OnBearCollisionEnter()
    {

        if (meeting == 0)
        {
            // animator.SetTrigger("Standing");

            conversationDictionary = new Dictionary<(int, string), string>
            {
                { (0," ??"), "" },
                { (1, "Player") , " Oh it's a bear.\n Hello!" },
                { (2," ??"), "Grrrrr..." },
                { (3,"Player"), "Oh, I gotta run...!" },

            };
            gameManager.ConversationData(conversationDictionary);
            meeting++;///////////////1
            meetingSubtract = 1;
        }
        else if (meeting == 1 || meeting == 3)
        {
            if (isAnimating == false)
            {
                ChasingBambi();
                isAnimating = true;
            }
        }

        else if (meeting == 2)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);

            conversationDictionary = new Dictionary<(int, string), string>
            {
                { (0,"Player"), "Hmmmm...." },
                { (1, "Player") , "Oh wait, I have a good idea." },
                { (2, "Player"), "There is a honey hive on the top of the big tree." },
                {(3, "Player"), "As soon as the bear sees the honey, he will stop chasing us\n and help putting out the fires on the big tree. " },
                {(4, "Player"), "The bear is tall and even able to climb the tree. He will be helpful." },
            };

            //gameManager.secondCameraOn = true;
            gameManager.ConversationData(conversationDictionary);
            meeting++;
            meetingSubtract = 1;
        }
        else if (meeting == 4)
        {
            conversationDictionary = new Dictionary<(int, string), string>
            {
                { (0,"Player"), "Look, Mr. Bear, there is a honey that you like!" },
                { (1, " Mr. Bear") , "Honey...?" },
                { (2, "Player"), "Yes, honey! Oh the top of the big tree." },
                { (3, " Mr. Bear"), " Oh!!! I want to eat that! But how? \n The fire is going to burn my hoeny!! " },
                { (4, "Player"), "Well, if you help me to put out the fire,\nI will let you know how! \n So stop being mean to me!" },
                { (5," Mr. Bear"), "I am sorry, it was just a small tantrum \n maybe because of my low blood sugar. \n I need to eat honey right now! \n How can I help you?" },
                { (6, "Player"), "Oh, small tantrum.... \n Okay, I take your apology. \n Just wait here for a bit please. \nI will bring a bucket full of water." },
                { (7, " Mr. Bear"), "A bucket? Do you have it?" },
                { (8, "Player"), "I think I saw a bucket floating the river." },
                 {(9, "Player"), "So, sometimes humans are helpful." },
                {(10, " Mr. Bear"), "Haha, that's a funny joke. \n Well, good luck." },
                { (11, " Mr. Bear"), "I will wait here." }

            };
            bigTreeSc.meeting++;
            bigTreeSc.meetingSubtract = 1;
            //gameManager.secondCameraOn = true;
            gameManager.ConversationData(conversationDictionary);
            meeting++;
            meetingSubtract = 1;
            bucketSc.gameObject.SetActive(true);
            gameManager.extinctFireText.text = "extinct fire : " + gameManager.fireOff + "\n" + "Let's bring a bucket full of water.";
        }
        else if (meeting == 5 && crowSc.crowMeeting ==6)
        {
            conversationDictionary = new Dictionary<(int, string), string>
            {
                { (0,"Player"), "Mr. Bear, I brought water!" },
                { (1, " Mr. Bear") , "Oh great! Let me handle this." },
                

            };
            bambiSc.walkingSpeed = 0;
            meeting++;/////6
            gameManager.ConversationDataWithoutZoomin(conversationDictionary); //////7
            gameManager.extinctFireText.text = "extinct fire : " + gameManager.fireOff + "\n" + "";

        }
    }
    public void OnBearCollisionStay(Collider other)
    {

        if (isAnimating == false)
        {
            if (meeting == 1 || meeting == 3)
            {

                if (distanceX > 0.1f || distanceZ > 0.1f)
                {
                    
                    ChasingBambi();

                    isAnimating = true;
                    if(meeting == 3)
                    {
                        CancelInvoke();
                    }
                    
                }


            }

        }


        
    }
    public void OnBearCollisionExit(Collider other)
    {
        
        if (meeting < 4 && isAnimating == true)
        {
            Invoke(nameof(StopChasing), bearAnimDelay * Time.deltaTime);
            if(meeting == 3)
            {
                
                CancelInvoke();
                ChasingBambi();
                isAnimating = false;
            }
            
        }
    }
    
    public void ChasingBambi()
    {

        animator.SetBool("isWalking", true);
        Vector3 directionToTarget = transform.position - bambiGO.position;
        directionToTarget.y = 0;
        Vector3 oppositeDirection = -directionToTarget;
        Quaternion bearRotation = Quaternion.LookRotation(oppositeDirection, Vector3.up).normalized;
        transform.rotation = bearRotation;
        runningSpot = bambiGO.position;
        //new WaitForSeconds(bearAnimDelay*Time.deltaTime);

            StartCoroutine(RunningTowardBambi());


    }
        
     
    
    IEnumerator RunningTowardBambi()
    {
      //  meeting = meeting + meetingAdding;/////////////////////////////////////////////////////////////??
    //    meetingAdding = 0;
        animator.SetBool("isRunning", true);
        animator.SetBool("isWalking", false);
        distanceX = Mathf.Abs(transform.position.x - runningSpot.x);
        distanceZ = Mathf.Abs(transform.position.z - runningSpot.z);
        
        while (distanceX > limitDistance && distanceZ > limitDistance)
        {


            float gravity =0;

            if (!characterController.isGrounded)
            {
                gravity = -1;
            }
            else
            {
                gravity = 0;
            }

            characterController.Move(new Vector3(0, gravity, 0) * Time.deltaTime * 9.81f);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(runningSpot.x, 0, runningSpot.z), bearRunningSpeed * Time.deltaTime);

            distanceX = Mathf.Abs(transform.position.x - runningSpot.x);
            distanceZ = Mathf.Abs(transform.position.z - runningSpot.z);

            //characterController.Move(moveTowards * bearRunningSpeed); 
            // characterController.transform.position = Vector3.forward * Time.deltaTime * bearRunningSpeed;

            //transform.position = transform.forward * bearRunningSpeed*Time.deltaTime;
            yield return null;
        }
        animator.SetBool("isRunning", false);
        StopChasing();

    }
    public void StopChasing() //gameManager
    {
        animator.SetTrigger("Standing");
        animator.SetBool("isRunning", false);
        if (meeting < 3)
        {
            new WaitForSeconds(bearAnimDelay * Time.deltaTime);
        }
        isAnimating = false;
        distanceX = Mathf.Abs(transform.position.x - runningSpot.x);
        distanceZ = Mathf.Abs(transform.position.z - runningSpot.z);
        float treeDistanceX = Mathf.Abs(transform.position.x - bigTreeT.position.x);
        float treeDistanceZ = Mathf.Abs(transform.position.z - bigTreeT.position.z);

        if (treeDistanceX < 100 && treeDistanceZ < 100 && meeting == 3)
        {
            meeting++;////////////4
            
            OnBearCollisionEnter();
            
        }
    }

    public void BearCaveEnter()
    {
        if (meeting == 0)
        {
            conversationDictionary = new Dictionary<(int, string), string>
            {
                { (0,"Player"), "Wow, a huge cave!" },
                { (1, "Player") , "We are in the north east forest." },
                { (2, "Player"), "But no body shows up." },
                { (3, "Player"), "Maybe we should search for someone around." },


            };
            gameManager.extinctFireText.text = "extinct fire : " + gameManager.fireOff + "\n" + "Let's check if someone is around.";
            gameManager.MonologueData(conversationDictionary);

        }
        else if (meeting < 3 && meeting!=0)
        {
            conversationDictionary = new Dictionary<(int, string), string>
            {
                { (0,"Player"), "This must be the bear's home." },
                { (1, "Player") , "It seems like baby rabbit's family nor my mom are not here in north east forest." },
                { (2, "Player"), "Maybe we have to go to the south east forest" }

            };
            gameManager.extinctFireText.text = "extinct fire : " + gameManager.fireOff + "\n" + "Let's check the south east forest.";
            gameManager.MonologueData(conversationDictionary);


        }


    }
    public void PouringWater()
    {
        Vector3 directionToTarget = bigTreeT.position;
        directionToTarget.y = 0;

        Quaternion turnAroundRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        transform.rotation = turnAroundRotation;
        bucketSc.PouringWaterAnim();
        animator.SetTrigger("Standing");
        new WaitForSeconds(bearAnimDelay*Time.deltaTime);
        bigFire1.BigFireOut();
        bigFire2.BigFireOut();
        bigFire3.BigFireOut();

        Invoke(nameof(AfterFireConversation), (bearAnimDelay + gameManager.fireOffDelay) * Time.deltaTime);
    }
    void AfterFireConversation()
    {

        
        conversationDictionary = new Dictionary<(int, string), string>
            {
                { (0,"Player"), "Wow, we saved the big tree!" },
                { (1, " Mr. Bear") , "And my honey too!" },
                { (2, "Player"), "Thank you for your help, Mr. Bear" },

            };
        //////7
        gameManager.ConversationDataWithoutZoomin(conversationDictionary);
       // bucketSc.gameObject.SetActive(false);

    }
    public void AfterByeBearMonologue()
    {   
        bucketSc.gameObject.SetActive(false);
        new WaitForSeconds(bearAnimDelay * Time.deltaTime);
        conversationDictionary = new Dictionary<(int, string), string>
            {
                { (0,"Player"), "Oh, he's not listening to eat the honey." },
                { (1, "Player") , "The bees had all evacuated at least when I found the hive." },
                { (2, "Player"), "Wait, I can see someone is coming." },

            };
        //////8
        foxSc.AfterBigFireOffMeeting();
        gameManager.MonologueData(conversationDictionary);
    }


}
