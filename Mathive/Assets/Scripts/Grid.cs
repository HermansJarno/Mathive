using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

  string[] symbols = {"+","/", "*", "-" };
  Tile[,] m_Grid;
  Text goalNumber;
  public GameObject m_GridContainer;

	// Use this for initialization
	void Start () {
    InitializeGrid();
    goalNumber = GameObject.Find("GoalNumber").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void SetGoalNumber() {
    Stack<Tile> hives = new Stack<Tile>();
    string numberSequence = findNumber(3, hives, 0,0,"");
    int theGoalNumber = 0;
    foreach (char index in numberSequence)
    {
      
    }
    goalNumber.text = theGoalNumber.ToString();
  }

  string findNumber(int stepsLeft, Stack<Tile> currentHives, int lastIndexI, int lastIndexJ, string goalNumber)
  {
    if (stepsLeft == 0)
    {
      return goalNumber;
    }
    else
    {
      if (currentHives.Count == 0)
      {
        lastIndexI = Random.Range(0, m_Grid.GetLength(0) + 1);
        lastIndexJ = Random.Range(0, m_Grid.GetLength(1) + 1);
        currentHives.Push(m_Grid[lastIndexI, lastIndexJ]);
        goalNumber += currentHives.Peek().number;
      }
      else
      {
        List<int> iNext = new List<int>();
        List<int> jNext = new List<int>();
        for (int i = -1; i <= 1; i++)
        {
          iNext.Add(i);
          jNext.Add(i);
        }
        if (lastIndexI == m_Grid.GetLength(0))
        {
          iNext.Remove(1);
        }
        if (lastIndexJ == m_Grid.GetLength(1))
        {
          jNext.Remove(1);
        }
        if (lastIndexI == 0)
        {
          iNext.Remove(-1);
        }
        if (lastIndexJ == 0)
        {
          jNext.Remove(-1);
        }
        int tempI;
        int tempJ;

        do
        {
          int randomIndexI = Random.Range(0, iNext.Count);
          int randomIndexJ = Random.Range(0, jNext.Count);
          tempI = lastIndexI + iNext[randomIndexI];
          tempJ = lastIndexJ + jNext[randomIndexJ];
        } while ((tempI == lastIndexI) && (tempJ == lastIndexJ));

        if (!currentHives.Contains(m_Grid[tempI, tempJ]))
        {
          lastIndexI = tempI;
          lastIndexJ = tempJ;
          currentHives.Push(m_Grid[lastIndexI, lastIndexJ]);
          goalNumber += currentHives.Peek().number.ToString();
        }
        else
        {
          return findNumber(stepsLeft, currentHives, lastIndexI, lastIndexJ, goalNumber);
        }
      }
    }
    return findNumber(stepsLeft--, currentHives, lastIndexI, lastIndexJ, goalNumber);
  }

  void InitializeGrid()
  {
    int stepsSinceLastSymbol = 0;
    bool hiveUsed = false;
    int width, height;
    width = m_GridContainer.transform.childCount;
    height = m_GridContainer.transform.GetChild(0).childCount;
    m_Grid = new Tile[width, height];
    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        hiveUsed = false;
        int oneOrTwo = Random.Range(1, 3);
        if (stepsSinceLastSymbol == 0 && !hiveUsed)
        {
          int num = Random.Range(1, 10);
          m_Grid[i, j] = new Tile(m_GridContainer.transform.GetChild(i).GetChild(j).gameObject, num.ToString());
          m_GridContainer.transform.GetChild(i).GetChild(j).Find("Text").GetComponent<Text>().text = num.ToString();
          stepsSinceLastSymbol++;
          hiveUsed = true;
        }
        if (stepsSinceLastSymbol == 1 && !hiveUsed)
        {
          if (oneOrTwo == 1)
          {
            int num = Random.Range(1, 10);
            m_Grid[i, j] = new Tile(m_GridContainer.transform.GetChild(i).GetChild(j).gameObject, num.ToString());
            m_GridContainer.transform.GetChild(i).GetChild(j).Find("Text").GetComponent<Text>().text = num.ToString();
            stepsSinceLastSymbol++;
          }
          else
          {
            int num = Random.Range(0, 4);
            m_Grid[i, j] = new Tile(m_GridContainer.transform.GetChild(i).GetChild(j).gameObject, symbols[num].ToString());
            m_GridContainer.transform.GetChild(i).GetChild(j).Find("Text").GetComponent<Text>().text = symbols[num].ToString();
            stepsSinceLastSymbol = 0;
          }
          hiveUsed = true;
        }
        if (stepsSinceLastSymbol == 2 && !hiveUsed)
        {
          int num = Random.Range(0, 4);
          m_Grid[i, j] = new Tile(m_GridContainer.transform.GetChild(i).GetChild(j).gameObject, symbols[num].ToString());
          m_GridContainer.transform.GetChild(i).GetChild(j).Find("Text").GetComponent<Text>().text = symbols[num].ToString();
          stepsSinceLastSymbol = 0;
        }
      }
    }
  }

  public bool IsHiveNear(GameObject lastHive, GameObject nextHive)
  {
    bool hiveIsNear = false;
    int indexIlast = 0, indexINext = 0;
    int indexJlast = 0, indexJNext = 0;
    for (int i = 0; i < m_Grid.GetLength(0); i++)
    {
      for (int j = 0; j < m_Grid.GetLength(1); j++)
      {
        if (m_Grid[i, j].tileObj == lastHive)
        {
          indexIlast = i;
          indexJlast = j;
        }
        if (m_Grid[i, j].tileObj == nextHive)
        {
          indexINext = i;
          indexJNext = j;
        }
      }
    }

    if (((indexJlast + 1 == indexJNext) || (indexJlast - 1 == indexJNext) || (indexJlast == indexJNext)))
    {
      if(((indexIlast + 1 == indexINext) || (indexIlast - 1 == indexINext) || (indexIlast == indexINext)))
      hiveIsNear = true;
    }
    return hiveIsNear;
  }

  public bool isSequenceMatch(List<GameObject> hives)
  {
    bool match = false;
    return match;
  }
}

public class Tile
{
  public GameObject tileObj;
  public string number;
  public Tile(GameObject tileObject, string value)
  {
    tileObj = tileObject;
    tileObj.transform.Find("Text").GetComponent<Text>().text = value;
    number = value;
  }
}
