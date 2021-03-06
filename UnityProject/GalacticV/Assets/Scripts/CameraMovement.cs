﻿using System.Collections;
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

    private Vector3 lastPositionRed;
    private Vector3 lastPositonBlue;

    private float lastZoomBlue;
    private float lastZoomRed;

    public Vector3 LastPositionRed
    {
        get { return this.lastPositionRed; }
        set { this.lastPositionRed = value; }
    }
    public Vector3 LastPositionBlue
    {
        get { return this.lastPositonBlue; }
        set { this.lastPositonBlue = value; }
    }

    public float LastZoomBlue
    {
        get { return this.lastZoomBlue; }
        set { this.lastZoomBlue = value; }
    }

    public float LastZoomRed
    {
        get { return this.lastZoomRed; }
        set { this.lastZoomRed = value; }
    }

    // Use this for initialization
    void Start()
    {
        this.screenHeight = Screen.height;
        this.screenWidth = Screen.width;
        lastPositionRed = new Vector3(53f, 0, -10);
        this.lastZoomRed = 5f;
        this.lastZoomBlue = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        GameObject pause = GameObject.Find("PauseCanvas").transform.Find("PauseMenu").gameObject;
        if(!pause.activeSelf)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
                if (transform.position.y >= 35f)
                {
                    transform.position = new Vector3(transform.position.x, 35f, transform.position.z);
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
                if (transform.position.y <= -35f)
                {
                    transform.position = new Vector3(transform.position.x, -35f, transform.position.z);
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
                if (transform.position.x >= 60f)
                {
                    transform.position = new Vector3(60f, transform.position.y, transform.position.z);
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Camera.main.orthographicSize -= 1f;
                if (Camera.main.orthographicSize < 1f)
                {
                    Camera.main.orthographicSize = 1f;
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                Camera.main.orthographicSize += 1f;
                if (Camera.main.orthographicSize > 25f)
                {
                    Camera.main.orthographicSize = 25f;
                }
            }
            //Mouse Movement
            if (Input.mousePosition.x > screenWidth - boundary)
            {
                transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime); // move on +X axis
                if (transform.position.x >= 60f)
                {
                    transform.position = new Vector3(60f, transform.position.y, transform.position.z);
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
                if (transform.position.y >= 35f)
                {
                    transform.position = new Vector3(transform.position.x, 35f, transform.position.z);
                }
            }
            if (Input.mousePosition.y < 0 + boundary)
            {
                transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);
                if (transform.position.y <= -35f)
                {
                    transform.position = new Vector3(transform.position.x, -35f, transform.position.z);
                }
            }
        }
    }

    public void SetCameraChangeTurn(string team)
    {
        Vector3 newPos;
        float newSize;
        if(team == "Blue")
        {
            lastPositionRed = Camera.main.gameObject.transform.position;
            lastZoomRed = Camera.main.orthographicSize;
            newPos = lastPositonBlue;
            newSize = lastZoomBlue;
        }
        else
        {
            lastPositonBlue = Camera.main.gameObject.transform.position;
            newPos = lastPositionRed;
            lastZoomBlue = Camera.main.orthographicSize;
            newSize = lastZoomRed;
        }
        Camera.main.gameObject.transform.position = newPos;
        Camera.main.orthographicSize = newSize;
    }
}
