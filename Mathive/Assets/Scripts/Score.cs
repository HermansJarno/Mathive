using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Score : MonoBehaviour
{

	int currentScore = 0;
	int blueScore = 0;
	int yellowScore = 0;
	int redScore = 0;
	int greenScore = 0;
	int magentaScore = 0;
	int cyanScore = 0;

	int targetBlue = 0;
	int targetYellow = 0;
	int targetCyan = 0;
	int targetGreen = 0;
	int targetRed = 0;
	int targetMagenta = 0;

	int numberOfStarsEarned = 0;

	List<int> targetScores = new List<int>();

	bool allTargetsDone = false;

	private Text scoreText;

	private GameManager gameManager;

	public ScoreUI SUI;

	private void Start()
	{
		scoreText = GameObject.Find("Score").GetComponent<Text>();
		SUI = GameObject.Find("UIScripts").GetComponent<ScoreUI>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	public void setTargetScores(List<int> scores){
		targetScores = scores;
	}

	public void SetTargets(int blue, int yellow, int red, int cyan, int magenta, int green)
	{
		targetBlue = blue;
		targetYellow = yellow;
		targetCyan = cyan;
		targetGreen = green;
		targetRed = red;
		targetMagenta = magenta;

		UpdateUI();
	}

	void UpdateUI()
	{
		string tblue, tyellow, tcyan, tgreen, tred, tmagenta;
		/* 
		tblue = blueScore.ToString() + "/" + targetBlue.ToString();
		tyellow = yellowScore.ToString() + "/" + targetYellow.ToString();
		tcyan = cyanScore.ToString() + "/" + targetCyan.ToString();
		tgreen = greenScore.ToString() + "/" + targetGreen.ToString();
		tred = redScore.ToString() + "/" + targetRed.ToString();
		tmagenta = magentaScore.ToString() + "/" + targetMagenta.ToString();*/

		tblue = (targetBlue - blueScore).ToString();
		tyellow = (targetYellow - yellowScore).ToString();
		tcyan = (targetCyan - cyanScore).ToString();
		tgreen = (targetGreen - greenScore).ToString();
		tred = (targetRed - redScore).ToString();
		tmagenta = (targetMagenta - magentaScore).ToString();

		//if(SUI = null) SUI = GameObject.Find("UIScripts").GetComponent<ScoreUI>();
		SUI.UpdateScores(tblue, tred, tgreen, tyellow, tmagenta, tcyan);
	}

	public void UpdateScores(int blue, int yellow, int cyan, int green, int red, int magenta)
	{
		blueScore = CountScore(blue, blueScore, targetBlue);
		yellowScore = CountScore(yellow, yellowScore, targetYellow);
		redScore = CountScore(red, redScore, targetRed);
		greenScore = CountScore(green, greenScore, targetGreen);
		cyanScore = CountScore(cyan, cyanScore, targetCyan);
		magentaScore = CountScore(magenta, magentaScore, targetMagenta);
		UpdateUI();
		checkIfTargetsAreDone();
	}

	public void CalculateScore(List<Hive> hives)
	{
		int numberOfBlue = 0, numberOfRed = 0, numberOfGreen = 0, numberOfYellow = 0, numberOfCyan = 0, numberOfMagenta = 0;

		foreach (Hive hive in hives)
		{
			switch (hive.GetHiveType)
			{
				case HiveType.red:
					numberOfRed++;
					break;
				case HiveType.green:
					numberOfGreen++;
					break;
				case HiveType.blue:
					numberOfBlue++;
					break;
				case HiveType.yellow:
					numberOfYellow++;
					break;
				case HiveType.cyan:
					numberOfCyan++;
					break;
				case HiveType.magenta:
					numberOfMagenta++;
					break;
			}
		}
		UpdateScores(numberOfBlue, numberOfYellow, numberOfCyan, numberOfGreen, numberOfRed, numberOfMagenta);

		int resultNumber = 0;
		int macht = hives.Count;
		resultNumber = 0;

		int tempScore = gameManager.Score;
		resultNumber = (int) Mathf.Pow(3, macht);
		if(gameManager.MovesLeft != 6){
			// scores update first before movesLeft, so we are actually checking 5 moves left.
			new MessageController().processScore(resultNumber);
		}

		resultNumber += tempScore;
		scoreText.text = resultNumber.ToString();
		UpdateScoreSlider(resultNumber);
		SUI.UpdateNumberOfStarsEarned(numberOfStarsEarned);
		gameManager.Score = resultNumber;
	}

	void UpdateScoreSlider(int resultNumber){
		if(resultNumber < targetScores[0]){
			SUI.UpdateGlobalScore(0, targetScores[0], resultNumber);
		} else if(resultNumber >= targetScores[0] && resultNumber < targetScores[1]){
			numberOfStarsEarned = numberOfStarsEarned == 0 ? 1 : numberOfStarsEarned;
			SUI.UpdateGlobalScore(targetScores[0], targetScores[1], resultNumber);
		} else if(resultNumber >= targetScores[1] && resultNumber < targetScores[2]){
			numberOfStarsEarned = numberOfStarsEarned <= 1 ? 2 : numberOfStarsEarned;
			SUI.UpdateGlobalScore(targetScores[1], targetScores[2], resultNumber);
		} else {
			SUI.UpdateGlobalScore(0, resultNumber, 0);
			numberOfStarsEarned = numberOfStarsEarned <= 2 ? 3 : numberOfStarsEarned;
		}
		gameManager.StarsEarned = numberOfStarsEarned;
	}

	int CountScore(int newScore, int oldScore, int target)
	{
		oldScore += newScore;
		if (oldScore >= target) oldScore = target;
		return oldScore;
	}

	void checkIfTargetsAreDone()
	{
		if (blueScore == targetBlue && yellowScore == targetYellow && redScore == targetRed && greenScore == targetGreen && cyanScore == targetCyan && magentaScore == targetMagenta)
		{
			allTargetsDone = true;
		}
	}

	public bool AllTargetsDone{
		get{
			return allTargetsDone;
		}
	}

}
