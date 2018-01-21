﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

  public GameObject m_GridContainer;
  [SerializeField] float m_refWidth = 800f;
  [SerializeField] float m_refHeight = 1280;
  [SerializeField] float xHiveOffset = 85;
  [SerializeField] float yRowOffset = 50;
  [SerializeField] float yHiveOffset = 100;
  [SerializeField] float lerpSpeed = 4;
  [SerializeField] float offsetTop = 200;

  [SerializeField] private Vector3[,] m_HivePositions;
  [SerializeField] private Hive[,] m_Grid;
  private GridLevels m_GridLevels = new GridLevels();
  private Text Score;
  
  private GameObject hivePrefab;
  private GameObject rowPrefab;
  private GameManager GM;

  float distanceBetweenHives = Mathf.Infinity;
  [SerializeField] float m_scaleX;
  [SerializeField] float m_scaleY;
  float m_midScale;
  [SerializeField] float extraScale = 0.1f;

  private int m_colums, m_rows;
  private int midNumber;


  // Use this for initialization
  void Start () {
    Score = GameObject.Find("Score").GetComponent<Text>();
    hivePrefab = Resources.Load("Hive") as GameObject;
    rowPrefab = Resources.Load("Row_") as GameObject;
    GM = GameObject.Find("Scripts").GetComponent<GameManager>();

    Debug.Log("Number of rows: " + m_GridLevels.Levels[GM.Level-1][0]);
    Debug.Log("number of columns: " + m_GridLevels.Levels[GM.Level-1][1]);

    InitializeGrid(m_GridLevels.Levels[GM.Level -1][0], m_GridLevels.Levels[GM.Level -1][1]);
  }

  void InitializeGrid(int colums, int rows)
  {
    // Get current width and height of Screen
    RectTransform rtGrid = m_GridContainer.GetComponent<RectTransform>();
    float widthScreen = rtGrid.rect.width;
    float heightScreen = rtGrid.rect.height;

    // Calculate scale
    m_scaleX = (widthScreen / m_refWidth);
    m_scaleY = (heightScreen / m_refHeight);

    m_scaleX = (m_scaleX + m_scaleY) / 2;

    yRowOffset *= m_scaleX;

    xHiveOffset *= m_scaleX;
    yHiveOffset *= m_scaleX;
    float width = xHiveOffset * colums;
    float height = yHiveOffset * rows;

    float xPosition = -(width / 2);
    float yPosition = -(height / 2);

    m_Grid = new Hive[colums, rows];
    m_HivePositions = new Vector3[colums, rows];
    m_colums = colums;
    m_rows = rows;

    for (int i = 0; i < colums; i++)
    {
      if ((i % 2) == 0)
      {
        InstantiateRows(i + 1, yRowOffset);
      }
      else
      {
        InstantiateRows(i + 1, 0);
      }
    }

    // Create The Positions
    for (int i = 0; i < colums; i++)
    {
      for (int j = 0; j < rows; j++)
      {
        m_HivePositions[i, j] = new Vector3(xPosition + (xHiveOffset / 2), yPosition, 0);
        yPosition += yHiveOffset;
      }
      xPosition += xHiveOffset;
      yPosition = -(height / 2);
    }

    //Fill the empty Hives
    for (int i = 0; i < colums; i++)
    {
      for (int j = 0; j < rows; j++)
      {
        for (int r = 0; r < m_GridLevels.EmptyHivesLevels[GM.Level - 1][i].Length; r++)
        {
          //first the level, then the row, then check for index and validate them with J
          if (m_GridLevels.EmptyHivesLevels[GM.Level - 1][i][r] == (j+1))
          {
            m_Grid[i, j] = InstantiateHive(i, j);
            m_Grid[i, j].SetHive(0, i, j);
          }
        }

      }
    }

    // Generate the hives
    for (int i = 0; i < colums; i++)
    {
      for (int j = 0; j < rows; j++)
      {
        if (m_Grid[i, j] == null)
        {
          int num = Random.Range(1, 6);
          m_Grid[i, j] = InstantiateHive(i, j);
          m_Grid[i, j].SetHive(num, i, j);

          if (j - 1 > 0)
          {
            float dist = Vector3.Distance(m_Grid[i, j].transform.position, m_Grid[i, j - 1].transform.position);
            if (distanceBetweenHives > dist)
            {
              distanceBetweenHives = Mathf.Ceil(dist);
            }
          }
        }
      }
    }
    CalculateMidNumber();
  }

  void SetIndexToZero(int x, int y)
  {
    m_Grid[x, y].SetHive(0, x, y);
  }

  public bool IsHiveNear(Hive lastHive, Hive nextHive)
  {
    bool hiveIsNear = false;

    if (((lastHive.X + 1 == nextHive.X) || (lastHive.X - 1 == nextHive.X) || (lastHive.X == nextHive.X)))
    {
      if (((lastHive.Y + 1 == nextHive.Y) || (lastHive.Y - 1 == nextHive.Y) || (lastHive.Y == nextHive.Y)))
      { 
        if (Mathf.Round(Vector3.Distance(lastHive.transform.position, nextHive.transform.position)) == distanceBetweenHives)
        {
          hiveIsNear = true;
        }
      }
    }
    return hiveIsNear;
  }

  public void CalculateScore(List<Hive> hives)
  {
    int resultNumber = 0;
    int macht = hives.Count;
    int currentNumber = hives[0].Value;
    resultNumber = currentNumber * 2;

    hives[hives.Count - 1].OnValueChanged(resultNumber);

    int tempScore = int.Parse(Score.text);
    foreach (Hive hive in hives)
    {
      resultNumber += hive.Value;
    }
    resultNumber *= macht;
    resultNumber += tempScore;
    Score.text = resultNumber.ToString();
  }

  public void UpdateGM(int quantity, int currentNumber)
  {
    if (GM.TargetIsCompleted())
    {
      GM.GetNextTarget(m_colums * m_rows);
      ActivateHives();
    }
  }

  void DestroyHives(List<Hive> hives)
  {
    foreach (Hive hive in hives)
    {
      if (hive != hives[hives.Count - 1])
      {
          StartCoroutine(hive.transform.Scale(Vector3.zero, 0.2f, hive));
          RemoveFromGrid(hive);
      }
    }
  }

  public void UpdateGrid(List<Hive> hives)
  {
    DestroyHives(hives);
    for (int i = 0; i < m_colums; i++)
    {
      for (int j = 0; j < m_rows; j++)
      {
        if (m_Grid[i, j] == null)
        {
          m_Grid[i, j] = FindNextHive(i, j);
          m_Grid[i, j].gameObject.AddComponent<MoveHive>();

          // Fill the highest index
          if (j == (m_rows - 1))
          {
            m_Grid[i, j].gameObject.GetComponent<MoveHive>().BeginLerp(new Vector3(m_Grid[i, j].transform.localPosition.x, m_Grid[i, j].transform.localPosition.y + yHiveOffset, m_Grid[i, j].transform.localPosition.z), m_HivePositions[i, j], lerpSpeed, 0f);
          }
          else
          {
            m_Grid[i, j].gameObject.GetComponent<MoveHive>().BeginLerp(m_Grid[i, j].transform.localPosition, m_HivePositions[i, j], lerpSpeed);
          }

          m_Grid[i, j].OnPositionChanged(i, j);
        }
      }
    }
    CalculateMidNumber();
  }

  public void UpdateGrid()
  {
    for (int i = 0; i < m_colums; i++)
    {
      for (int j = 0; j < m_rows; j++)
      {
        if (m_Grid[i, j] == null)
        {
          m_Grid[i, j] = FindNextHive(i, j);
          m_Grid[i, j].gameObject.AddComponent<MoveHive>();

          m_Grid[i, j].gameObject.GetComponent<MoveHive>().BeginLerp(m_Grid[i, j].transform.localPosition, m_HivePositions[i, j], lerpSpeed);
          

          m_Grid[i, j].OnPositionChanged(i, j);
        }
      }
    }
  }

  void RemoveFromGrid(Hive hive)
  {
    for (int i = 0; i < m_colums; i++)
    {
      for (int j = 0; j < m_rows; j++)
      {
        if (m_Grid[i, j] == hive)
        {
          m_Grid[i, j] = null;
        }
      }
    }
  }

  Hive FindNextHive(int i, int j)
  {
    // MAX INDEX
    if (j == m_rows - 1)
    {
      return InstantiateHive(i, j);
    }
    else
    {
      j++;
      if (m_Grid[i, j] != null && m_Grid[i, j].Value != 0)
      {
        Hive tempHive = m_Grid[i, j];
        m_Grid[i, j] = null;
        return tempHive;
      }
      else
      {
        return FindNextHive(i, j);
      }
    }
  }

  public List<Hive> ReturnAllHivesOfSameValue(int value, List<Hive> notToBeSelected)
  {
    List<Hive> temphives = new List<Hive>();
    for (int i = 0; i < m_colums; i++)
    {
      for (int j = 0; j < m_rows; j++)
      {
        if (m_Grid[i, j].Value == value && !notToBeSelected.Contains(m_Grid[i, j]))
        {
          temphives.Add(m_Grid[i, j]);
          m_Grid[i, j].SwitchState();
        }
      }
    }
    return temphives;
  }

  public void ShuffleGrid()
  {
    Shuffle(m_Grid);
  }

  void Shuffle(Hive[,] array)
  {
    for (int i = 0; i < m_colums; i++)
    {
      for (int j = 0; j < m_rows; j++)
      {
        Swap(i, j, Random.Range(0, i - 1), Random.Range(0, j - 1), array);
      }
    }
  }

  void Swap(int currentI, int currentJ, int newI, int newJ, Hive[,] array)
  {
    int temp = array[currentI, currentJ].Value;
    array[currentI, currentJ].SetHive(array[newI, newJ].Value, newI, newJ);
    array[newI, newJ].SetHive(temp, currentI, currentJ);
  }

  public List<Hive> DeselectAllHivesOfSameValue(List<Hive> hives)
  {
    foreach (Hive hive in hives)
    {
      hive.SwitchState();
    }
    return null;
  }

  void InstantiateRows(int number,float offsetY)
  {
    GameObject row = Instantiate(rowPrefab, new Vector3(0, 0 + offsetY, 0), rowPrefab.transform.rotation) as GameObject;
    row.transform.SetParent(m_GridContainer.transform, false);
    row.name = string.Format("{0}{1}", "Row_", number);
  }

  Hive InstantiateHive(int x, int y)
  {
    GameObject prefabHive = Instantiate(hivePrefab, m_HivePositions[x, y], hivePrefab.transform.rotation) as GameObject;
    prefabHive.GetComponent<RectTransform>().localScale = new Vector3(m_scaleX,m_scaleX,m_scaleX);
    prefabHive.transform.SetParent(m_GridContainer.transform.GetChild(x),false);
    Hive tempHive = prefabHive.GetComponent<Hive>();
    tempHive.OnPositionChanged(x, y);
    tempHive.OnValueChanged(CalculateValue(midNumber));
    return tempHive;
  }

  void CalculateMidNumber()
  {
    int number = 0;
    for (int i = 0; i < m_colums; i++)
    {
      for (int j = 0; j < m_rows; j++)
      {
          number += m_Grid[i, j].Value;
      }
    }
    Debug.Log(number);
    Debug.Log(m_Grid.Length);
    
    number = Mathf.FloorToInt(number / (m_Grid.Length/2));
    Debug.Log(number);
    midNumber = number;
  }

  int CalculateValue(int target)
  {
    return Random.Range(1, target+1);
  }

  void ResetHives(List<Hive> hives)
  {
    ActivateHives();
    for (int i = 0; i < hives.Count; i++)
    {
      if (i != hives.Count - 1)
      {
        int num = Random.Range(1, 3);
        hives[i].OnValueChanged(num);
        StartCoroutine(hives[i].transform.Scale(Vector3.zero, 0.2f,false));
      }
    }
  }

  void ActivateHives()
  {
    List<GameObject> tempHives = new List<GameObject>();

    for (int i = 0; i < m_colums; i++)
    {
      for (int j = 0; j < m_rows; j++)
      {
        if (m_Grid[i,j].Value == 0)
        {
          tempHives.Add(m_Grid[i, j].gameObject);
        }
      }
    }

    if (tempHives.Count > 0)
    {
      int rndIndex = Random.Range(0, tempHives.Count);
      Hive hive = tempHives[rndIndex].GetComponent<Hive>();
      hive.OnValueChanged(Random.Range(1, 3));
      StartCoroutine(tempHives[rndIndex].transform.Scale(hivePrefab.transform.localScale, 0.2f,true));
    }
  }
}


