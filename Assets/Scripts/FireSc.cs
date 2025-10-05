using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.SceneManagement;
using UnityEngine;

public class FireSc : MonoBehaviour
{
    //  public GameObject fireInstance;
    public GameManager gameManager;
    public Script_CharacterMovement bambiSc;
    public Dictionary<(int index, string character), string> monologueDictionary;
    public Crow crowSc;
    public ParticleSystem fire;

    private void Start()
    {
        //    Instantiate(fireInstance, transform.position, Quaternion.identity);
    }
    // private GameObject fire;

    // Start is called before the first frame update

    public void OnTriggerEnter(Collider collider)
    {

        if (collider.CompareTag("Player")&& gameManager.fireFound == gameManager.fireOff && gameManager.monologueCanvas.activeSelf == false && crowSc.crowMeeting > 2 )
        {
            bambiSc.firePosition = transform.position;
            if (gameManager.fireFound == 0)
            {
                gameManager.fireOn = true;
                monologueDictionary = new Dictionary<(int, string), string>
                {
                    {(0, "Player"),"Oh, a fire!" },
                    {(1, "Player"), "If I don't put out this fire right now, it will spread again..." },
                    {(2, "Pause")," " },
                    {(3, "Player"), "Now it's safe." }
                };
                gameManager.MonologueData(monologueDictionary);
                bambiSc.walkingSpeed = 0;
                gameManager.fireFound++;
            }
            else if (gameManager.fireFound != 0)
            {
                gameManager.fireOn = true;
                monologueDictionary = new Dictionary<(int, string), string>
                {
                    { (0, "Player"), "A fire!"},
                    { (1, "Pause")," " },
                    { (2, "Player"), "Now it's safe." }
                };
                gameManager.MonologueData(monologueDictionary);
                bambiSc.walkingSpeed = 0;
                gameManager.fireFound++;

                // fire = this.GetComponent<GameObject>();
            }
            Debug.Log(bambiSc.firePosition);

        }

        else if (collider.CompareTag("Player")&& gameManager.fireFound == gameManager.fireOff && gameManager.monologueCanvas.activeSelf && crowSc.crowMeeting > 2 )
        {
            bambiSc.firePosition = transform.position;
            Debug.Log(bambiSc.firePosition);
            gameManager.fireOn = true;
            gameManager.fireFound++;
            bambiSc.walkingSpeed = 0;
            gameManager.FireMonologueOverlapped();
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        gameManager.fireOff = gameManager.fireFound;
    }
    public void BigFireOut()
    {
        fire = GetComponent<ParticleSystem>();
   

        
        

        new WaitForSeconds(6 * Time.deltaTime);
        gameManager.fireFound++;
        gameManager.FireOff();
        fire.Stop();


    }
}
