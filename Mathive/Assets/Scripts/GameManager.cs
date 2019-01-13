using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

  private int _level = 1;
  [SerializeField] private int _score = 0;
  [SerializeField] private int _goalScore = 0;
  [SerializeField] private int _movesLeft = 30;

  [SerializeField] private int _targetNumber = 1;
  [SerializeField] private int _targetQuantityOfNumber = 20;
  [SerializeField] private int _currentQuantityOfNumber = 0;

  Score scoreController;
  private Text _movesLeftText;
  private GameObject popUpNoMovesLeft;
  private GridController gridController;
  private bool gameEnded = false;

  private void Start()
  {
    scoreController = GameObject.Find("Scripts").GetComponent<Score>();
    gridController = GameObject.Find("Scripts").GetComponent<GridController>();
    SetTargets();
    _level = GameObject.Find("LevelData").GetComponent<Level>()._Level;
    MovesLeft = _movesLeft;
    popUpNoMovesLeft = GameObject.Find("PopUpOutOfMoves");
    popUpNoMovesLeft.SetActive(false);

  }

  public void SetLevel(int level, int goalScore, int movesLeft)
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
    get { return _level = GameObject.Find("LevelData").GetComponent<Level>()._Level; }
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

  public bool GameEnded {
    get { return gameEnded; }
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
      if(value == 0)
      {
        gridController.InputAvailable = false;
        gameEnded = true;
        popUpNoMovesLeft.SetActive(true);
        Debug.Log("GAME ENDED");
      }
      _movesLeftText.text = _movesLeft.ToString();
    }
  }
}
