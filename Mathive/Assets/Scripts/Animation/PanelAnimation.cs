using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimation : MonoBehaviour
{
	public float yOffset;
	public float _maxSpeed = 4;//This is the maximum speed that the object will achieve
	public float _acceleration = 1;//How fast will object reach a maximum speed

	float timeTakenDuringLerp = 1f;
	float _speed = 1;//Don't touch this
	float _timeToWaitToDrop = 0.1f;
	float _timeStartedLerping;
	bool _startLerp = false;
	bool _isLerping = false;
	Vector3 _startPos, _endPos;

    // Start is called before the first frame update
    void Start()
    {
		_startPos = transform.localPosition;
		_endPos = new Vector3(_startPos.x, _startPos.y + yOffset, _startPos.x);
		_timeStartedLerping = Time.time;
		_startLerp = true;
    }

    // Update is called once per frame
    void Update()
    {
		if (_startLerp)
		{
			float timeSinceStarted = (Time.time - _timeStartedLerping);

			if (timeSinceStarted >= _timeToWaitToDrop)
			{
				_startLerp = false;
				_timeStartedLerping = Time.time;
				_isLerping = true;
			}
		}
		if (_isLerping)
		{
			if (_speed < _maxSpeed)
			{
				_speed = _speed + (_acceleration * Time.deltaTime);
			}
			float timeSinceStarted = (Time.time - _timeStartedLerping) * _speed;
			float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

			//Perform the actual lerping.  Notice that the first two parameters will always be the same
			//throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
			//to start another lerp)
			transform.localPosition = Vector3.Lerp(_startPos, _endPos, percentageComplete);

			//When we've completed the lerp, we set _isLerping to false
			if (percentageComplete >= 1.0f)
			{
				_timeStartedLerping = Time.time;
				_isLerping = false;
				Destroy(this);
			}
		}
    }
}
