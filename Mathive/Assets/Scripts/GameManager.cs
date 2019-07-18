using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	private int _level = 1;
	private int _starsEarned = 0;
	[SerializeField] private int _score = 0;
	[SerializeField] private int _goalScore = 0;
	[SerializeField] private int _movesLeft = 30;

	[SerializeField] private int _targetNumber = 1;
	[SerializeField] private int _targetQuantityOfNumber = 20;
	[SerializeField] private int _currentQuantityOfNumber = 0;

	Score scoreController;
	private Text _movesLeftText;
	private GameObject popUpNoMovesLeft, popUpLevelDone;
	private LevelController levelController;
	private bool gameEnded = false;

	private void Awake()
	{
		scoreController = GameObject.Find("Scripts").GetComponent<Score>();
		levelController = GameObject.Find("LevelData").GetComponent<LevelController>();
		_level = levelController.level;
		MovesLeft = _movesLeft;
		popUpNoMovesLeft = GameObject.Find("PopUpOutOfMoves");
		popUpLevelDone = GameObject.Find("PopUpLevelDone");
	}

	public void SetLevel(int level, int goalScore, int movesLeft)
	{
		_level = level;
		_goalScore = goalScore;
		_movesLeft = movesLeft;
	}

	public void SetTargets(List<int> targets, int movesLeft, List<int> scoreTargets)
	{
		scoreController.SetTargets(targets[0], targets[1], targets[2], targets[3], targets[4], targets[5]);
		MovesLeft = movesLeft;
		scoreController.setTargetScores(scoreTargets);
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
		get {
			if(levelController == null) levelController = GameObject.Find("LevelData").GetComponent<LevelController>();
			return _level = levelController.level; 
		}
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

	public int StarsEarned
	{
		get { return _starsEarned; }
		set { _starsEarned = value; }
	}

	public int GoalScore
	{
		get { return _goalScore; }
		set { _goalScore = value; }
	}

	public bool GameEnded
	{
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
			if(_movesLeft == 5){
				new MessageController().showRemainingMoves();
			}
			checkIfTargetIsDone(value);
			_movesLeftText.text = _movesLeft.ToString();
		}
	}

	void checkIfTargetIsDone(int value){
		if(value > 0){
			if (scoreController.AllTargetsDone)
			{
				showPanel(popUpLevelDone);
				levelController.UpdateHighestLevel(_level + 1);
				gameEnded = true;
			}
		}else if(value == 0){
			if (scoreController.AllTargetsDone)
			{
				showPanel(popUpLevelDone);
				levelController.UpdateHighestLevel(_level + 1);
			}else{
				levelController.LowerNumberOfLifes();
				popUpNoMovesLeft.transform.Find("Heart").GetComponent<HeartUI>().UpdateNumberOfLifes();
				showPanel(popUpNoMovesLeft);
			}
			gameEnded = true;
		}
	}

	void showPanel(GameObject panel){
		panel.AddComponent<PanelAnimation>();
		panel.GetComponent<PanelAnimation>().yOffset = -popUpNoMovesLeft.transform.localPosition.y;
	}
}
