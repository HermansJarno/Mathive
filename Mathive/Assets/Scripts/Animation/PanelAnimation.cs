using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    	public void BeginLerp(Vector3 begin, Vector3 end, float speed, float alpha, float delay, float numberOfSteps)
	{
		//We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
		/* _startPosition = begin;
		_endPosition = end;
		_endPositionExtra1 = new Vector3(_endPosition.x, _endPosition.y + extraDistance1, _endPosition.z);
		_endPositionExtra2 = new Vector3(_endPosition.x, _endPosition.y + extraDistance2, _endPosition.z);
		_maxSpeed = speed;
		m_alpha = alpha;
		transparancy = gameObject.AddComponent<CanvasGroup>();
		transparancy.alpha = m_alpha;
		_alphaLerping = true;
		Invoke("startLerpDelayed", delay);*/
	}
}
