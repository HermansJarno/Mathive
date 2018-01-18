using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

  public GameObject m_GridContainer;
  [SerializeField] float m_refWidth = 800f;
  [SerializeField] float m_refHeight = 1280;
  [SerializeField] float xRowOffset = 85;
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

  private int _numberOfFirstTarget = 50;
  private int m_rows, m_colums;


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

  void InitializeGrid(int rows, int colums)
  {
    float widthScreen = m_GridContainer.GetComponent<RectTransform>().rect.width;
    float heightScreen = m_GridContainer.GetComponent<RectTransform>().rect.height;
    m_scaleX = (widthScreen / m_refWidth) + extraScale;
    m_scaleY = (heightScreen / m_refHeight) + extraScale;

    offsetTop *= m_scaleY;
    xRowOffset *= m_scaleX;
    yRowOffset *= m_scaleX;
    yHiveOffset *= m_scaleX;
    float width = xRowOffset * rows;
    float height = yRowOffset * colums;
    float xPosition = -(width / 2);
    float yPosition = -(height / 2) - offsetTop;

    m_Grid = new Hive[rows, colums];
    m_HivePositions = new Vector3[rows, colums];
    m_rows = rows;
    m_colums = colums;

    for (int i = 0; i < rows; i++)
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
    for (int i = 0; i < rows; i++)
    {
      for (int j = 0; j < colums; j++)
      {
        m_HivePositions[i, j] = new Vector3(xPosition + (xRowOffset / 2), yPosition, 0);
        yPosition += yHiveOffset;
      }
      xPosition += xRowOffset;
      yPosition = -(height / 2) - offsetTop;
    }

    //Fill the empty Hives
    for (int i = 0; i < rows; i++)
    {
      for (int j = 0; j < colums; j++)
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
    for (int i = 0; i < rows; i++)
    {
      for (int j = 0; j < colums; j++)
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
    GM.SetTargets(1, _numberOfFirstTarget);
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
      GM.GetNextTarget(m_rows * m_colums);
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
    for (int i = 0; i < m_rows; i++)
    {
      for (int j = 0; j < m_colums; j++)
      {
        if (m_Grid[i, j] == null)
        {
          m_Grid[i, j] = FindNextHive(i, j);
          m_Grid[i, j].gameObject.AddComponent<MoveHive>();

          // Fill the highest index
          if (j == (m_colums - 1))
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
  }

  public void UpdateGrid()
  {
    for (int i = 0; i < m_rows; i++)
    {
      for (int j = 0; j < m_colums; j++)
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
    for (int i = 0; i < m_rows; i++)
    {
      for (int j = 0; j < m_colums; j++)
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
    if (j == m_colums - 1)
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
    for (int i = 0; i < m_rows; i++)
    {
      for (int j = 0; j < m_colums; j++)
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
    Hive[,] temphives = new Hive[m_rows,m_colums];

    for (int i = 0; i < m_rows; i++)
    {
      for (int j = 0; j < m_colums; j++)
      {
        temphives[i, j] = null;

      }
    }

    for (int i = 0; i < m_rows; i++)
    {
      for (int j = 0; j < m_colums; j++)
      {
        if (m_Grid[i, j].Value != 0 && temphives[i,j] == null)
        {
          bool newIndexFound = false;
          int newIndexI = 0;
          int newIndexJ = 0;

          while (!newIndexFound)
          {
            newIndexI = Random.Range(0, i + 1);
            newIndexJ = Random.Range(0, j + 1);
            if (m_Grid[newIndexI, newIndexJ].Value != 0 && temphives[newIndexI, newIndexJ] == null) newIndexFound = true;
          }
          Hive tempHive = m_Grid[i, j];
          temphives[i,j] = m_Grid[i, j];
          temphives[i, j] = m_Grid[newIndexI, newIndexJ];
          m_Grid[i, j].SetHive(m_Grid[newIndexI, newIndexJ].Value, newIndexI, newIndexJ);
          m_Grid[newIndexI, newIndexJ].SetHive(tempHive.Value, i, j);
        }
      }
    }
  }

  //public void ShuffleGrid()
  //{
  //  Shuffle(m_Grid);
  //  UpdateGrid();
  //}

  //public void ShuffleGrid()
  //{
  //  Hive[,] tempHives = m_Grid;

  //  for (int i = 0; i < m_rows; i++)
  //  {
  //    for (int j = 0; j < m_colums; j++)
  //    {
  //      if (m_Grid[i, j].Value != 0 && tempHives[i, j] != null)
  //      {
  //        int i2 = 0;
  //        int j2 = 0;

  //        bool nextIndexfound = false;
  //        while (!nextIndexfound)
  //        {
  //          i2 = Random.Range(0, m_rows);
  //          j2 = Random.Range(0, m_colums);

  //          if (m_Grid[i2, j2].Value != 0 && tempHives[i2, j2] != null)
  //          {
  //            nextIndexfound = true;
  //          }
  //        }

  //        Hive tempHive = m_Grid[i, j];
  //        m_Grid[i, j] = m_Grid[i2, j2];
  //        m_Grid[i2, j2] = tempHive;

  //        if (m_Grid[i, j].GetComponent<MoveHive>() == null) m_Grid[i, j].gameObject.AddComponent<MoveHive>();
  //        if (m_Grid[i2, j2].GetComponent<MoveHive>() == null) m_Grid[i2, j2].gameObject.AddComponent<MoveHive>();

  //        //move first hive
  //        m_Grid[i, j].gameObject.GetComponent<MoveHive>().BeginLerp(m_Grid[i2, j2].transform.localPosition, m_HivePositions[i, j], lerpSpeed);
  //        m_Grid[i2, j2].gameObject.GetComponent<MoveHive>().BeginLerp(m_Grid[i, j].transform.localPosition, m_HivePositions[i2, j2], lerpSpeed);

  //        m_Grid[i, j].OnPositionChanged(i2, j2);
  //        m_Grid[i2, j2].OnPositionChanged(i, j);
  //        tempHives[i, j] = null;
  //        tempHives[i2, j2] = null;
  //      }
  //    }
  //  }
  //}

  //public void ShuffleGrid()
  //{
  //  int lengthRow = m_Grid.GetLength(0);

  //  for (int i = m_Grid.GetLength(1) - 1; i > 0; i--)
  //  {
  //    int i0 = i / lengthRow;
  //    int i1 = i % lengthRow;

  //    int j = Random.Range(0,i + 1);
  //    int j0 = j / lengthRow;
  //    int j1 = j % lengthRow;

  //    if (m_Grid[i0, i1].Value != 0 && m_Grid[j0, j1].Value != 0)
  //    {
  //      Hive temp = m_Grid[i0, i1];
  //      m_Grid[i0, i1] = m_Grid[j0, j1];
  //      m_Grid[i0, i1].gameObject.AddComponent<MoveHive>();
  //      m_Grid[i0, i1].gameObject.GetComponent<MoveHive>().BeginLerp(m_Grid[i0, i1].transform.localPosition, m_HivePositions[j0, j1], lerpSpeed);
  //      m_Grid[i0, i1].OnPositionChanged(j0, j1);

  //      m_Grid[j0, j1] = temp;
  //      m_Grid[j0, j1].gameObject.AddComponent<MoveHive>();
  //      m_Grid[j0, j1].gameObject.GetComponent<MoveHive>().BeginLerp(temp.transform.localPosition, m_HivePositions[i0, i1], lerpSpeed);
  //      m_Grid[j0, j1].OnPositionChanged(i0, i1);
  //    }

  //  }
  //}

  ////public void ShuffleGrid()
  ////{
  ////  Hive[,] tempHives = m_Grid;

  ////  List<Hive> hives = new List<Hive>();
  ////  foreach (Hive hive in m_Grid)
  ////  {
  ////    hives.Add(hive);
  ////  }

  ////  DestroyHives(hives);

  ////  for (int i = 0; i < m_rows; i++)
  ////  {
  ////    for (int j = 0; j < m_colums; j++)
  ////    {
  ////      if (m_Grid[i, j] == null)
  ////      {
  ////        int index = Random.Range(0, hives.Count);
  ////        m_Grid[i, j] = hives[index];
  ////        m_Grid[i, j].gameObject.AddComponent<MoveHive>();

  ////        m_Grid[i, j].gameObject.GetComponent<MoveHive>().BeginLerp(m_Grid[i, j].transform.localPosition, m_HivePositions[i, j], lerpSpeed);

  ////        m_Grid[i, j].OnPositionChanged(i, j);
  ////        hives.RemoveAt(index);
  ////      }
  ////    }
  ////  }
  ////}

  //public void Shuffle(Hive[,] array)
  //{
  //  int lengthRow = array.GetLength(1);

  //  for (int i = array.Length - 1; i > 0; i--)
  //  {
  //    int i0 = i / lengthRow;
  //    int i1 = i % lengthRow;

  //    int j = Random.Range(0,i + 1);
  //    int j0 = j / lengthRow;
  //    int j1 = j % lengthRow;

  //    Hive temp = array[i0, i1];
  //    array[i0, i1] = array[j0, j1];
  //    array[j0, j1] = temp;
  //  }
  //}

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
    int targetNumber = GM.TargetNumber;
   
    tempHive.OnValueChanged(CalculateValue(targetNumber));
    return tempHive;
  }

  int CalculateValue(int target)
  {
    int[] possibleNumbers;
    if (target > 2)
    {
      possibleNumbers = new int[(int)Mathf.Sqrt(target/2)];
      int targetNumber = target/2;

      for (int i = 0; i < possibleNumbers.Length; i++)
      {
        possibleNumbers[i] = targetNumber;
        targetNumber /= 2;
      }
    }
    else
    {
      possibleNumbers = new int[2] { 1, 2 };
    }

    return possibleNumbers[Random.Range(0, possibleNumbers.Length)];
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

    for (int i = 0; i < m_rows; i++)
    {
      for (int j = 0; j < m_colums; j++)
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


