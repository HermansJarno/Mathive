using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

  string[] symbols = {"+","/", "*", "-" };
  Vector3[,] m_HivePositions;
  Hive[,] m_Grid;
  Text Score;
  public GameObject m_GridContainer;
  private GameObject hivePrefab;
  int gridWith, gridHeight;

  // Use this for initialization
  void Start () {
    InitializeGrid();
    Score = GameObject.Find("Score").GetComponent<Text>();
    hivePrefab = Resources.Load("Hive") as GameObject;

  }
	
	// Update is called once per frame
	void Update () {

  }

  

  void InitializeGrid()
  { 
    gridWith = m_GridContainer.transform.childCount;
    gridHeight = m_GridContainer.transform.GetChild(0).childCount;
    m_Grid = new Hive[gridWith, gridHeight];
    m_HivePositions = new Vector3[gridWith, gridHeight];
    for (int i = 0; i < gridWith; i++)
    {
      for (int j = 0; j < gridHeight; j++)
      {
        int num = Random.Range(1, 3);
        m_Grid[i, j] = m_GridContainer.transform.GetChild(i).GetChild(j).GetComponent<Hive>();
        m_Grid[i, j].SetHive(num.ToString(),i,j);
        m_HivePositions[i, j] = m_Grid[i, j].transform.position;
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
        if (m_Grid[i, j].gameObject == lastHive)
        {
          indexIlast = i;
          indexJlast = j;
        }
        if (m_Grid[i, j].gameObject == nextHive)
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

  public void CalculateScore(List<Hive> hives)
  {
    int resultNumber = 0;
    int macht = hives.Count;
    int currentNumber = int.Parse(hives[0].Value);
    resultNumber = currentNumber * 2;

    hives[hives.Count - 1].OnValueChanged(resultNumber.ToString());

    resultNumber = Mathf.RoundToInt(Mathf.Pow(currentNumber, macht));
    int tempScore = int.Parse(Score.text);
    Score.text = (tempScore + resultNumber).ToString();
    DestroyHives(hives);
    UpdateGrid();
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

  void UpdateGrid()
  {
      for (int i = 0; i < gridWith; i++)
      {
        for (int j = 0; j < gridHeight; j++)
        {
          if (m_Grid[i, j] == null)
          {
              m_Grid[i, j] = FindNextHive(i, j);
              m_Grid[i, j].gameObject.AddComponent<MoveHive>();
              m_Grid[i, j].gameObject.GetComponent<MoveHive>().BeginLerp(m_Grid[i, j].transform.position, m_HivePositions[i, j], 4f);
              m_Grid[i, j].OnPositionChanged(i, j);
          }
        }
      }
  }

  void RemoveFromGrid(Hive hive)
  {
    for (int i = 0; i < gridWith; i++)
    {
      for (int j = 0; j < gridHeight; j++)
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
    if (j == gridHeight - 1)
    {
      return InstantiateHive(i, j);
    }
    else
    {
      j++;
      if (m_Grid[i, j] != null)
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

  Hive InstantiateHive(int x, int y)
  {
    GameObject prefabHive = Instantiate(hivePrefab, m_HivePositions[x, y], hivePrefab.transform.rotation) as GameObject;
    prefabHive.transform.SetParent(m_GridContainer.transform.GetChild(x),false);
    prefabHive.GetComponent<Hive>().OnPositionChanged(x, y);
    prefabHive.GetComponent<Hive>().OnValueChanged(Random.Range(1, 3).ToString());
    return prefabHive.GetComponent<Hive>();
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

    for (int i = 0; i < gridWith; i++)
    {
      for (int j = 0; j < gridHeight; j++)
      {
        if (!m_Grid[i,j].gameObject.activeSelf)
        {
          tempHives.Add(m_Grid[i, j].gameObject);
        }
      }
    }

    if (tempHives.Count > 0)
    {
      int num = Random.Range(1, 3);
      int rndIndex = Random.Range(0, tempHives.Count);
      StartCoroutine(tempHives[rndIndex].transform.Scale(hivePrefab.transform.localScale, 0.2f,true));
      tempHives.Remove(tempHives[rndIndex]);
      if (num == 2 && tempHives.Count > 0)
      {
        rndIndex = Random.Range(0, tempHives.Count);
        StartCoroutine(tempHives[rndIndex].transform.Scale(hivePrefab.transform.localScale, 0.2f,true));
      }
    }
  }
}


