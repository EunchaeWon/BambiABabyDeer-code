using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro.Examples;
using UnityEngine;


public class Crow : MonoBehaviour
{
    public MommyDearSc mommyDearSc;
    public BearSc bearSc;
    public Transform LandingSpot;
    public Transform crowPosition;
    public float flyingSpeed = 1.0f;
    public Transform bambiT;
    public Script_CharacterMovement bambiScript;
    public float rotationSpeed = 2f;
    public Animator animator;
    public float maxMagnitudeDelta;
    public float minMagnitudeDelta;
    public Animator BambiAni;
    public GameManager gameManager;
    public CameraFollow cameraFollow;
    public float distanceAhead = 70f;   // Distance in front of the player
    public float heightOffset = 17f;    // Height above the player
    public float speed = 0.5f;           // Speed of forward movement
    public float flightAmplitude = 50f; // Amplitude of the wave pattern
    public float flightFrequency = 10f; // Frequency of the wave pattern
    public float crowGuideDelay = 5;
    public Collider crowCollider;
    public int crowMeeting =-1;
    public int crowMeetingSubtract =0;
    public GameObject bucketGO;
    public float flightTime = 1f;
    public Dictionary<(int index, string character), string> conversationDictionary;
    public bool escapeOn = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        crowMeeting = -1;
        crowCollider = GetComponent<Collider>();
        crowPosition.transform.LookAt(bambiT.position);
        animator = GetComponent<Animator>();
        animator.SetBool("Fly", true);
        crowCollider.enabled = false;
        new WaitForSeconds(gameManager.typingDelayTime*Time.deltaTime);
        GameStartMonologue();
        FirstMeeting();

    }

      // Update is called once per frame
    private void FixedUpdate()
    {


        if (crowMeeting > -1 && crowMeeting < 2)
        {
            FirstMeeting();


        }
        else if (crowMeeting == 7)
        {
            FirstMeeting();

        }
        else if (crowMeeting == 9 && escapeOn == false && gameManager.fireOff >= gameManager.fireSum)
        {

            new WaitForSeconds(crowGuideDelay * Time.deltaTime);

            //crowMeeting++;
            crowCollider.enabled = false;
            FirstMeeting();

        }

    }
    
    public IEnumerator MoveToLandingspot()
    {
        escapeOn = true;
        crowCollider.enabled = false;
        animator.SetBool("Fly", true);
        while (crowPosition.position != LandingSpot.position) //Vector3(-171.15f,16f,196.3f) TREE
        {
            crowPosition.transform.position = Vector3.MoveTowards(crowPosition.position, LandingSpot.position, flyingSpeed * Time.deltaTime);
            crowPosition.transform.LookAt(LandingSpot.position);

            yield return null;
        }
        escapeOn = false;
        crowCollider.enabled = true;
        if (crowMeeting == 4)//bucket
        {
            bucketGO.SetActive(false);
            bucketGO.transform.position = bambiT.position;
            crowMeeting++;////5
            LandingSpot.position = new Vector3(-779.2f, 27f, -1000.65f);
            StartCoroutine(MoveToLandingspot());
        }
        else if (crowMeeting == 5)
        {
            bucketGO.SetActive(true);
            crowMeeting++;//////6
            crowMeetingSubtract = 1;
            conversationDictionary = new Dictionary<(int, string), string>
            {
                {(0, " Ms. Crow") , " Here you are." },
                {(1, "Player") , "Thank you Ms. Crow." },
                {(2, " Ms. Crow") , "You are welcomed." },

            };
            bambiScript.walkingSpeed = 0;
            gameManager.ConversationDataWithoutZoomin(conversationDictionary);
        }
        else if (crowMeeting == 8) /////////////////////////////////////////rabbit = 7
        {
            if (gameManager.fireOff >= gameManager.fireSum)
            {

                crowMeeting++;
            }

        }
        else if (crowMeeting == 9)
        {
            crowMeeting++;
        }
        
    }

  
    public void FirstMeeting()
    {
        Debug.Log("9999");
        Invoke("CrowColliderOn", 10);
        // heightOffset = 10f;
        distanceAhead = 40f;
        crowPosition.transform.LookAt(bambiT);
        flightTime += Time.deltaTime;
        // Calculate the position in front of the player
        Vector3 aheadPosition = bambiT.position + bambiT.transform.forward * distanceAhead;
        aheadPosition.y += heightOffset;

        // Calculate wave pattern for the crow's movement (like a figure-eight)
        float offsetX = Mathf.Sin(flightFrequency * flightTime * 0.1f) * flightAmplitude;
        float offsetY = Mathf.Cos(flightFrequency * flightTime * 0.1f) * flightAmplitude * 0.5f;

        // Set the new position with the wave offset
        Vector3 flightPosition = aheadPosition + bambiT.transform.right * offsetX + Vector3.up * offsetY;

        if (escapeOn == false)
        {

            // Smoothly move the crow to the calculated position
            crowPosition.transform.position = Vector3.Lerp(crowPosition.transform.position, flightPosition, speed * Time.deltaTime);

            // Rotate the crow to face the direction it¡¯s moving
            Vector3 direction = flightPosition - crowPosition.transform.position;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                crowPosition.transform.rotation = Quaternion.Slerp(crowPosition.transform.rotation, targetRotation, speed * Time.deltaTime);
            }

        }


    }

    public void OnTriggerEnter(Collider other)//Conversation and quest start
    {
        if (other.CompareTag("Player"))
            {
            escapeOn = false;
            if (crowMeeting == 0 && gameManager.monologueCanvas.activeSelf == false)//gameManager.Quest == 0)
            {

                LandingSpot.position = new Vector3(-779.2f, 27f, -1000.65f); //TreeTrunk in Lake

                conversationDictionary = new Dictionary<(int, string), string>
            {
                { (0," ??"), " Caw, caw " },
                { (1, "Player") , " Oh,\n hello, Ms. Crow. " },
                { (2, " Ms. Crow") , "Hello, baby dear. \n Why are you here alone?" },
                { (3, "Player") , "Everyone was running, \navoiding from fire,\n and suddenly I became alone..." },
                {(4, "Player"), "Do you know where my mom is?" },
                { (5, " Ms. Crow") , "Oh I am sorry, dear, " },
                {(6, "Mc. Crow"), "because of the smoke from the fire, \nit's still difficult to find someone \n in the sky right now." },
                { (7, " Ms. Crow") , "But when the smoke goes away later,\n I will try to find your mom." },
                { (8, "Player") , "Oh, I see... \nThank you, Ms. Crow." },
                { (9, " Ms. Crow") , "By the way, dear, you look so weary." },
                { (10, "Player") , "Actually I am really hungry and thirsty..." },
                { (11, " Ms. Crow") , "Oh poor thing. \nI will take you to the river first. \n Follow me." },
                { (12, " Ms. Crow") , "Just follow my voice. \nCaw, caw!" },
                { (13, "Player") , "Okay, see you later!" }
            };
                crowMeeting++;////1
                crowMeetingSubtract = 1;
                gameManager.ConversationData(conversationDictionary);

            }
            else if (crowMeeting == 3 && bearSc.meeting == 5)
            {
                conversationDictionary = new Dictionary<(int, string), string>
            {
                { (0," Ms. Crow"), " Caw, caw \n Hello, dear, what are you looking for? " },
                { (1, "Player") , " Hello, Ms. Crow. \n I am looking for a bucket, \nthat I can use to put out fire." },
                { (2, " Ms. Crow") , "Oh, I see. \n Here is a bucket. \n I will bring it to you." },
                { (3, "Player") , "Thank you, Ms. Crow!" },

            };
                animator.SetBool("Fly", true);
                crowMeeting++;////4
                crowMeetingSubtract = 1;
                gameManager.ConversationDataWithoutZoomin(conversationDictionary);

            }
            else if (crowMeeting == 7)
            {
                if (gameManager.fireOff < gameManager.fireSum)
                {
                    conversationDictionary = new Dictionary<(int, string), string>
                {
                    {(0, " Ms. Crow") , "CAW! CAW!" },
                    {(1, "Player") , "Oh, Ms. Crow! What happened?" },
                    {(2, " Ms. Crow"),"I came to let you know that fire is almost extinquished!" },
                    {(3, " Ms. Crow") , "But still it's not enough to make the sky clear." },
                    {(4, " Ms. Crow"),"I will come back as soon as it gets all clear." },

                };
                    crowMeeting++;//8
                    crowMeetingSubtract = 1;
                    gameManager.ConversationDataWithoutZoomin(conversationDictionary);
                }

                else if (gameManager.fireOff >= gameManager.fireSum)
                {
                    conversationDictionary = new Dictionary<(int, string), string>
                {
                {(0, " Ms. Crow") , "CAW! CAW!" },
                {(1, "Player") , "Oh, Ms. Crow! Something happened?" },
                {(2, " Ms. Crow"),"I came to let you know that fire is all extinquished!" },
                {(3, " Ms. Crow") , "The sky is now clear so that I could search for your mom." },
                {(4, " Ms. Crow"),"I will come back as soon as I find your mom." },

                };
                    crowMeeting++;//8
                    
                    crowMeetingSubtract = 1;


                    gameManager.ConversationDataWithoutZoomin(conversationDictionary);
                }


            }
            else if (crowMeeting == 9)
            {
                conversationDictionary = new Dictionary<(int, string), string>
            {
                {(0, " Ms. Crow") , "CAW! CAW!" },
                {(1, " Ms. Crow") , "I found your mom! Head to the Lake!" },
                {(2, " Ms. Crow"),"She seemed to have searched for you." },
                {(3, "Player") , "Thank you, Ms. Crow!" },
                {(4, " Ms. Crow"),"Head to the South west forest!" },

            };
                crowMeeting++;//10
                crowMeetingSubtract = 1;
                mommyDearSc.gameObject.SetActive(true);

                gameManager.ConversationDataWithoutZoomin(conversationDictionary);
            }
        }
    }
    public void FireAllClearQuest()
    {
        
        conversationDictionary = new Dictionary<(int, string), string>
            {
                {(0, "Player") , "I guess I need to cheer up to extinguish more fire in the forest." },
                {(1, "Player") , "It would be helpful for Ms. Crow to find my mom." },
                {(2, "Player"), "And also for our forest." },
               

            };
        gameManager.MonologueData(conversationDictionary);
    }

    public void CrowVoiceGuideMonologue()
    {
        conversationDictionary = new Dictionary<(int, string), string>
        {


                {(0, "Player") , "Why does ms. Crow want to help me?" },
                {(1, "Player") , "Perhaps she wants something from me." },
                {(2, "Player") , "Otherwise why would someone helps the others?" },
                {(3, "Player") , "Everyone is doing for their own good."},


        };

        gameManager.MonologueData(conversationDictionary);
    }
    public void CrowVoiceGuideConversation()////2
    {
        conversationDictionary = new Dictionary<(int, string), string>
        {
            {(0, " Ms. Crow") , "CAW! CAW!\nFollow me!" },
            {(1, "Player") , "I am coming!" },
            {(2, "Pause"),"" },
            {(3, " Ms. Crow") , "CAW! CAW!" },
            {(4, "Pause"),"" },
            {(5, " Ms. Crow") , "CAW! CAW! \nStill there?"},
            {(6, "Pause"),"" },
            {(7, " Ms. Crow") , "CAW! CAW!" },
            {(8, "Player") , "Ms. Crow, \nI wish I had wings." },
            {(9, " Ms. Crow") , "It's not that far. \n Just around a corner." }
        };
        
          gameManager.ConversationDataWithoutZoomin(conversationDictionary);

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && crowMeeting ==2 && LandingSpot.position == crowPosition.position && gameManager.conversationRunning== false && gameManager.monologueCanvas.activeSelf == false)
        {
            crowMeeting++;  ///3
            crowMeetingSubtract = 1;
            conversationDictionary = new Dictionary<(int, string), string>
            {
                {(0, " Ms. Crow") , " Well done." },
                {(1, "Player") , "Thank you Ms. Crow. \nNow I feel better." },
                {(2, " Ms. Crow") , "Good. \nWell, \nif you don't mind, \ncould you do me a favor?" },
                {(3, "Player") , "Sure.\n What is it, Ms. Crow?"},
                {(4, " Ms. Crow") , "Well, \nif you find any fires left, \nI hope you to put them out." },
                {(5, "Player") , "Okay!" }
            };
            
            cameraFollow.LakeLandscape();
            CancelInvoke();
            Invoke("CrowLandingAnim", 1);
            gameManager.ConversationData(conversationDictionary);
        }
        
    }
    private void CrowLandingAnim()
    {
        animator.SetBool("Fly", false);
        crowPosition.transform.LookAt(bambiT);
    }
    public void CrowColliderOn()
    {
        crowCollider.enabled = true;
    }
    public void CrowBucket()
    {
        LandingSpot.position = new Vector3(bucketGO.transform.position.x,transform.position.y, bucketGO.transform.position.z);
        StartCoroutine(MoveToLandingspot());

    }
    void GameStartMonologue()//////////////CrowMeeting=-1
    {
        conversationDictionary = new Dictionary<(int, string), string>
        {

        
                {(0, "Player") , "I am Baby deer." },
                {(1, "Player") , "There was a big forest fire. " },
                {(2, "Player") , "I lost my mom." },
                {(3, "Player") , "I don't know where my mom is..."},
                {(4, "Player") , "I don't really understand how my mom ran away leaving me behind..." },
            {(5, "Player"), "I cannot help to think about what her child or even family means to her..." },
            {(6, "Player"), "I know it's quite unfair to expect only mothers to have such a big maternal instinct... but..." },
            {(7, "Player"), "It's still disappointing to know that my mom does care herself more over their child," },
            {(8, "Palyer"), "when comparing to other mothers around." }

        };

        gameManager.MonologueData(conversationDictionary);
        

    }

}
