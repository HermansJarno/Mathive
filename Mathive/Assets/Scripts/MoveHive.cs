using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHive : MonoBehaviour
{

	public float timeTakenDuringLerp = 1f;
	public float timeTakenDuringLerpScale = 0.5f;
	public float timeToWaitToDrop = 0.16f;

	float m_speed = 3;//Don't touch this
	float _maxSpeed = 20;//This is the maximum speed that the object will achieve
	float _acceleration = 10;//How fast will object reach a maximum speed

	/// <summary>
	/// How far the object should move when 'space' is pressed
	/// </summary>
	//public float m_speed = 10;
	public float m_speedExtra = 20;
	public float m_alpha = 0;
	float extraDistance1 = 5;
	float extraDistance2 = 2;
	float extraDistance3 = 0.5f;

	//Whether we are currently interpolating or not
	private bool _isLerping;
	private bool _startLerp;
	private bool _alphaLerping = false;
	private bool _isExtraDistance1Up = false;
	private bool _isExtraDistance2Up = false;
	private bool _isExtraDistance3Up = false;
	private bool _isExtraDistance1Down = false;
	private bool _isExtraDistance2Down = false;
	private bool _isExtraDistance3Down = false;

	//ExtraDistance1
	private Vector3 _endPositionExtra1;

	//ExtraDistance2
	private Vector3 _endPositionExtra2;

	//ExtraDistance3
	private Vector3 _endPositionExtra3;

	//The start and finish positions for the interpolation
	private Vector3 _startPosition;
	private Vector3 _endPosition;
	private Vector3 _totalScale;
	private Vector3 _midScale;
	private Vector3 _midScale2;
	private Vector3 _midScale3;

	private CanvasGroup transparancy;

	//The Time.time value when we started the interpolation
	private float _timeStartedLerping;

	public void BeginLerp(Vector3 begin, Vector3 end, float speed, float delay, float numberOfSteps)
	{
		//We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
		_startPosition = begin;
		_endPosition = end;
		_endPositionExtra1 = new Vector3(_endPosition.x, _endPosition.y + extraDistance1, _endPosition.z);
		_endPositionExtra2 = new Vector3(_endPosition.x, _endPosition.y + extraDistance2, _endPosition.z);
		_endPositionExtra3 = new Vector3(_endPosition.x, _endPosition.y + extraDistance3, _endPosition.z);
		_maxSpeed = speed;
		_totalScale = transform.localScale;
		_midScale = new Vector3(transform.localScale.x - (numberOfSteps / 100), transform.localScale.y + (numberOfSteps / 100), transform.localScale.z);
		_midScale2 = new Vector3(transform.localScale.x - (numberOfSteps / 500), transform.localScale.y + (numberOfSteps / 500), transform.localScale.z);
		_midScale3 = new Vector3(transform.localScale.x - (numberOfSteps / 1000), transform.localScale.y + (numberOfSteps / 1000), transform.localScale.z);


		Invoke("startLerpDelayed", delay);
	}

	public void BeginLerp(Vector3 begin, Vector3 end, float speed, float alpha, float delay, float numberOfSteps)
	{
		//We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
		_startPosition = begin;
		_endPosition = end;
		_endPositionExtra1 = new Vector3(_endPosition.x, _endPosition.y + extraDistance1, _endPosition.z);
		_endPositionExtra2 = new Vector3(_endPosition.x, _endPosition.y + extraDistance2, _endPosition.z);
		_endPositionExtra3 = new Vector3(_endPosition.x, _endPosition.y + extraDistance3, _endPosition.z);
		_maxSpeed = speed;
		_totalScale = transform.localScale;
		_midScale = new Vector3(transform.localScale.x - (numberOfSteps / 100), transform.localScale.y + (numberOfSteps / 100), transform.localScale.z);
		_midScale2 = new Vector3(transform.localScale.x - (numberOfSteps / 500), transform.localScale.y + (numberOfSteps / 500), transform.localScale.z);
		_midScale3 = new Vector3(transform.localScale.x - (numberOfSteps / 1000), transform.localScale.y + (numberOfSteps / 1000), transform.localScale.z);
		m_alpha = alpha;
		transparancy = gameObject.AddComponent<CanvasGroup>();
		transparancy.alpha = m_alpha;
		_alphaLerping = true;
		Invoke("startLerpDelayed", delay);
	}

	private void startLerpDelayed()
	{
		_timeStartedLerping = Time.time;
		_startLerp = true;
	}

	void FixedUpdate()
	{
		if (_startLerp)
		{
			float timeSinceStarted = (Time.time - _timeStartedLerping);

			if (timeSinceStarted >= timeToWaitToDrop)
			{
				_startLerp = false;
				_timeStartedLerping = Time.time;
				_isLerping = true;
			}
		}
		if (_isLerping)
		{
			if(m_speed < _maxSpeed){
				m_speed = m_speed + (_acceleration * Time.deltaTime);
			}
			float timeSinceStarted = (Time.time - _timeStartedLerping) * m_speed;
			float percentageComplete = timeSinceStarted / timeTakenDuringLerp;
			float percentageCompleteS = timeSinceStarted / timeTakenDuringLerpScale;

			//Perform the actual lerping.  Notice that the first two parameters will always be the same
			//throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
			//to start another lerp)
			transform.localPosition = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);
			if (_alphaLerping)
			{
				transparancy.alpha = percentageComplete;
			}

			if (percentageComplete <= 0.95f)
			{
				transform.localScale = Vector3.Lerp(_totalScale, _midScale, percentageCompleteS);
			}
			else
			{
				transform.localScale = Vector3.Lerp(_midScale, _totalScale, percentageCompleteS);
			}

			//When we've completed the lerp, we set _isLerping to false
			if (percentageComplete >= 1.0f)
			{
				if (_alphaLerping)
				{
					transparancy.alpha = 1f;
					Destroy(transparancy);
				}
				_timeStartedLerping = Time.time;
				_isLerping = false;
				_isExtraDistance1Up = true;
			}
		}
		if (_isExtraDistance1Up)
		{
			if (m_speed < _maxSpeed)
			{
				m_speed = m_speed + _acceleration * Time.deltaTime;
			}
			float timeSinceStarted = (Time.time - _timeStartedLerping) * m_speedExtra;
			float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

			transform.localPosition = Vector3.Lerp(_endPosition, _endPositionExtra1, percentageComplete);

			if (percentageComplete >= 1.0f)
			{
				_isExtraDistance1Up = false;
				_isExtraDistance1Down = true;
				_timeStartedLerping = Time.time;
			}
		}
		if (_isExtraDistance1Down)
		{
			if (m_speed < _maxSpeed)
			{
				m_speed = m_speed + _acceleration * Time.deltaTime;
			}
			float timeSinceStarted = (Time.time - _timeStartedLerping) * m_speedExtra;
			float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

			transform.localPosition = Vector3.Lerp(_endPositionExtra1, _endPosition, percentageComplete);

			if (percentageComplete >= 1.0f)
			{
				_isExtraDistance1Down = false;
				_isExtraDistance2Up = true;
				_timeStartedLerping = Time.time;
			}
		}
		if (_isExtraDistance2Up)
		{
			if (m_speed < _maxSpeed)
			{
				m_speed = m_speed + _acceleration * Time.deltaTime;
			}
			float timeSinceStarted = (Time.time - _timeStartedLerping) * m_speedExtra;
			float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

			transform.localPosition = Vector3.Lerp(_endPosition, _endPositionExtra2, percentageComplete);

			if (percentageComplete >= 1.0f)
			{
				_isExtraDistance2Up = false;
				_isExtraDistance2Down = true;
				_timeStartedLerping = Time.time;
			}
		}
		if (_isExtraDistance2Down)
		{
			if (m_speed < _maxSpeed)
			{
				m_speed = m_speed + _acceleration * Time.deltaTime;
			}
			float timeSinceStarted = (Time.time - _timeStartedLerping) * m_speedExtra;
			float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

			transform.localPosition = Vector3.Lerp(_endPositionExtra2, _endPosition, percentageComplete);

			if (percentageComplete >= 1.0f)
			{
				_isExtraDistance2Down = false;
				_isExtraDistance3Up = true;
				_timeStartedLerping = Time.time;
			}
		}
		if (_isExtraDistance3Up)
		{
			if (m_speed < _maxSpeed)
			{
				m_speed = m_speed + _acceleration * Time.deltaTime;
			}
			float timeSinceStarted = (Time.time - _timeStartedLerping) * m_speedExtra;
			float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

			transform.localPosition = Vector3.Lerp(_endPosition, _endPositionExtra3, percentageComplete);

			if (percentageComplete >= 1.0f)
			{
				_isExtraDistance3Up = false;
				_isExtraDistance3Down = true;
				_timeStartedLerping = Time.time;
			}
		}
		if (_isExtraDistance3Down)
		{
			if (m_speed < _maxSpeed)
			{
				m_speed = m_speed + _acceleration * Time.deltaTime;
			}
			float timeSinceStarted = (Time.time - _timeStartedLerping) * m_speedExtra;
			float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

			transform.localPosition = Vector3.Lerp(_endPositionExtra3, _endPosition, percentageComplete);

			if (percentageComplete >= 1.0f)
			{
				_isExtraDistance3Down = false;
				_timeStartedLerping = Time.time;

				Destroy(this);
			}
		}

	}

}
