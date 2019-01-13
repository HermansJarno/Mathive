using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float speed = 0.01f;
    float fadeOutSpeed = 0f;
    float lastYPoint = 0;
    private bool fadeOut = false;
    private GameObject levelContainer;
    bool movingUp = true;

    void Start()
    {
        levelContainer = GameObject.Find("Levels");
        lastYPoint = levelContainer.transform.position.y;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("right");
            transform.Translate(new Vector3(10, 0, 0) * Time.deltaTime * speed);
        }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("left");
            transform.Translate(new Vector3(-10, 0, 0) * Time.deltaTime * speed);
        }


        if (Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("Up");
            transform.Translate(new Vector3(0, -10, 0) * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("Down");
            transform.Translate(new Vector3(0, 10, 0) * Time.deltaTime * speed);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Get movement of the finger since last frame
            Vector3 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            levelContainer.transform.Translate(0, touchDeltaPosition.y * speed, 0);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (lastYPoint > levelContainer.transform.position.y)
            {
                movingUp = false;
            }
            else if (lastYPoint < levelContainer.transform.position.y)
            {
                movingUp = true;
            }
            if (lastYPoint != levelContainer.transform.position.y)
            {
                fadeOut = true;
                fadeOutSpeed = 0.1f;
                lastYPoint = levelContainer.transform.position.y;
            }
        }

        /* 
        if (fadeOut)
        {
            if (fadeOutSpeed > 0f)
            {
                fadeOutSpeed -= (Time.deltaTime / 2f);
                if (fadeOutSpeed < 0f)
                {
                    fadeOutSpeed = 0f;
                    fadeOut = false;
                }
            }
            if (movingUp)
            {
                levelContainer.transform.Translate(new Vector3(0f, -10f, 0f) * fadeOutSpeed);
            }else{
                levelContainer.transform.Translate(new Vector3(0f, 10f, 0f) * fadeOutSpeed);
            }
        }
        */
    }
}
