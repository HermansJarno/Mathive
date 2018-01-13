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

  [SerializeField] private int _targetNumber = 1;
  [SerializeField] private int _targetQuantityOfNumber = 20;
  [SerializeField] private int _currentQuantityOfNumber = 0;

  private Text _movesLeftText;

  //public void SetLevel(int level, int maxTime, int goalScore)
  //{
  //  _level = level;
  //  _maxTime = maxTime;
  //  _timeLeft = maxTime;
  //  _goalScore = goalScore;
  //}

  public void SetTargets(int targetNumber, int quantityNumber)
  {
    TargetNumber = targetNumber;
    TargetQuantity = quantityNumber;
  }

  public void GetNextTarget(int numberOfHives)
  {
    TargetNumber += _targetNumber;
    if (numberOfHives * 2 > _targetNumber) _targetQuantityOfNumber = Mathf.CeilToInt((numberOfHives * 2) / (_targetNumber / 1.5f));
    if (_targetQuantityOfNumber == 1)
    {
      _targetQuantityOfNumber = 2;
    }
  }

  public bool TargetIsCompleted()
  {
    if (_currentQuantityOfNumber >= _targetQuantityOfNumber)
    {
      return true;
    }
    return false;
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

  public int TargetNumber
  {
    get { return _targetNumber; }
    set
    {
      _targetNumber = value;
    }
  }

  public int TargetQuantity
  {
    get { return _targetQuantityOfNumber; }
    set { _targetQuantityOfNumber = value; }
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
      if (_movesLeftText == null)
      {
        _movesLeftText = GameObject.Find("MovesLeft").GetComponent<Text>();
      }
      if (_movesLeft > 1)
      {
        _movesLeft = value;
      }
      else if(value == 0)
      {
        Debug.Log("GAME ENDED");
      }
      _movesLeftText.text = _movesLeft.ToString();
    }
  }
}
