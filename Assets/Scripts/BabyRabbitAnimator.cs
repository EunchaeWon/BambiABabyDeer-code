using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class BabyRabbitAnimator : MonoBehaviour
{
    public BambiAnimator bambiAnimator;
    public Script_CharacterMovement bambiSc;
    public int rabbitMeeting=0;
    public Animator animator;
    //public Collider sphereCollider;
    public List<string> dialogueLines;
    public GameManager gameManager;
    public float limitDistance =150;
    public Transform targetBambi;
    public Vector3 offset = new Vector3(10, 10, 10);
    public Vector3 newOffset = new Vector3(50, 30, 50);
    public Vector3 velocity;
    public float distance; //acceptable distance
    private Vector3 targetPosition;
    private Vector3 limitedPosition;
    public float smoothSpeed = 1f;
    public float gravity = 9.81f;
    private Vector3 velocityG;
    public CharacterController characterController;
    public float walkingSpeed = 25f;
    public float rotationSpeed = 1.0f;
    private Vector3 rotate;
    private float speed;
    private bool running = false;
    public float runningSpeed = 100;
    public GameObject babyRabbit;
    public Transform rabbitFamily;
    public Transform BRabbit;
    public Dictionary<(int index, string character), string> conversationDictionary;
    public int rabbitMeetingSubtract = 0;
    public FoxSc foxSc;


    private void Start()
    {
     babyRabbit.SetActive(false); 
        animator = GetComponent<Animator>();
    }

    public void MeetingBabyRabbit()
    {
        babyRabbit.SetActive (true);
       
        animator.SetBool("Talk", true);
        Collider collider = GetComponent<Collider>();
        characterController = GetComponent<CharacterController>();
        
    }

    void OnTriggerEnter(Collider other)//Conversation and quest start
    {
        
        if (other.CompareTag("Player") && rabbitMeeting == 0)
        {
            rabbitMeetingSubtract = 1;
            rabbitMeeting++;
            conversationDictionary = new Dictionary<(int index, string character), string>
            { 
                {(0," ??"), "Sob, sob..." },
                {(1, "Player"),"Oh, it's baby rabbit! \n Hello, where is your mom?"},
                {(2, " Baby Rabbit"), "I am lost..." },
                {(3, "Player"), "Oh, well, me either... \n I am looking for my mom" },
                {(4, " Baby Rabbit"), "I want to find my family. "},
                {(5, "Player"), "I guess your family evacuated from your home due to fire. \n But fire is now extinguishing. \n I am sure they will come back home soon. " },
                {(6, " Baby Rabbit"), " But the forest looks so different from before since fire. \n I don't remember how to find my home either." },
                {(7, "Player"), " Well, how about to come along with me? \n Let's find home together." },
                {(8, " Baby Rabbit"), "Thank you! Let's go!" }
                
            };
            bambiSc.walkingSpeed *= 0.2f;
            bambiAnimator.bambiAnimator.speed *= 0.2f;
            gameManager.ConversationData(conversationDictionary);
            
            //sphereCollider.enabled = true;

        }



        // Call this to display the next line, e.g., on button click

    }

    


    void FixedUpdate()
    {
        /*
        velocity.y += gravity * Time.deltaTime; // Adjust gravity over time
        characterController.Move(velocity * Time.deltaTime); // Move the character with gravity*/
        if (rabbitMeeting >1 && rabbitMeeting < 5)
        {
            //transform.LookAt(targetBambi.position);
            if (running == false)
            {
                MoveCharacter3();
            }
            
            //animator.SetBool("Talk", false);
               targetPosition = targetBambi.position + offset;
               Vector3 rabbitPosition = transform.position;

            if (Mathf.Abs(rabbitPosition.x - targetPosition.x) > distance || Mathf.Abs(rabbitPosition.z - targetPosition.z) > distance)
            {
                //transform.position = Vector3.SmoothDamp(targetPosition, rabbitPosition, ref velocity, smoothSpeed);
                transform.position = Vector3.MoveTowards(transform.position, targetBambi.position, runningSpeed * Time.deltaTime);
                running = true;


                /*//transform.position.y -= gravity * Time.deltaTime;


                //animation(Jump)*/
            }
            else
            {
                if (running == true)
                {
                    running = false;
                }
            }
               

           
        }
        
        else if (rabbitMeeting == 5)////////////////////////////bambiSc>>> TriggerEnter ++
        {
            
            float distanceX = Mathf.Abs(targetBambi.position.x - transform.position.x);
            float distanceZ = Mathf.Abs(targetBambi.position.z - transform.position.z);
            
            if (distanceX > limitDistance && distanceZ > limitDistance)
            {
                BRabbit.position = Vector3.MoveTowards(BRabbit.position, targetBambi.position, runningSpeed * Time.deltaTime);
                Debug.Log(distanceX + ", " + distanceZ);
            }
            else
            {
           
                animator.SetBool("Move", false);
            
                rabbitMeeting++;///////////6
                Debug.Log("Stop" + rabbitMeeting);
                FoundFamily();
                Debug.Log("Stop" + rabbitMeeting);/////7
            }

        }
        else if (rabbitMeeting == 8)
        {
            ByeRabbitConversation();

        }
        else if (rabbitMeeting == 10)
        {
            float distanceX = Mathf.Abs(rabbitFamily.transform.position.x - transform.position.x);
            float distanceZ = Mathf.Abs(rabbitFamily.transform.position.z - transform.position.z);

            if (distanceX > limitDistance*0.5f && distanceZ > limitDistance*0.5f)
            {
                BRabbit.position = Vector3.MoveTowards(BRabbit.position, rabbitFamily.transform.position, runningSpeed * Time.deltaTime);

            }
            else
            {
                rabbitMeeting++;
                AfterByeRabbitMonologue();
            }
        }
       


    }



    void MoveCharacter3()
      {

            //Rotation

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");



            Vector3 movement = new Vector3(x, 0f, z).normalized;

            if (movement.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * walkingSpeed * Time.deltaTime);
            }

            float y = transform.eulerAngles.y;

            speed = 0;



            if (z != 0)
            {


                if (x == 0)
                {

                    if (y > 0)
                    {
                        speed = rotationSpeed;
                        rotate = new Vector3(0, -z * speed * Time.deltaTime, 0); //anticlockwise -

                    }
                    else if (y < 0)
                    {
                        speed = rotationSpeed;
                        rotate = new Vector3(0, z * speed * Time.deltaTime, 0); // clockwise +

                    }
                    else if (y == 0 || Mathf.Abs(y) == 180)
                    {
                        speed = 0;

                        rotate = new Vector3(0, z * speed * Time.deltaTime, 0);

                    }


                }

                else if (x != 0)
                {




                    //Diagonal Rotate


                    if (z > 0)
                    {
                        speed = rotationSpeed;

                        if (Mathf.Abs(y) < 45) //|| Mathf.Abs(y) > 90 && Mathf.Abs(y) < 135)// Downward+- -+
                        {
                            rotate = new Vector3(0, x * speed * Time.deltaTime, 0);
                        }

                        else if (Mathf.Abs(y) > 45) //&& Mathf.Abs(y) < 90 || Mathf.Abs(y) > 135 && Mathf.Abs(y) < 180) //Upward-+ +-
                        {
                            rotate = new Vector3(0, -x * speed * Time.deltaTime, 0);

                        }

                        else if (x > 0 && y == 45)
                        {
                            speed = 0;
                            rotate = new Vector3(0, x * speed * Time.deltaTime, 0);
                        }
                        else if (z > 0 && x < 0 && y == -45)
                        {
                            speed = 0;
                            rotate = new Vector3(0, x * speed * Time.deltaTime, 0);
                        }


                    }

                    else if (z < 0)
                    {
                        speed = rotationSpeed;

                        if (Mathf.Abs(y) < 135)
                        {
                            rotate = new Vector3(0, x * speed * Time.deltaTime, 0);// Downward+- -+
                        }

                        else if (Mathf.Abs(y) > 135)
                        {
                            rotate = new Vector3(0, -x * speed * Time.deltaTime, 0);//Upward-+ +-
                        }
                        else if (x > 0 && y == 135)
                        {
                            speed = 0;
                            rotate = new Vector3(0, x * speed * Time.deltaTime, 0);
                        }
                        else if (x < 0 && y == -135)
                        {
                            speed = 0;
                            rotate = new Vector3(0, x * speed * Time.deltaTime, 0);
                        }
                    }

                }

            }

            else if (z == 0 && x != 0)

            {



                //Flip or Fast Turn

                if (x < 0 && y > 0)
                {

                    if (Mathf.Abs(y) < 90) // anticlockwise --

                    {
                        speed = rotationSpeed;
                        rotate = new Vector3(0, -x * speed * Time.deltaTime, 0);

                    }

                    else if (Mathf.Abs(y) > 90) // clockwise ++
                    {
                        speed = rotationSpeed;
                        rotate = new Vector3(0, x * speed * Time.deltaTime, 0);
                    }

                }
                else if (x > 0 && y < 0)
                {
                    if (Mathf.Abs(y) < 90) // clockwise ++
                    {
                        speed = rotationSpeed;
                        rotate = new Vector3(0, x * speed * Time.deltaTime, 0);
                    }


                    else if (Mathf.Abs(y) > 90) // anticlockwise --

                    {
                        speed = rotationSpeed;
                        rotate = new Vector3(0, -x * speed * Time.deltaTime, 0);

                    }

                    else if (x > 0 && y > 0)

                    {

                        if (Mathf.Abs(y) < 90) // clockwise +
                        {
                            speed = rotationSpeed;
                            rotate = new Vector3(0, x * speed * Time.deltaTime, 0);
                        }

                        else if (Mathf.Abs(y) > 90) // anticlockwise -
                        {
                            speed = rotationSpeed;
                            rotate = new Vector3(0, -x * speed * Time.deltaTime, 0);
                        }
                    }
                    else if (x < 0 && y < 0)
                    {

                        if (Mathf.Abs(y) < 90) // anticlockwise -
                        {
                            speed = rotationSpeed;
                            rotate = new Vector3(0, -x * speed * Time.deltaTime, 0);
                        }
                        else if (Mathf.Abs(y) > 90) // clockwise +
                        {
                            speed = rotationSpeed;
                            rotate = new Vector3(0, x * speed * Time.deltaTime, 0);
                        }
                    }

                    else if (x > 0 && y == 90)
                    {
                        speed = 0;
                        rotate = new Vector3(0, x * speed * Time.deltaTime, 0);
                    }
                    else if (x < 0 && y == -90)
                    {
                        speed = 0;
                        rotate = new Vector3(0, x * speed * Time.deltaTime, 0);
                    }

                }
            }

            // Animation
            if (x != 0 || z != 0)
            {
                animator.SetBool("Move", true);
            }

            else if (x == 0 && z == 0)
            {
                animator.SetBool("Move", false);
            }

            this.transform.Rotate(rotate);

            float gravityY = -gravity * Time.deltaTime;
            //Move
            characterController.Move(new Vector3(Input.GetAxis("Horizontal") * walkingSpeed * Time.deltaTime, gravityY, Input.GetAxis("Vertical") * walkingSpeed * Time.deltaTime));

        
     } 
    
    public void ConversationEndDistance()
    {
        offset = newOffset;
        animator.SetBool("Talk", false);
    }

   /* public void TowardsFamilyEnding()
    {
        characterController.Move(Vector3.MoveTowards(this.transform.position, gameManager.rabbitFamily.transform.position, runningSpeed * Time.deltaTime));
    }

    public IEnumerator TowardsBambi()
    {
        BRabbit.LookAt(targetBambi);
        animator.SetBool("Move",true);
        float distanceX = Mathf.Abs(targetBambi.position.x-transform.position.x);
        float distanceZ = Mathf.Abs(targetBambi.position.z - transform.position.z);

        while (distanceX > limitDistance && distanceZ > limitDistance)
        {
            BRabbit.position = Vector3.MoveTowards(BRabbit.position, targetBambi.position, runningSpeed * Time.deltaTime);
            Debug.Log("distanceX" + distanceX + "distanceZ"+ distanceZ);
            yield return null;
        }
        animator.SetBool("Talk", false);
        animator.SetBool("Move", false);
        
        
    }*/

    public void MonologueAfterRabbitMeeting()
    {
        if (rabbitMeeting == 2)
        {

            rabbitMeeting++;///3
            rabbitMeetingSubtract = 1;
            conversationDictionary = new Dictionary<(int index, string character), string>
            {
                {(0,"Player"), "Let's find baby rabbit's home first." },
                {(1, "Player"),"This baby rabbit is too young to leave it alone."},
                {(2, "Player"), "There are a lot of small caves in the north west forest." },
                {(3, "Player"), "I think one of them is this baby rabbit's home." },

            };

            gameManager.MonologueData(conversationDictionary);
            foxSc.FoxAppear(); //////rabbitMeeting = 3>>4
            //sphereCollider.enabled = true;

        }
    }
    public void FoundFamily()//////////////////////////////Crow=6,Fox=2,Bear=8,rabbit=7 NPC group all enabled.!!!!!!!!!!!!!!!!!!!!
    {
        conversationDictionary = new Dictionary<(int index, string character), string>
            {
                {(0,"Player"), "Look, Baby Rabbit! Over there!" },
                {(1, " Baby Rabbit")," Mom! Dad! Big Sisters! Brothers!"},
            };
        bambiSc.walkingSpeed = 0;
        gameManager.ConversationDataWithoutZoomin(conversationDictionary);
        rabbitMeeting++; /////////7  >>>>>>>>>>>>>>>>FixedUpdate++8
        rabbitMeetingSubtract = 1;
        
        
    }
    /*
    public IEnumerator TowardsFamilyEnding()
    {
        animator.SetBool("Move", true);
        // runningSpeed = runningSpeed * 0.5f;
        BRabbit.LookAt(rabbitFamily.position);
        float distanceX = Mathf.Abs(rabbitFamily.position.x - transform.position.x);
        float distanceZ = Mathf.Abs(rabbitFamily.position.z - transform.position.z);
        while (distanceX > limitDistance && distanceZ > limitDistance)
        {
            BRabbit.position = Vector3.MoveTowards(BRabbit.position, rabbitFamily.position, runningSpeed * Time.deltaTime);

            yield return null;
        }
        
       // ByeRabbitConversation();
    }
    */

    public void ByeRabbitConversation()/////8
    {
        
        animator.SetBool("Move", false);
        conversationDictionary = new Dictionary<(int index, string character), string>
            {
                {(0," Baby Rabbit"), "Thank you, baby dear." },
                {(1, "Player")," I am glad that I could help you. \n See you!"}
            };
        bambiSc.walkingSpeed = 0;
        gameManager.ConversationDataWithoutZoomin(conversationDictionary);
        rabbitMeeting++;////rabbitMeeting >>9 >>10++
        rabbitMeetingSubtract = 1;

        
    }
    public void AfterByeRabbitMonologue()
    {
        conversationDictionary = new Dictionary<(int index, string character), string>
            {
                {(0,"Player"), "I didn't know that the rabbit family was that big." },
                {(1, "Player"),"Well, now I am alone again. \n I guess I feel lonely now."},
                {(2, "Player"), "Now I have no idea where to go to find my mom..." }
            };
        bambiSc.walkingSpeed *= 0.5f;
        gameManager.MonologueData(conversationDictionary);
        //rabbitMeeting++;////FixedUpdate ++11
        rabbitMeetingSubtract = 1;//////////////FixedUpdate meeting///////////// >>>>>crow appears
        Debug.Log(rabbitMeeting +" it should be 10 Monologue");
    }
    public void MoveAnimation()
    {
        animator.SetBool("Move", true);
    }
}