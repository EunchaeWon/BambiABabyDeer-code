using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


public class Script_CharacterMovement : MonoBehaviour
{
    public BabyRabbitAnimator rabbitSc;
    public FireSc fireSc;

    public BearSc bearSc;
    public float walkingSpeed = 20f;
    public float rotationSpeed = 5.0f;
  //  public GameObject character;
    public float gravity = -9.81f;
    // public Rigidbody rigidBody;
    private CharacterController controller;
    // private Vector3 velocity;
    public BambiAnimator bambiAnimator;
    private Animator animator;
    private Vector3 rotate;
    private float speed;

    private Vector3 moveDirection;
    private Quaternion targetRotation;
    public GameManager gameManager;
    public Transform targetCrow;
    public AudioSource audioSource;
    private bool running;
    public AudioClip runningClip;
    public float volume;
    public float pitch;
    public float walkSoundDelay = 10;
    public Vector3 firePosition;
    
    public ParticleSystem ps;



    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        animator = bambiAnimator.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.Pause();
        //   transform.rotation = Quaternion.Euler(0, 0, 0);
        controller = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;
       // Debug.Log("start");

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            controller.Move(new Vector3(0, 3, 1));
            //Debug.Log("Jump");
            //transform.position = new Vector3(transform.position.x,transform.position.y+10,transform.position.z);
        }

                

        //MoveCharacter1();
         //MoveCharacter2();
        MoveCharacter3();
    }

    void MoveCharacter1()   
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        animator.SetBool("isWalking", false);
        
             
        // Reset move direction at the beginning   
        moveDirection = Vector3.zero;

        // Normalize the move direction for consistent speed
        moveDirection.Normalize();


        if (z > 0) // Forward
        {
            moveDirection += transform.forward;
            animator.SetBool("isWalking", true);

        }
        if (z < 0) // Backward
        {
            moveDirection += -transform.forward;
            animator.SetBool("isWalking", true);

        }
        if (x < 0) // Left
        {
            moveDirection += -transform.right;
            animator.SetBool("isWalking", true);

        }
        if (x > 0) // Right
        {
            moveDirection += transform.right;
            animator.SetBool("isWalking", true);
        }
       



            // Rotate the character to face the move direction smoothly
        RotateCharacter(moveDirection);

        // gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move based on direction and speed
        controller.Move(moveDirection *walkingSpeed * Time.deltaTime);

    }
    void MoveCharacter2()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        

        animator.SetBool("isWalking", false);

        // Reset move direction at the beginning   
        moveDirection = Vector3.zero;

        // Normalize the move direction for consistent speed
        moveDirection.Normalize();
        

        if (z > 0) // Forward
        {
            moveDirection += transform.forward;
            animator.SetBool("isWalking", true);
            RotateLook2(moveDirection);
            RotateCharacter2();
        }
        if (z < 0) // Backward
        {
            moveDirection += -transform.forward;
            animator.SetBool("isWalking", true);
            RotateLook2(moveDirection);
            RotateCharacter2();
        }
        if (x < 0) // Left
        {
            moveDirection += -transform.right;
            animator.SetBool("isWalking", true);
            RotateLook2(moveDirection);
            RotateCharacter2();
        }
        if (x > 0) // Right
        {
            moveDirection += transform.right;
            animator.SetBool("isWalking", true);
            RotateLook2(moveDirection);
            RotateCharacter2();
        }

        


        // Rotate the character to face the move direction smoothly
        

        // gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move based on direction and speed
        controller.Move(moveDirection * walkingSpeed * Time.deltaTime);

    }

    void RotateLook2(Vector3 direction)
    {
        // Convert the direction vector into a quaternion for rotation
        targetRotation = Quaternion.LookRotation(direction);
    }
    void RotateCharacter2() 
    { 
        // Smoothly rotate the character towards the target direction
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
    }
    void RotateCharacter(Vector3 direction) //MoveCharacter1()
    {
        // Convert the direction vector into a quaternion for rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate the character towards the target direction
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
            transform.Translate(Vector3.forward * walkingSpeed*Time.deltaTime);
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
            animator.SetBool("isWalking", true);
            if(running == false) 
            {
                SoundPlay();

            }
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
        animator.SetBool("isWalking", true);

        if (running == false)
        {
            SoundPlay();
            
        }
    }

    else if (x == 0 && z == 0)
    {
        animator.SetBool("isWalking", false);
        running = false;
        audioSource.Stop();

    }

    this.transform.Rotate(rotate);

    float gravityY =- gravity * Time.deltaTime;
                    //Move
    controller.Move(new Vector3(Input.GetAxis("Horizontal") * walkingSpeed * Time.deltaTime, gravityY, Input.GetAxis("Vertical") * walkingSpeed * Time.deltaTime));
    
    
    }
   

    void SoundPlay()
    {
        audioSource.clip = runningClip;
        audioSource.Play();
        running = true;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        Invoke(nameof(SoundLoopDelay), walkSoundDelay);
    }
    void SoundLoopDelay()
    {
        if (running)
        {
            audioSource.clip = runningClip;
            audioSource.Play();
            running = true;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            Invoke(nameof(SoundLoopDelay), walkSoundDelay);
        }
    }
    public void BambiFirePutout()
    {
        animator.SetTrigger("FirePutout");

        TurnAround();



    }
    private void OnTriggerEnter(Collider other)
    {
       /* if (other.CompareTag("Fire"))
        {
            gameManager.fireOn = true;
            firePosition = other.transform.position;
        }*/
        if (other.CompareTag("RabbitFamily")&&rabbitSc.rabbitMeeting == 4)
        {
            rabbitSc.rabbitMeeting++;//////////////5
            //rabbitSc.animator.SetBool("Move", true);
            walkingSpeed = 10;
            //StartCoroutine(rabbitSc.TowardsBambi());
        }
        else if (other.CompareTag("Finish"))
        {
            gameManager.GameFinishCredit();
            walkingSpeed = 0;
            gameManager.conversationCanvas.SetActive(true);
            gameManager.monologueCanvas.SetActive(true);
            gameManager.npcText.text = "end";
            gameManager.playerText.text = "Mom!!";
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Fire") && other.gameObject.activeSelf && gameManager.fireFound == gameManager.fireOff && gameManager.fireOn&&gameManager.fireFound!=0 )//&& gameManager.monologueCanvas.activeSelf==false)
        {
            ps = other.gameObject.GetComponent<ParticleSystem>();
            ps.Stop();

            other.enabled = false;
            gameManager.fireOn = false;
            walkingSpeed = 50;
            rotationSpeed = 5;
        }

    }
    public void TurnAround()
    {
        rotationSpeed = 0.1f;
        Debug.Log("Turn");
        Vector3 directionToTarget = transform.position - firePosition;
        directionToTarget.y = 0;
        Vector3 oppositeDirection = directionToTarget;
        Quaternion fireputoutRotation = Quaternion.LookRotation(oppositeDirection, Vector3.up);
        transform.rotation = fireputoutRotation;
        //
    }
    
  
}
