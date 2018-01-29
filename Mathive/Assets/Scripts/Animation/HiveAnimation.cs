using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiveAnimation : MonoBehaviour {

  public float timeTakenDuringLerp = 1f;
  public float timeTakenDuringLerpScale = 0.5f;

  /// <summary>
  /// How far the object should move when 'space' is pressed
  /// </summary>
  public float m_speed = 10;
  public float m_alpha = 1;

  //Whether we are currently interpolating or not
  private bool _isLerping;
  private bool _alphaLerping = false;

  //The start and finish positions for the interpolation
  private Vector3 _totalScale;
  private Vector3 _midScale;
  private CanvasGroup transparancy;

  public Image image;

  //The Time.time value when we started the interpolation
  private float _timeStartedLerping;

  public void BeginScale(float scale, float speed, Sprite background)
  {
    _isLerping = true;
    _timeStartedLerping = Time.time;
    image.sprite = background;
    m_speed = speed;
    _totalScale = transform.localScale;
    _midScale = transform.localScale * scale;
    transparancy = gameObject.AddComponent<CanvasGroup>();
    transparancy.alpha = m_alpha;
    _alphaLerping = true;
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
      if (_alphaLerping)
      {
        transparancy.alpha = Mathf.MoveTowards(1, 0, percentageComplete);
      }

      transform.localScale = Vector3.Lerp(_totalScale, _midScale, percentageCompleteS);  

      //When we've completed the lerp, we set _isLerping to false
      if (percentageComplete >= 1.0f)
      {
        transparancy.alpha = 0;
        Destroy(transparancy);
        _isLerping = false;
        Destroy(gameObject);
      }
    }

  }
}
