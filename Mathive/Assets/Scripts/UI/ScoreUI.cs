using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

  [SerializeField] float m_refWidth = 800f;
  [SerializeField] float m_refHeight = 1280f;
  [SerializeField] float m_yOffset = 125;
  [SerializeField] float m_xOffset = 150;
  [SerializeField] float extraScaleCorrection = 0.1f;

  public Text blueScore, redScore, greenScore, yellowScore, magentaScore, cyanScore;

  public GameObject[] TopScores;
  public GameObject[] BottomScores;
  public GameObject m_GridContainer;

  float m_scaleX = 0;
  float m_scaleY = 0;


  // Use this for initialization
  void Start () {
    SetupUI();
	}

  public void UpdateScores(string blue, string red, string green, string yellow, string magenta, string cyan)
  {
    blueScore.text = blue;
    greenScore.text = green;
    yellowScore.text = yellow;
    redScore.text = red;
    cyanScore.text = cyan;
    magentaScore.text = magenta;
  }

  void SetupUI()
  {
    RectTransform rtGrid = m_GridContainer.GetComponent<RectTransform>();
    float widthScreen = rtGrid.rect.width;
    Debug.Log(widthScreen);
    float heightScreen = rtGrid.rect.height;

    float ratio = widthScreen / heightScreen;
    Debug.Log(ratio);
    if (ratio < 0.55f)
    {
      // Calculate scale
      m_scaleX = (widthScreen / m_refWidth) - extraScaleCorrection;
      Debug.Log(m_scaleX);
      m_scaleY = (heightScreen / m_refHeight) -extraScaleCorrection;

      if (ratio < 0.45f)
      {
        m_scaleX -= extraScaleCorrection;
        m_scaleY -= extraScaleCorrection;
      }

      //m_yOffset *= m_scaleY;
      m_xOffset *= m_scaleX;

      TopScores[0].transform.localPosition = new Vector3(-(m_xOffset), TopScores[0].transform.localPosition.y, 0);
      TopScores[1].transform.localPosition = new Vector3(0, TopScores[1].transform.localPosition.y, 0);
      TopScores[2].transform.localPosition = new Vector3(m_xOffset, TopScores[2].transform.localPosition.y, 0);
      BottomScores[0].transform.localPosition = new Vector3(-(m_xOffset), BottomScores[0].transform.localPosition.y, 0);
      BottomScores[1].transform.localPosition = new Vector3(0, BottomScores[1].transform.localPosition.y, 0);
      BottomScores[2].transform.localPosition = new Vector3(m_xOffset, BottomScores[2].transform.localPosition.y, 0);
      foreach (GameObject scoreObj in TopScores)
      {
        scoreObj.GetComponent<RectTransform>().localScale = new Vector3(m_scaleX, m_scaleX, m_scaleX);
      }

      foreach (GameObject scoreObj in BottomScores)
      {
        scoreObj.GetComponent<RectTransform>().localScale = new Vector3(m_scaleX, m_scaleX, m_scaleX);
      }
    }
  }
}
