using UnityEngine;

public class Score : MonoBehaviour {

  int totalScore = 0;
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

  public ScoreUI SUI;

  private void Start()
  {
    SUI = GameObject.Find("UIScripts").GetComponent<ScoreUI>();
  }

  public void SetTargets(int blue, int yellow, int cyan, int green, int red, int magenta)
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
    tblue = blueScore.ToString() + "/" + targetBlue.ToString();
    tyellow = yellowScore.ToString() + "/" + targetYellow.ToString();
    tcyan = cyanScore.ToString() + "/" + targetCyan.ToString();
    tgreen = greenScore.ToString() + "/" + targetGreen.ToString();
    tred = redScore.ToString() + "/" + targetRed.ToString();
    tmagenta = magentaScore.ToString() + "/" + targetMagenta.ToString();
    
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
  }

  int CountScore(int newScore, int oldScore, int target)
  {
    oldScore += newScore;
    if (oldScore >= target) oldScore = target;
    return oldScore;
  }
}
