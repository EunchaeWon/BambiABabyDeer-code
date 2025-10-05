using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
  //  public List<Conversation> conversationLines = new List<Conversation>();
    public int fireSum = 14;
    public GameObject extinctFireCanvas;
    public TMPro.TextMeshProUGUI extinctFireText;
    public GameObject conversationCanvas;
    public GameObject monologueCanvas;   // Assign your panel here
    public TMPro.TextMeshProUGUI monologueText;
    public TMPro.TextMeshProUGUI npcText;
    public TMPro.TextMeshProUGUI playerText;// Assign your Text component here
    public Button continueButton;      // Optional button to proceed in dialogue
    public float Quest = 0;
    public Camera mainCamera;
    public CameraFollow cameraFollow;
    public Script_CharacterMovement bambiSc;
    public BabyRabbitAnimator babyRabbitSc;
    public Crow crow;
    public Transform bearT;
    public Transform crowT;
    public Transform foxT;
    public Transform bambiT;
    public GameObject babyRabbitGO;
    public float distance = 2000;
    public float distanceWithoutZoomin = 10000;
    public GameObject rabbitFamily;
    public float screenLimitX=500;
    public float screenLimitY=150;
    public FireSc fireSc;
    public BearSc bearSc;
    public BigTreeSc bigTreeSc;
    public FoxSc foxSc;
    public Transform bigTreeT;
    private Dictionary<(int Cindex, string Ccharacter), string> conversationDictionary;
    private Dictionary<(int Mindex, string Mcharacter), string> monologueDictionary;
    public BucketSc bucketSc;
    public int currentConversationIndex = 0;
    private int maximumConversationIndex;
    private int maximumMonologueIndex;

    private int currentMonologueIndex = 0;
    private string monologueTypingText;
    private string conversationTypingText;
    private float typingDelay;
    public float typingDelayTime = 4f;
    private int intM=0;
    private int intC=0;

    public float monologuePauseTime = 5;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 targetDir;

    private bool coroutineRunning = false;

    public bool conversationRunning = false;
    public bool conversationWOZoominRunning = false;
    public float fireOffDelay = 2;
    public int fireFound = 0;
    public int fireOff = 0;
    public bool fireOn = false;
    public bool secondCameraOn = false;
    private string conversationKey;
    private string monologueKey;
    public string questText;
    public float playerTextHeightMultiply = 0.3f;
    private bool afterMonologueRunning = false;
    private bool afterMonologueRunningWithoutZoomin = false;
    public BambiAnimator bambiAnimator;
    void Start()
    {
        extinctFireCanvas.SetActive(false);
        conversationCanvas.SetActive(false);
        monologueCanvas.SetActive(false);  // Hide the dialogue UI initially
        continueButton.onClick.AddListener(NextMonologueLine);  // Set up the button click
       
        rabbitFamily.SetActive(false);
        Debug.Log(bambiAnimator.bambiAnimator.speed);

    }
    private void FixedUpdate()
    {

        if (bambiSc.walkingSpeed == 0 && Input.GetKeyDown(KeyCode.Escape)) 
        { 
            crow.escapeOn = true;
            bambiSc.walkingSpeed = 50;
            cameraFollow.CameraZoomOutPosition();

            conversationCanvas.SetActive(false);
            monologueCanvas.SetActive(false);  // Hide the dialogue UI initially
            babyRabbitSc.rabbitMeeting -= babyRabbitSc.rabbitMeetingSubtract;
            crow.crowMeeting -= crow.crowMeetingSubtract;
            bearSc.meeting -= bearSc.meetingSubtract;
            bigTreeSc.meeting -= bigTreeSc.meetingSubtract;

            MeetingSubtractNormalize();
        }
        if (conversationCanvas.activeSelf && !secondCameraOn && cameraFollow.lakeCamera.enabled == false && crow.crowMeeting == 2)
        {
            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(crowT.position);

            if ( viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
            {
                Vector3 distanceBambiCrow = crowT.position - bambiT.position;
                Debug.Log("distance" + distanceBambiCrow);

                if (Mathf.Abs(distanceBambiCrow.z) > Mathf.Abs(distanceBambiCrow.x))
                {
                    if (distanceBambiCrow.x > 500)
                    {
                        distanceBambiCrow.x = 500;
                    }
                    else if (distanceBambiCrow.x < -500)
                    {
                        distanceBambiCrow.x = -500;
                    }
                    else
                    {
                        // Normalize only if within bounds [-500, 500] and map to [0.05, 0.95]
                        float normalizedX = (distanceBambiCrow.x + 500) / 1000f; // Normalize to [0, 1]
                        distanceBambiCrow.x = Mathf.Lerp(0.3f, 0.7f, normalizedX); // Map to [0.05, 0.95]
                    }
                    if (distanceBambiCrow.z > 0)
                    {
                        distanceBambiCrow.z = 500;
                    }
                    else
                    {
                        distanceBambiCrow.z = -500;
                    }

                }
                else
                {
                    if (distanceBambiCrow.z > 500)
                    {
                        distanceBambiCrow.z = 500;
                    }
                    else if (distanceBambiCrow.z < -500)
                    {
                        distanceBambiCrow.z = -500;
                    }
                    else
                    {
                        // Normalize only if within bounds [-500, 500] and map to [0.05, 0.95]
                        float normalizedZ = (distanceBambiCrow.z + 500) / 1000f; // Normalize to [0, 1]
                        distanceBambiCrow.z = Mathf.Lerp(0.3f, 0.7f, normalizedZ); // Map to [0.05, 0.95]
                    }
                    if (distanceBambiCrow.x > 0)
                    {
                        distanceBambiCrow.x = 500;
                    }
                    else
                    {
                        distanceBambiCrow.x = -500;
                    }
                }


               

                // Crow is out of camera view
                // Clamp the text box to the edges of the screen
            //    distanceBambiCrow.x = Mathf.Clamp(distanceBambiCrow.x, 0.05f, 0.95f); // Avoid extreme corners
              //   distanceBambiCrow.z = Mathf.Clamp(distanceBambiCrow.z, 0.05f, 0.95f);
                Debug.Log("Clamp" + distanceBambiCrow);
                targetDir = new Vector3(distanceBambiCrow.x * Screen.width, distanceBambiCrow.z * Screen.height, 0);
            }
            //-515.46, 34.24, -463.26
            // viewportPosition(-448.95, -1403.71, 0.30)
            //0.95 0.95 viewportPosition(269.89, 842.01, -0.50)targetPosition(-516.13, 34.22, -464.63)
            else
            {
                // Crow is visible, position normally
                targetDir = mainCamera.WorldToScreenPoint(crowT.position);
            }

            // Apply offset and set the UI image's position
            targetDir.x = Mathf.Clamp(targetDir.x, screenLimitX, Screen.width - screenLimitX);
            targetDir.y = Mathf.Clamp(targetDir.y, screenLimitY, Screen.height - screenLimitY);
            npcText.transform.position = targetDir;




        }
            /*targetPosition = crowT.position;



            targetDir = mainCamera.WorldToScreenPoint(Vector3.MoveTowards(new Vector3(bambiT.position.x, bambiT.position.z, 0), new Vector3(targetPosition.x, targetPosition.z, 0), distanceWithoutZoomin * Time.deltaTime));

            targetDir.x = Mathf.Clamp(targetDir.x, screenLimitX, Screen.width - screenLimitX);
            targetDir.y = Mathf.Clamp(targetDir.y, screenLimitY, Screen.height - screenLimitY);
            // Apply offset and set the UI image's position
            npcText.transform.position = targetDir;
        }
        
        else if (babyRabbitGO.activeSelf)
        {
            targetPosition = babyRabbitGO.transform.position;
        }
 

        if (!secondCameraOn)
        {
            if (k == " Baby Rabbit")
            {
                targetPosition = babyRabbitGO.transform.position;

            }
            else if (k == " Ms. Crow")
            {
                targetPosition = crowT.position;
            }
            else if (k == " Mr. Bear")
            {
                targetPosition = bearT.position;
            }
            else if (k == " Mr. Fox")
            {
                targetPosition = foxT.position;
            }
            targetDir = mainCamera.WorldToScreenPoint(Vector3.MoveTowards(bambiT.position, targetPosition, distance * Time.deltaTime));
            targetDir.x = Mathf.Clamp(targetDir.x, npcText.rectTransform.sizeDelta.x, Screen.width - npcText.rectTransform.sizeDelta.x);
            targetDir.y = Mathf.Clamp(targetDir.y, npcText.rectTransform.sizeDelta.y, Screen.height - npcText.rectTransform.sizeDelta.y);

            // Apply offset and set the UI image's position
            npcText.transform.position = new Vector3(targetDir.x, targetDir.y, 0);
        }*/
      /*   if(conversationCanvas.activeSelf) 
        {
            targetDir.x = Mathf.Clamp(targetDir.x, npcText.rectTransform.sizeDelta.x, Screen.width - npcText.rectTransform.sizeDelta.x);
            targetDir.y = Mathf.Clamp(targetDir.y, npcText.rectTransform.sizeDelta.y, Screen.height - npcText.rectTransform.sizeDelta.y);

            // Apply offset and set the UI image's position
            npcText.transform.position = new Vector3(targetDir.x, targetDir.y, 0);
        }
        //Vector3 targetPos = Camera.main.WorldToScreenPoint(bambiT.position);

        // Clamp to screen boundaries
       targetDir.x = Mathf.Clamp(targetDir.x, npcText.rectTransform.sizeDelta.x, Screen.width - npcText.rectTransform.sizeDelta.x);
        targetDir.y = Mathf.Clamp(targetDir.y, npcText.rectTransform.sizeDelta.y, Screen.height - npcText.rectTransform.sizeDelta.y);

            // Apply offset and set the UI image's position
        npcText.transform.position = new Vector3(targetDir.x, targetDir.y, 0);*/
       
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (coroutineRunning == false)
            {
                typingDelay = typingDelayTime;
                if (monologueCanvas.activeSelf)
                {
                    NextMonologueLine();
                }

                if (conversationCanvas.activeSelf)
                {
                    if (conversationRunning == true)
                    {
                        NextConversationLine();
                    }
                    else
                    {
                        NextConversationLineWithoutZoomin();
                    }
                }
            }
            else if (coroutineRunning)
            {
                if (conversationCanvas.activeSelf || monologueCanvas.activeSelf)
                {

                    typingDelay = 0;
                    
                }
            }
        }
    }

    public void MonologueData(Dictionary<(int Mindex, string Mcharacter),string> monologueData)
    {
       
        monologueDictionary = monologueData;
        monologueText.text = "";
        maximumMonologueIndex = monologueDictionary.Count;
       
        StartMonologue();

    }
    public void StartMonologue()
    {
        
        //conversationNpc.Clear();
        monologueCanvas.SetActive(true);
       
        monologueText.rectTransform.gameObject.SetActive(true);
        // BambiSc.walkingSpeed = 0;
       
        NextMonologueLine();
    }
    private void NextMonologueLine()
    {
        
        if (currentMonologueIndex >= maximumMonologueIndex)
        { 
            EndMonologue();
            return;
        }

        if (Quest == 15)
        {

        }

        //Conversation conversation = conversationLines[currentLineIndex];

        else
        {
            foreach (var (Mindex, Mcharacter) in monologueDictionary.Keys)
            {
                // Match the current index but exclude the Player
                if (Mindex == currentMonologueIndex && Mcharacter == "Player")
                {

                    monologueTypingText = monologueDictionary[(currentMonologueIndex, "Player")];
                    StartCoroutine(MonologueTypingCoroutine());
                    break;
                }
            }
            if (monologueDictionary.ContainsKey((currentMonologueIndex, "Pause")))
            {
                PauseMonologue();

            }

        }
        
        currentMonologueIndex++;
       
    }
    private void EndMonologue()
    {

        monologueCanvas.SetActive(false);
        monologueText.text = ""; // Clear the dialogue text when done
 
        currentMonologueIndex = 0;
        bambiSc.walkingSpeed = 50;
        secondCameraOn = false;
        MeetingSubtractNormalize();
        if (crow.crowMeeting == -1)
        {
            crow.crowMeeting++;
            crow.gameObject.SetActive(true);
        }
        else if (bigTreeSc.meeting == 1)
        {
            questText = "Find someone to save the big tree.";
            extinctFireText.text = "extinct fire : " + fireOff + "\n"+questText;

        }

        else if (bearSc.meeting == 8 )
        {
            
            bearSc.meeting++;//////9
            questText = "Baby rabbit's family came back to their home.";
            extinctFireText.text = "extinct fire : " + fireOff + "\n" + questText;
            rabbitFamily.SetActive(true);

        }
        else if (babyRabbitSc.rabbitMeeting == 11  && crow.crowMeeting == 6)
        {
            new WaitForSeconds(fireOffDelay * Time.deltaTime);
            crow.crowMeeting++;//7
        }
        if (afterMonologueRunning)
        {
            StartConversation();
            afterMonologueRunning = false;
        }
        else if (afterMonologueRunningWithoutZoomin)
        {
            StartConversationWithoutZoomin();
            afterMonologueRunningWithoutZoomin = false;
        }


    }
    private void PauseMonologue()
    {
        monologueCanvas.SetActive(false);
        monologueText.text = "";
        if (fireFound != fireOff)
        {
            bambiSc.BambiFirePutout();

            Invoke(nameof(FireOff), (fireOffDelay) * Time.deltaTime);
            Invoke(nameof(StartMonologue), fireOffDelay * 1.5f * Time.deltaTime);

        }
        else
        {
            Invoke(nameof(StartMonologue), monologuePauseTime * Time.deltaTime);
        }

    }
    public void FireMonologueOverlapped()
    {


        bambiSc.BambiFirePutout();
        bambiSc.walkingSpeed = 0;
        Invoke(nameof(FireOff), (fireOffDelay) * Time.deltaTime);



    }

    public void ConversationData(Dictionary<(int index,string character),string> conversationData)
    {
        
        conversationDictionary = conversationData;
        maximumConversationIndex = conversationDictionary.Count;
        if (monologueCanvas.activeSelf==false)
        {
            StartConversation();
        }
        else
        {
            bambiSc.walkingSpeed *= 0.5f;
           // Debug.Log(bambiSc.walkingSpeed);
            afterMonologueRunning = true;
        }
        
    }
    public void StartConversation()
    {
        
        conversationRunning = true;
        currentConversationIndex = 0;
        if (secondCameraOn == false)
        {
            cameraFollow.CameraZoomInPosition();
        }
        else 
        {
            cameraFollow.SecondCameraOn();
        }
        
        //conversationNpc.Clear();
        conversationCanvas.SetActive(true);
        npcText.rectTransform.transform.gameObject.SetActive(true);
        playerText.rectTransform.transform.gameObject.SetActive(true);

        bambiSc.walkingSpeed *= 0.3f;
        bambiAnimator.bambiAnimator.speed *= 0.8f;
        Debug.Log(bambiSc.walkingSpeed);
        npcText.text = ""; // Clear text on start
        playerText.text = "";
        playerText.transform.position = new Vector3(Screen.width / 2f, Screen.height * 0.8f, 0);

        NextConversationLine();

    }
    private void NextConversationLine()
    {

        if (currentConversationIndex >= maximumConversationIndex)//////////////////////////////////////////////////////////////////////////////////////FIX
        {
            EndConversation();
            return;
        }



        //Conversation conversation = conversationLines[currentLineIndex];
        if (conversationDictionary.ContainsKey((currentConversationIndex, "Player")))
        {
            playerText.rectTransform.gameObject.SetActive(true);
            conversationTypingText = conversationDictionary[(currentConversationIndex, "Player")];
            StartCoroutine(ConversationTypingCoroutine());
            npcText.text = "";
            npcText.rectTransform.gameObject.SetActive(false);
            
        }
        else
        {
            foreach (var key in conversationDictionary.Keys)
            {   
                
                // Match the current index but exclude the Player
                if (key.Cindex == currentConversationIndex && key.Ccharacter != "Player")
                {
                    conversationKey = key.Ccharacter;
                    NpcTextPositionTargeting();
                    
                    npcText.rectTransform.gameObject.SetActive(true);
                    conversationTypingText = $"{key.Ccharacter}: {conversationDictionary[key]}";
                    StartCoroutine(ConversationTypingCoroutine());
                    playerText.text = ""; // Clear Player's text
                    playerText.rectTransform.gameObject.SetActive(false);
                    break;
                }
            }
        }
        currentConversationIndex++;
        
    }
    private void EndConversation()
    {
        conversationRunning = false;
        conversationCanvas.SetActive(false);
        npcText.text = ""; // Clear the dialogue text when done
        playerText.text = "";
        npcText.rectTransform.transform.gameObject.SetActive(false);
        playerText.rectTransform.transform.gameObject.SetActive(false);
        //Debug.Log(dialogueLines.Count);
        conversationDictionary.Clear();
        bambiSc.walkingSpeed = 50;
        bambiAnimator.bambiAnimator.speed =1;

        currentConversationIndex = 0;
        Quest++;
        secondCameraOn = false;
        MeetingSubtractNormalize();

        cameraFollow.CameraZoomOutPosition();

        if (crow.crowMeeting== 1 && babyRabbitSc.rabbitMeeting == 0) //Quest 1
        {
            crow.crowMeeting++;
            Invoke(nameof(InvokeCrowVoiceGuideConversation), crow.crowGuideDelay);
            StartCoroutine(crow.MoveToLandingspot());
            extinctFireCanvas.SetActive(true);
            extinctFireText.text = "Let's follow Ms. Crow.";

        }
        else if (crow.crowMeeting == 3 && babyRabbitSc.rabbitMeeting == 0)
        {
            cameraFollow.LakeLandscapeOff();
            babyRabbitSc.MeetingBabyRabbit();
            extinctFireText.text = "Extinguish fire in the forest.";

        }

        else if (babyRabbitSc.rabbitMeeting == 1 && bearSc.meeting == 0) // companything with rabbit
        {   
            babyRabbitSc.rabbitMeeting++;///2
            questText = "Let's head to the north west forest.";
            extinctFireText.text = "extinct fire : " + fireOff + "\n" + questText;

            babyRabbitSc.ConversationEndDistance();  //because when camera zooms in the rabbit goes out of camera
            babyRabbitSc.MonologueAfterRabbitMeeting();
        }
        else if(babyRabbitSc.rabbitMeeting == 4 && bearSc.meeting == 0 && foxSc.meeting==1)
        {
            bearSc.gameObject.SetActive(true);
            new WaitForSeconds(fireOffDelay * Time.deltaTime);
            foxSc.MonologueAfterFox();
            questText = "Let's head to the north east forest.";
            extinctFireText.text = "extinct fire : " + fireOff + "\n" + questText;

        }


        else if(bearSc.meeting == 1)
        {
            bearSc.ChasingBambi(); ////meeting++;???????????????
            questText = "Let's head to the south east forest.";
            extinctFireText.text = "extinct fire : " + fireOff + "\n" + questText;

        }
        else if(bearSc.meeting == 3)
        {
            bearSc.ChasingBambi(); ////meeting++;???????????????
            questText = "Let's lure the bear to the big tree.";
            extinctFireText.text = "extinct fire : " + fireOff + "\n" + questText;

        }

    }    
    public void ConversationDataWithoutZoomin(Dictionary<(int index, string character), string> conversationData)
    {
        
        conversationDictionary = conversationData;
        maximumConversationIndex = conversationDictionary.Count;
        if (monologueCanvas.activeSelf == false)
        {
            StartConversationWithoutZoomin();
        }
        else
        {
            afterMonologueRunningWithoutZoomin = true;
        }
    }

    private void StartConversationWithoutZoomin()
    {

        conversationWOZoominRunning = true;
        conversationCanvas.SetActive(true);

        npcText.rectTransform.transform.gameObject.SetActive(true);
        playerText.rectTransform.transform.gameObject.SetActive(true);
        npcText.text = ""; // Clear text on start
        playerText.text = "";
        playerText.transform.position = new Vector3(Screen.width / 2f, Screen.height * 0.6f, 0);

        NextConversationLineWithoutZoomin();

    }
    private void NextConversationLineWithoutZoomin()
    {

             
        if(currentConversationIndex >= maximumConversationIndex)
        {
            EndConversationWithoutZoomin();
            currentConversationIndex = 0;

            return;
        }

        if (conversationDictionary.ContainsKey((currentConversationIndex, "Pause"))) 
        {
            PauseConversationWithoutZoomin();
           
        }



        else if (conversationDictionary.ContainsKey((currentConversationIndex, "Player")))
        {
            playerText.rectTransform.gameObject.SetActive(true);
            conversationTypingText = conversationDictionary[(currentConversationIndex, "Player")];
            StartCoroutine(ConversationTypingCoroutine());
            npcText.text = "";
            npcText.rectTransform.gameObject.SetActive(false);

        }
        else
        {
            
            foreach (var key in conversationDictionary.Keys)
            {
                // Match the current index but exclude the Player
                if (key.Cindex == currentConversationIndex && key.Ccharacter != "Player" && key.Ccharacter != "Pause")
                {
                    conversationKey = key.Ccharacter;
                    NpcTextPositionTargeting();
                    npcText.rectTransform.gameObject.SetActive(true);
                    conversationTypingText = $"{key.Ccharacter}: {conversationDictionary[key]}";
                    StartCoroutine(ConversationTypingCoroutine());
                    playerText.text = ""; 
                    playerText.rectTransform.gameObject.SetActive(false);
                    break;
                }
            }
        }        
        


        currentConversationIndex++;

    }
    private void PauseConversationWithoutZoomin()
    {
        conversationCanvas.SetActive(false);
        npcText.text = "";
        playerText.text = "";
        npcText.rectTransform.transform.gameObject.SetActive(false);
        playerText.rectTransform.transform.gameObject.SetActive(false);
        Invoke(nameof(StartConversationWithoutZoomin), crow.crowGuideDelay);
    }
    private void EndConversationWithoutZoomin()
    {
        bambiSc.walkingSpeed = 50;
        conversationWOZoominRunning = false;
        conversationCanvas.SetActive(false);
        npcText.text = ""; 
        playerText.text = "";
        npcText.rectTransform.transform.gameObject.SetActive(false);
        playerText.rectTransform.transform.gameObject.SetActive(false);
        if (crow.crowMeeting == 4 && bearSc.meeting < 6)
        {
            crow.CrowBucket();

        }
        else if (crow.crowMeeting < 3 && bearSc.meeting < 3)
        {
            Invoke("InvokeCrowVoiceGuideConversation", crow.crowGuideDelay);

        }
        else if (bearSc.meeting == 6)
        {
            bearSc.meeting++;/////7
            bearSc.PouringWater();
        }
        else if (bearSc.meeting == 7 )
        {
            bearSc.AfterByeBearMonologue();
            bearSc.meeting++;////8
            
        }
        
        else if (babyRabbitSc.rabbitMeeting == 7)
        {
            //babyRabbitSc.ByeRabbitConversation();
            babyRabbitSc.rabbitMeeting++;////////8
            babyRabbitSc.MoveAnimation();
      
        }
        
        else if (babyRabbitSc.rabbitMeeting == 9)
        {
          //  new WaitForSeconds(fireOffDelay * Time.deltaTime);
            babyRabbitSc.rabbitMeeting++;//////////////10
            babyRabbitSc.MoveAnimation();
            extinctFireText.text = "extinct fire : " + fireOff;
           // Debug.Log(babyRabbitSc.rabbitMeeting + " it should be 8 after See you");

        }

        else if (crow.crowMeeting == 8 )
        {
            if(fireOff < fireSum)
            {
            StartCoroutine(crow.MoveToLandingspot());
            crow.FireAllClearQuest();
            extinctFireText.text = "extinct fire : " + fireOff + "\n" + "Extinguish more fire in the forest.";
            }
            else
            {
                StartCoroutine(crow.MoveToLandingspot());
             //   crow.crowMeeting++;
            }
            
        }

        else if(crow.crowMeeting == 10)
        {
            StartCoroutine(crow.MoveToLandingspot());
            extinctFireText.text = "extinct fire : " + fireOff + "\n" + "Mom is waiting at the lake.";
        }


        /*
        else if (babyRabbitSc.rabbitMeeting == 10)// Rabbit Family shows up////////////////////////////////////////////////////
        {
            rabbitFamily.SetActive(true);
        }
        else if (babyRabbitSc.rabbitMeeting == 11) // As a Quest 16, meeting rabbit family
        {
            StartCoroutine(babyRabbitSc.TowardsFamilyEnding());
        }
        */


        secondCameraOn = false;
        MeetingSubtractNormalize();
    }
    private void InvokeCrowVoiceGuideConversation()
    {
        crow.CrowVoiceGuideConversation();
        if (Quest == 1)
        {
            crow.CrowVoiceGuideMonologue();
            Quest++;
        }

    }

    IEnumerator MonologueTypingCoroutine()
    {
        typingDelay = typingDelayTime;
        coroutineRunning = true;
        monologueText.text = string.Empty;
        for (intM = 0; intM < monologueTypingText.Length; intM++)
        {

                monologueText.text += monologueTypingText[intM];
               


            yield return new WaitForSeconds(typingDelay*Time.deltaTime);
            
        }
        
        yield return null;
        coroutineRunning = false;
       
        //for (int i = 0; i)
    }
    IEnumerator ConversationTypingCoroutine()
    {
        typingDelay = typingDelayTime;
        coroutineRunning = true;
        npcText.text = string.Empty;
        playerText.text = string.Empty;
        for (intC = 0; intC < conversationTypingText.Length; intC++)
        {
            if (playerText.rectTransform.gameObject.activeSelf)
            {
                playerText.text += conversationTypingText[intC];
            }
            else if (npcText.rectTransform.gameObject.activeSelf)
            {
                npcText.text += conversationTypingText[intC];
            }

            yield return new WaitForSeconds(typingDelay * Time.deltaTime);

        } 

        yield return null;
        coroutineRunning = false;
        
        //for (int i = 0; i)
    }


    public void FireOff()
    {

        fireOff++;
        new WaitForSeconds(fireOffDelay*0.5f*Time.deltaTime);
        extinctFireCanvas.SetActive(true);
        extinctFireText.text = "extinct fire : " + fireOff + "\n" + questText;
        bambiSc.walkingSpeed = 50f;

    }

    void MeetingSubtractNormalize()
    {
        babyRabbitSc.rabbitMeetingSubtract = 0;
        crow.crowMeetingSubtract = 0;
        bearSc.meetingSubtract = 0;
        bigTreeSc.meetingSubtract= 0;
    }
    void NpcTextPositionTargeting()
    {
        
        if (conversationKey == " Ms. Crow" && crow.crowMeeting != 2)
        {
            targetPosition = crowT.position;
        }
        
        else if (conversationKey == " Baby Rabbit")
        {
            targetPosition = babyRabbitGO.transform.position;
            
        }

        else if (conversationKey == " Mr. Bear")
        {
            targetPosition = bearT.position;
        }
        else if (conversationKey == " Mr. Fox")
        {
            targetPosition = foxT.position;
        }
        //Vector3 targetPos = Camera.main.WorldToScreenPoint(bambiT.position);

        // Clamp to screen boundaries


     //   targetDir = mainCamera.WorldToScreenPoint(Vector3.MoveTowards(bambiT.position, targetPosition, 1));
        if ((bambiT.position.x - targetPosition.x) >= 0 && (bambiT.position.z - targetPosition.z) < 0)
        {
            targetDir = new Vector3(Screen.width*0.3f,Screen.height*0.8f,0);
            Debug.Log("¿ÞÀ§"+ (bambiT.position.x - targetPosition.x)+"\n"+ (bambiT.position.z - targetPosition.z));
            Debug.Log("\n" + crowT.position + "\n" +targetPosition);
        }
        else if((bambiT.position.x - targetPosition.x) < 0 && (bambiT.position.z - targetPosition.z) < 0)
        {
            targetDir = new Vector3(Screen.width * 0.7f, Screen.height * 0.8f, 0);
            Debug.Log("¿ÀÀ§"+ (bambiT.position.x - targetPosition.x)+"\n"+ (bambiT.position.z - targetPosition.z));
        }
        else if ((bambiT.position.x - targetPosition.x) >= 0 && (bambiT.position.z - targetPosition.z) >= 0)
        {
            targetDir = new Vector3(Screen.width * 0.3f, Screen.height * 0.2f, 0);
            Debug.Log("¿Þ¾Æ"+ (bambiT.position.x - targetPosition.x)+"\n"+ (bambiT.position.z - targetPosition.z));
        }
        else if ((bambiT.position.x - targetPosition.x) < 0 && (bambiT.position.z - targetPosition.z) >= 0)
        {
            targetDir = new Vector3(Screen.width * 0.7f, Screen.height * 0.2f, 0);
            Debug.Log("¿À¾Æ"+ (bambiT.position.x - targetPosition.x)+"\n"+ (bambiT.position.z - targetPosition.z));
        }
        
       // targetDir = mainCamera.WorldToScreenPoint(Vector3.MoveTowards(bambiT.position, targetPosition, distance * Time.deltaTime));
        targetDir.x = Mathf.Clamp(targetDir.x, npcText.rectTransform.sizeDelta.x * 2, Screen.width - (npcText.rectTransform.sizeDelta.x * 1.5f));
        targetDir.y = Mathf.Clamp(targetDir.y, npcText.rectTransform.sizeDelta.y, Screen.height - (npcText.rectTransform.sizeDelta.y));
        // Apply offset and set the UI image's position
        npcText.transform.position = new Vector3(targetDir.x, targetDir.y, 0);
        

    }
    void NpcTextPositionTargetingWithoutZoomin()
    {
        if (conversationKey == " Ms. Crow" && crow.crowMeeting != 2)
        {
            targetPosition = crowT.position;
            Debug.Log(targetPosition.ToString());
        }

        else if (conversationKey == " Baby Rabbit")
        {
            targetPosition = babyRabbitGO.transform.position;

        }

        else if (conversationKey == " Mr. Bear")
        {
            targetPosition = bearT.position;
        }
        else if (conversationKey == " Mr. Fox")
        {
            targetPosition = foxT.position;
        }
        //Vector3 targetPos = Camera.main.WorldToScreenPoint(bambiT.position);

        // Clamp to screen boundaries


        targetDir = mainCamera.WorldToScreenPoint(Vector3.MoveTowards(new Vector3(bambiT.position.x, bambiT.position.z, 0), new Vector3(targetPosition.x, targetPosition.z, 0), distanceWithoutZoomin * Time.deltaTime));
        targetDir.x = Mathf.Clamp(targetDir.x, npcText.rectTransform.sizeDelta.x * 2, Screen.width - (npcText.rectTransform.sizeDelta.x * 1.5f));
        targetDir.y = Mathf.Clamp(targetDir.y, npcText.rectTransform.sizeDelta.y, Screen.height - (npcText.rectTransform.sizeDelta.y));

        // Apply offset and set the UI image's position
        npcText.transform.position = new Vector3(targetDir.x, targetDir.y, 0);


    }
    public void GameFinishCredit()
    {
        conversationDictionary = new Dictionary<(int, string), string>
            {
                {(0, "Player") , " Developer / Game Designer : Eunchae Won \n \n Studio Big Small Game " },
            };
        MonologueData(conversationDictionary);

    }

}
    