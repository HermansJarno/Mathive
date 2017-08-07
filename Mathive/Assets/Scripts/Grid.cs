using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

  Tile[,] m_Grid;
  public GameObject m_GridContainer;

	// Use this for initialization
	void Start () {
    InitializeGrid();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void InitializeGrid()
  {
    int width, height;
    width = m_GridContainer.transform.childCount;
    height = m_GridContainer.transform.GetChild(0).childCount;
    m_Grid = new Tile[width, height];
    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        int num = Random.Range(1, 9);
        m_Grid[i, j] = new Tile(m_GridContainer.transform.GetChild(i).GetChild(j).gameObject,num.ToString());
        m_GridContainer.transform.GetChild(i).GetChild(j).Find("Text").GetComponent<Text>().text = num.ToString();
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
}

public class Tile
{
  public GameObject tileObj;
  public int number;
  public Tile(GameObject tileObject, string value)
  {
    tileObj = tileObject;
    tileObj.transform.Find("Text").GetComponent<Text>().text = value;
  }
}
