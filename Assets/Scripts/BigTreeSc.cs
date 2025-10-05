using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTreeSc : MonoBehaviour
{
    public GameManager gameManager;
    public BearSc bearSc;
    public Crow crowSc;
    public Dictionary<(int index, string character), string> monologueDictionary;
    public float meeting;
    public Script_CharacterMovement bambiSc;
    public float meetingSubtract=0;
    public AudioSource audioSource;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.monologueCanvas.activeSelf==false)
        {
            if (meeting == 0)
            {
                monologueDictionary = new Dictionary<(int, string), string>
                {
                    { (0,"Player"), "Oh, this is the biggest tree in our forest. \n But it's on fire!" },
                    { (1, "Player") , "I am not able to reach those fires on the high branches. \n I think I need someone for help." },

                };

                if (bearSc.meeting > 0)
                {
                    bearSc.meeting++;
                }

                gameManager.MonologueData(monologueDictionary);
                bambiSc.walkingSpeed = 0;
                meeting++;/////1
                meetingSubtract = 1;

            }
            else if (meeting == 1 && bearSc.meeting < 3 && bearSc.meeting != 1)
            {


                monologueDictionary = new Dictionary<(int, string), string>
                    {
                        { (0, "Player") , "I think I need someone for help." },

                    };
                gameManager.MonologueData(monologueDictionary);
                bambiSc.walkingSpeed = 0;

            }
            else if (meeting == 1 && bearSc.meeting == 1)
            {
                bearSc.meeting++;
                bearSc.meetingSubtract = 1;
                bearSc.OnBearCollisionEnter();
            }


        }


        
        
        
    }        

 
        
}
