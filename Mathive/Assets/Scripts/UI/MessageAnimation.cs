using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageAnimation : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    private CanvasGroup transparancy;
    private float timeStartedLerping = 0;
    private float timeStartedDelerping = 0;
    private float timeTakenDuringLerp = 1f;
    float m_speed = 1.2f;
    float _maxSpeed = 20;

    float _acceleration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        transparancy = gameObject.AddComponent<CanvasGroup>();
        transparancy.alpha = 0f;
        timeStartedLerping = Time.time;
        startPosition = new Vector3(0,0,0);
        endPosition = GameObject.Find("MessageContainer").transform.localPosition;
    }

    // Update is called once per frame
    public void Update(){
        if(timeStartedLerping != 0){
            float timeSinceStarted = Time.time - timeStartedLerping;
            if(m_speed < _maxSpeed){
				m_speed = m_speed + (_acceleration * Time.deltaTime);
			}
            if(transparancy.alpha != 1f){
                float percentageComplete = (timeSinceStarted / timeTakenDuringLerp) * m_speed;
                if(percentageComplete < 1f){
                    transform.localPosition = Vector3.Lerp(startPosition, endPosition, percentageComplete);
                    transparancy.alpha = percentageComplete;
                }else{
                    transparancy.alpha = 1f;
                }
            }
            if(timeSinceStarted >= 1f && timeSinceStarted < 1.5f){
                timeStartedDelerping = Time.time;
            }
            if (timeSinceStarted >= 1.5f){
                timeSinceStarted = Time.time - timeStartedDelerping;
                float percentageComplete = (timeSinceStarted / timeTakenDuringLerp) * m_speed;
                float percentage = 1f - percentageComplete;
                transparancy.alpha = percentage;
            }
        }
    }
}
