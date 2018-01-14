using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

  public GameObject m_GridContainer;
  public float m_refWidth = 800f;
  [SerializeField] float xRowOffset = 85;
  [SerializeField] float yRowOffset = 50;
  [SerializeField] float yHiveOffset = 100;
  [SerializeField] float lerpSpeed = 4;

  [SerializeField] private Vector3[,] m_HivePositions;
  [SerializeField] private Hive[,] m_Grid;
  private GridLevels m_GridLevels = new GridLevels();
  private Text Score;
  
  private GameObject hivePrefab;
  private GameObject rowPrefab;
  private GameManager GM;

  float distanceBetweenHives = Mathf.Infinity;
  [SerializeField] float m_scale;



  private int _numberOfFirstTarget = 50;
  private int m_rows, m_colums;

  // Use this for initialization
  void Start () {
    Score = GameObject.Find("Score").GetComponent<Text>();
    hivePrefab = Resources.Load("Hive") as GameObject;
    rowPrefab = Resources.Load("Row_") as GameObject;
    GM = GameObject.Find("Scripts").GetComponent<GameManager>();

    Debug.Log(m_GridLevels.Levels[GM.Level-1][0]);
    Debug.Log(m_GridLevels.Levels[GM.Level-1][1]);

    InitializeGrid(m_GridLevels.Levels[GM.Level -1][0], m_GridLevels.Levels[GM.Level -1][1]);

  }

  //void InitializeGrid()
  //{ 
  //  m_rows = m_GridContainer.transform.childCount;
  //  m_colums = m_GridContainer.transform.GetChild(0).childCount;
  //  m_Grid = new Hive[m_rows, m_colums];
  //  m_HivePositions = new Vector3[m_rows, m_colums];
  //  for (int i = 0; i < m_rows; i++)
  //  {
  //    for (int j = 0; j < m_colums; j++)
  //    {
  //      int num = Random.Range(1, 3);
  //      m_Grid[i, j] = m_GridContainer.transform.GetChild(i).GetChild(j).GetComponent<Hive>();
  //      m_Grid[i, j].SetHive(num.ToString(),i,j);
  //      m_HivePositions[i, j] = m_Grid[i, j].transform.position;
  //      if (j - 1 > 0)
  //      {
  //        float dist = Vector3.Distance(m_Grid[i, j].transform.position, m_Grid[i, j - 1].transform.position);
  //        if (distanceBetweenHives > dist)
  //        {
  //          distanceBetweenHives = Mathf.Ceil(dist);
  //        }
  //      }   
  //    }
  //  }
  //  GM.SetTargets(1, _numberOfFirstTarget);
  //}

  void InitializeGrid(int rows, int colums)
  {
    float widthScreen = m_GridContainer.GetComponent<RectTransform>().rect.width;
    m_scale = widthScreen / m_refWidth;
    xRowOffset *= m_scale;
    yRowOffset *= m_scale;
    yHiveOffset *= m_scale;
    float width = xRowOffset * rows;
    float height = yRowOffset * colums;
    float xPosition = -(width / 2);
    float yPosition = height / 2;

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
        yPosition -= yHiveOffset;
      }
      xPosition += xRowOffset;
      yPosition = height / 2;
    }

    // Generate the hives
    for (int i = 0; i < rows; i++)
    {
      for (int j = 0; j < colums; j++)
      {
        int num = Random.Range(1, 6);
        m_Grid[i, j] = InstantiateHive(i,j);
        m_Grid[i, j].SetHive(num.ToString(), i, j);
        
        if (j - 1 > 0)
        {
          float dist = Vector3.Distance(m_Grid[i, j].transform.position, m_Grid[i, j - 1].transform.position);
          if (distanceBetweenHives > dist)
          {
            distanceBetweenHives = Mathf.Ceil(dist);
          }
        }
        //System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(0.5));
      }
    }
    GM.SetTargets(1, _numberOfFirstTarget);
  }

  void SetIndexToZero(int x, int y)
  {
    m_Grid[x, y].SetHive("0", x, y);
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
    int currentNumber = int.Parse(hives[0].Value);
    resultNumber = currentNumber * 2;

    hives[hives.Count - 1].OnValueChanged(resultNumber.ToString());

    //resultNumber = Mathf.RoundToInt(Mathf.Pow(currentNumber, macht));
    int tempScore = int.Parse(Score.text);
    foreach (Hive hive in hives)
    {
      resultNumber += int.Parse(hive.Value);
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

  void DestroyHives(List<Hive> hives, bool removeHive)
  {
    foreach (Hive hive in hives)
    {
      if (hive != hives[hives.Count - 1])
      {
        if (removeHive)
        {
          StartCoroutine(hive.transform.Scale(Vector3.zero, 0.2f));
          SetIndexToZero(hive.X, hive.Y);
          removeHive = false;
        }
        else
        {
          StartCoroutine(hive.transform.Scale(Vector3.zero, 0.2f, hive));
          RemoveFromGrid(hive);
        }
      }
    }
  }

  public void UpdateGrid(List<Hive> hives, bool removeHive)
  {
    //DestroyHives(hives, removeHive);
    for (int i = 0; i < m_rows; i++)
    {
      for (int j = 0; j < m_colums; j++)
      {
        if (m_Grid[i, j] == null)
        {
          m_Grid[i, j] = FindNextHive(i, j);
          m_Grid[i, j].gameObject.AddComponent<MoveHive>();
          if (j == (m_colums - 1))
          {
            m_Grid[i, j].gameObject.GetComponent<MoveHive>().BeginLerp(new Vector3(m_Grid[i, j].transform.position.x, m_Grid[i, j].transform.position.y + distanceBetweenHives, m_Grid[i, j].transform.position.z), m_HivePositions[i, j], 4f, 0f);
          }
          else
          {
            m_Grid[i, j].gameObject.GetComponent<MoveHive>().BeginLerp(m_Grid[i, j].transform.position, m_HivePositions[i, j], 4f);
          }

          m_Grid[i, j].OnPositionChanged(i, j);
        }
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
            m_Grid[i, j].gameObject.GetComponent<MoveHive>().BeginLerp(new Vector3(m_Grid[i, j].transform.localPosition.x, m_Grid[i, j].transform.localPosition.y + distanceBetweenHives, m_Grid[i, j].transform.localPosition.z), m_HivePositions[i, j], lerpSpeed, 0f);
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
      if (m_Grid[i, j] != null && m_Grid[i, j].Value != "0")
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

  void InstantiateRows(int number,float offsetY)
  {
    GameObject row = Instantiate(rowPrefab, new Vector3(0, 0 + offsetY, 0), rowPrefab.transform.rotation) as GameObject;
    row.transform.SetParent(m_GridContainer.transform, false);
    row.name = string.Format("{0}{1}", "Row_", number);
  }

  Hive InstantiateHive(int x, int y)
  {
    GameObject prefabHive = Instantiate(hivePrefab, m_HivePositions[x, y], hivePrefab.transform.rotation) as GameObject;
    prefabHive.GetComponent<RectTransform>().localScale = new Vector3(m_scale,m_scale,m_scale);
    prefabHive.transform.SetParent(m_GridContainer.transform.GetChild(x),false);
    Hive tempHive = prefabHive.GetComponent<Hive>();
    tempHive.OnPositionChanged(x, y);
    int targetNumber = GM.TargetNumber;
   
    tempHive.OnValueChanged(CalculateValue(targetNumber).ToString());
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
        hives[i].OnValueChanged(num.ToString());
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
        if (m_Grid[i,j].Value == "0")
        {
          tempHives.Add(m_Grid[i, j].gameObject);
        }
      }
    }

    if (tempHives.Count > 0)
    {
      int rndIndex = Random.Range(0, tempHives.Count);
      Hive hive = tempHives[rndIndex].GetComponent<Hive>();
      hive.OnValueChanged((Random.Range(1, 3).ToString()));
      StartCoroutine(tempHives[rndIndex].transform.Scale(hivePrefab.transform.localScale, 0.2f,true));
    }
  }
}


