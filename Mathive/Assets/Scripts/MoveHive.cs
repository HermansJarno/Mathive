using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHive : MonoBehaviour {

  public float timeTakenDuringLerp = 1f;
  public float timeTakenDuringLerpScale = 0.5f;

  /// <summary>
  /// How far the object should move when 'space' is pressed
  /// </summary>
  public float m_speed = 10;

  //Whether we are currently interpolating or not
  private bool _isLerping;

  //The start and finish positions for the interpolation
  private Vector3 _startPosition;
  private Vector3 _endPosition;
  private Vector3 _totalScale;
  private Vector3 _midScale;

  //The Time.time value when we started the interpolation
  private float _timeStartedLerping;

  public void BeginLerp(Vector3 begin, Vector3 end, float speed)
  {
    _isLerping = true;
    _timeStartedLerping = Time.time;

    //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
    _startPosition = begin;
    _endPosition = end;
    m_speed = speed;
    _totalScale = transform.localScale;
    _midScale = transform.localScale / 2;
  }

  void FixedUpdate()
  {
    if (_isLerping)
    {
      float timeSinceStarted = (Time.time - _timeStartedLerping) * m_speed;
      float percentageComplete = timeSinceStarted / timeTakenDuringLerp;
      float percentageCompleteS = timeSinceStarted / timeTakenDuringLerpScale;

      //Perform the actual lerping.  Notice that the first two parameters will always be the same
      //throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
      //to start another lerp)
      transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

      if (percentageComplete <= 0.5)
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
        _isLerping = false;
        Destroy(this);
      }
    }

  }
}
