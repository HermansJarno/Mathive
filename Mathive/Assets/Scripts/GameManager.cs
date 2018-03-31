using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

  [SerializeField] private int _level = 1;
  [SerializeField] private int _score = 0;
  [SerializeField] private int _goalScore = 0;
  [SerializeField] private int _movesLeft = 30;

  [SerializeField] private int _targetNumber = 1;
  [SerializeField] private int _targetQuantityOfNumber = 20;
  [SerializeField] private int _currentQuantityOfNumber = 0;

  Score scoreController;
  private Text _movesLeftText;

  private void Start()
  {
    scoreController = GameObject.Find("Scripts").GetComponent<Score>();
    SetTargets();
    //Scene scene = SceneManager.GetActiveScene();
    //string levelNr = scene.name.Replace("Level ", "");
    //_level = int.Parse(levelNr);

    _level = GameObject.Find("LevelData").GetComponent<Level>()._Level;
  }

  public void SetLevel(int level, int maxTime, int goalScore, int movesLeft)
  {
    _level = level;
    _goalScore = goalScore;
    _movesLeft = movesLeft;
  }

  public void SetTargets()
  {
    //TargetNumber = targetNumber;
    //TargetQuantity = quantityNumber;

    //make this dynamic later point
    scoreController.SetTargets(10, 10, 10, 10, 10, 10);
  }

  public bool TargetIsCompleted()
  {
    if (_currentQuantityOfNumber >= _targetQuantityOfNumber)
    {
      return true;
    }
    return false;
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
      if (_movesLeft >= 1)
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
