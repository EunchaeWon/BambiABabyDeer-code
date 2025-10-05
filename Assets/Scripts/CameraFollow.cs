
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 1f;
    public Vector3 offset = new Vector3(0f, 250f, -150f);
    public Vector3 zoomInAngle = new Vector3(0f, 250f, -190f);
    public Vector3 zoomOutAngle = new Vector3(0f, 250f, -150f);
    public float fieldOfViewZoomOut = 50f;
    public float fieldOfViewZoomIn = 15f;
    public Vector3 velocity;
    public float distance; //acceptable distance
    private Vector3 targetPosition;
    private Vector3 limitedPosition;
    public Camera mainCamera;
    public Vector3 positionZIn = new Vector3(0f, 100f, -227f);
    public Vector3 positionZOut = new Vector3(-315f, 255f, -190f);
    public Vector3 rotationZIn = new Vector3(25f, 0f,0f);
    public Vector3 rotationZOut = new Vector3(53f, 0f, 0f);
    public GameManager gameManager;

    public Transform player; // The player or target the camera follows
    public Vector3 initialOffset = new Vector3(0, 20, -50); // Default offset from the player

    private Vector3 adjustedPosition;
    private Quaternion adjustedRotation;
    public Vector3 landscapeAdjust = new Vector3(0f, 0f, 0f);
    public Vector3 landsacpeAdjustRotate = new Vector3(0f, 0f, 0f);
    public Camera lakeCamera;
    public Camera secondCamera;
    public Transform lakeCameraMovingSpot;
    public float movingSpeed=5;
    
    void Start()
    {
        mainCamera.enabled = true;
        secondCamera.enabled = false;
        transform.position = target.position + offset;
        lakeCamera.enabled = false;
     
    }
    void FixedUpdate()
    {
 /*       if ( gameManager.monologueCanvas.activeSelf == false)
        {*/
            targetPosition = target.position + offset;
            Vector3 cameraPosition = transform.position;

            if ( Mathf.Abs(cameraPosition.x-targetPosition.x) > distance || Mathf.Abs(cameraPosition.z - targetPosition.z) > distance)
            {
                transform.position = Vector3.SmoothDamp(targetPosition, cameraPosition, ref velocity, smoothSpeed);
            }

      //  }


        //Vector3.Lerp(targetPosition, desiredPosition, Time.deltaTime * smoothSpeed);

        //transform.LookAt(target);
        /*
                Vector3 targetPosition = target.TransformPoint(offset);
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);
                transform.LookAt(target);
        */
        //CameraZoomInPosition();
    }
    public void CameraZoomInPosition()
    {
        mainCamera.fieldOfView = fieldOfViewZoomIn;
        offset = positionZIn;
        //mainCamera.transform.position = positionZIn;
        mainCamera.transform.rotation =  Quaternion.Euler(rotationZIn);
       
        // Set initial camera position based on offset
        adjustedPosition = player.position + initialOffset;
        transform.position = adjustedPosition;

        // Make sure camera is looking at the player initially
        //transform.LookAt(player);
        transform.position = Vector3.Lerp(transform.position, adjustedPosition, Time.deltaTime * 2);
    }
    public void SecondCameraOn()
    {
        mainCamera.enabled = false;
        secondCamera.enabled = true;
    }
    public void CameraZoomOutPosition()
    {
        mainCamera.enabled = true;
        secondCamera.enabled= false;
        mainCamera.fieldOfView = fieldOfViewZoomOut;
        offset = zoomOutAngle;
        transform.position = positionZOut;
        transform.rotation = Quaternion.Euler(rotationZOut);
        //transform.LookAt(player);
    }

    public void LakeLandscape()
    {
        lakeCamera.enabled = true;
        mainCamera.enabled = false;
        StartCoroutine(MovingLakeCamera());
    }
    public void LakeLandscapeOff()
    {
;       lakeCamera.enabled = false;
        mainCamera.enabled = true;
    }
    IEnumerator MovingLakeCamera()
    {

        while (lakeCamera.transform.position != lakeCameraMovingSpot.position) //Vector3(-171.15f,16f,196.3f) TREE
        {
            lakeCamera.transform.position = Vector3.MoveTowards(lakeCamera.transform.position, lakeCameraMovingSpot.position, movingSpeed * Time.deltaTime);


            yield return null;
        }


    }

}
