using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

  [SerializeField] private int _level = 1;
  [SerializeField] private float _timeLeft;
  [SerializeField] private float _maxTime;
  [SerializeField] private int _score = 0;
  [SerializeField] private int _goalScore = 0;
  [SerializeField] private int _movesLeft = 30;

  private Text _movesText;

  void SetLevel(int level, int maxTime, int goalScore)
  {
    _level = level;
    _maxTime = maxTime;
    _timeLeft = maxTime;
    _goalScore = goalScore;
  }

  bool IsLevelCompleted()
  {
    if (_score >= _goalScore)
    {
      return true;
    }
    else
    {
      return false;
    }
  }

  public float MaxTime
  {
    get { return _maxTime; }
    set { _maxTime = value; }
  }

  public float TimeLeft
  {
    get { return _timeLeft; }
    set { _timeLeft = value; }
  }

  public int Level
  {
    get { return _level; }
    set { _level = value; }
  }

  public int Score
  {
    get { return _score; }
    set { _score = value; }
  }

  public int GoalScore
  {
    get { return _goalScore; }
    set { _goalScore = value; }
  }

  public int MovesLeft
  {
    get { return _movesLeft; }
    set
    {
      if (_movesLeft > 0)
      {
        _movesLeft = value;
        if (_movesText == null)
        {
          _movesText = GameObject.Find("MovesLeft").GetComponent<Text>();
        }
        _movesText.text = _movesLeft.ToString();
      }
    }
  }
}
