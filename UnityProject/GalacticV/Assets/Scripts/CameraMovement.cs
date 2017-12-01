using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 0f;
    private int boundary = 15;
    private float xMax;
    private float yMin;

    private int screenWidth;
    private int screenHeight;

    // Use this for initialization
    void Start()
    {
        this.screenHeight = Screen.height;
        this.screenWidth = Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
            if(transform.position.y >= 20f)
            {
                transform.position = new Vector3(transform.position.x, 20f, transform.position.z);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
            if (transform.position.x <= -10f)
            {
                transform.position = new Vector3(-10f, transform.position.y, transform.position.z);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);
            if (transform.position.y <= -20f)
            {
                transform.position = new Vector3(transform.position.x, -20f, transform.position.z);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
            if (transform.position.x >= 30f)
            {
                transform.position = new Vector3(30f,transform.position.y, transform.position.z);
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.orthographicSize -= 1f;
            if(Camera.main.orthographicSize < 1f)
            {
                Camera.main.orthographicSize = 1f;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize += 1f;
            if(Camera.main.orthographicSize > 25f)
            {
                Camera.main.orthographicSize = 25f;
            }
        }
        //Mouse Movement
       /* if (Input.mousePosition.x > screenWidth - boundary)
        {
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime); // move on +X axis
            if (transform.position.x >= 30f)
            {
                transform.position = new Vector3(30f, transform.position.y, transform.position.z);
            }
        }
        if (Input.mousePosition.x < 0 + boundary)
        {
            transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
            if (transform.position.x <= -10f)
            {
                transform.position = new Vector3(-10f, transform.position.y, transform.position.z);
            }
        }
        if (Input.mousePosition.y > screenHeight - boundary)
        {
            transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
            if (transform.position.y >= 20f)
            {
                transform.position = new Vector3(transform.position.x, 20f, transform.position.z);
            }
        }
        if (Input.mousePosition.y < 0 + boundary)
        {
            transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);
            if (transform.position.y <= -20f)
            {
                transform.position = new Vector3(transform.position.x, -20f, transform.position.z);
            }
        }*/

    }

    public void SetCameraChangeTurn(string team)
    {
        Vector3 newPos;
        if(team == "Blue")
        {
            newPos = new Vector3(0f, 0f, -10f);
        }
        else
        {
            newPos = new Vector3(25f, 0f, -10f);
        }
        Camera.main.gameObject.transform.position = newPos;
    }
}
