using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 20f;
    public Transform t_playerBody;
    float xRotation;
    //private Camera camera;
    public float cameraSize = 22f;
    public float scrollSize = 1000f;      // Speed of zoom
    public float minZoom = 20f;        // Minimum field of view (zoomed in)
    public float maxZoom = 60f;        // Maximum field of view (zoomed out)
                                       // public Camera camera;
                                       // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        float scrollZ = Input.GetAxis("Mouse ScrollWheel") * scrollSize * -1f * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, 0f, 70f);
        

        if (scrollZ != 0)
        {
            cameraSize = scrollZ + cameraSize;
            // Modify the field of view based on scroll input
            Camera.main.orthographicSize = cameraSize;

            // Clamp the field of view to stay within the min and max limits
            cameraSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        }

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        t_playerBody.Rotate(Vector3.up * mouseX);
    }
}
