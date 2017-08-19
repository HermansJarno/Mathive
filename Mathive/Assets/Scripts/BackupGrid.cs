using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackupGrid : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  //void SetGoalNumber()
  //{
  //  List<Tile> hives = new List<Tile>();
  //  Queue<int> numbersHives = new Queue<int>();
  //  string numberSequence = FindNumber(3, hives, Random.Range(0, m_Grid.GetLength(0)), Random.Range(0, m_Grid.GetLength(1)), "", true);
  //  int theGoalNumber = 0;
  //  for (int i = 0; i < numberSequence.Length; i++)
  //  {
  //    if (i == 0)
  //    {
  //      theGoalNumber = numberSequence[i];
  //    }
  //    if ((i % 2) != 0 && i != numberSequence.Length)
  //    {
  //      switch (numberSequence[i])
  //      {
  //        case '/':
  //          theGoalNumber /= numberSequence[i + 1];
  //          break;
  //        case '*':
  //          theGoalNumber *= numberSequence[i + 1];
  //          break;
  //        case '-':
  //          theGoalNumber -= numberSequence[i + 1];
  //          break;
  //        case '+':
  //          theGoalNumber += numberSequence[i + 1];
  //          break;
  //        default:
  //          break;
  //      }
  //    }

  //  }
  //  goalNumber.text = theGoalNumber.ToString();
  //}

  //string FindNumber(int stepsLeft, List<Tile> currentHives, int lastIndexI, int lastIndexJ, string goalNumber, bool lastWasSymbol)
  //{
  //  Debug.Log(stepsLeft);
  //  if (stepsLeft == 0)
  //  {
  //    return goalNumber;
  //  }
  //  else
  //  {
  //    List<Tile> possibleTiles = new List<Tile>();
  //    //foreach (Tile tile in m_Grid)
  //    //{
  //    //  if (tile != null)
  //    //  {
  //    //    if (Vector3.Distance(m_Grid[lastIndexI, lastIndexJ].tileObj.transform.position, tile.tileObj.transform.position) == distanceBetweenTiles)
  //    //    {
  //    //      if (!currentHives.Contains(tile))
  //    //      {
  //    //        possibleTiles.Add(tile);
  //    //        Debug.Log(Vector3.Distance(m_Grid[lastIndexI, lastIndexJ].tileObj.transform.position, tile.tileObj.transform.position));
  //    //      }
  //    //    }
  //    //  }
  //    //}

  //    if ((lastIndexI % 2) == 0)
  //    {
  //      // even number column, is counting up
  //      if (lastIndexJ != m_Grid.GetLength(1))
  //      {
  //        possibleTiles.Add(m_Grid[lastIndexI, lastIndexJ + 1]);
  //      }

  //      if (lastIndexJ != 0)
  //      {
  //        possibleTiles.Add(m_Grid[lastIndexI, lastIndexJ - 1]);
  //      }

  //      if (lastIndexI != m_Grid.GetLength(0))
  //      {

  //        possibleTiles.Add(m_Grid[lastIndexI + 1, lastIndexJ]);
  //        if (lastIndexJ != m_Grid.GetLength(1))
  //        {
  //          possibleTiles.Add(m_Grid[lastIndexI + 1, lastIndexJ + 1]);

  //        }
  //      }

  //      if (lastIndexI != 0)
  //      {
  //        possibleTiles.Add(m_Grid[lastIndexI - 1, lastIndexJ]);
  //        if (lastIndexJ != m_Grid.GetLength(1))
  //        {
  //          possibleTiles.Add(m_Grid[lastIndexI - 1, lastIndexJ + 1]);
  //        }
  //      }
  //    }
  //    else
  //    {
  //      // uneven number column, is counting up

  //      if (lastIndexJ != m_Grid.GetLength(1))
  //      {
  //        possibleTiles.Add(m_Grid[lastIndexI, lastIndexJ + 1]);
  //      }

  //      if (lastIndexJ != 0)
  //      {
  //        possibleTiles.Add(m_Grid[lastIndexI, lastIndexJ - 1]);
  //      }

  //      if (lastIndexI != m_Grid.GetLength(0))
  //      {
  //        possibleTiles.Add(m_Grid[lastIndexI + 1, lastIndexJ]);
  //        if (lastIndexJ != 0)
  //        {
  //          possibleTiles.Add(m_Grid[lastIndexI + 1, lastIndexJ - 1]);
  //        }
  //      }

  //      if (lastIndexI != 0)
  //      {
  //        possibleTiles.Add(m_Grid[lastIndexI - 1, lastIndexJ]);
  //        if (lastIndexJ != 0)
  //        {
  //          possibleTiles.Add(m_Grid[lastIndexI - 1, lastIndexJ - 1]);
  //        }
  //      }
  //    }

  //    List<Tile> deleteTiles = new List<Tile>();
  //    for (int i = 0; i < possibleTiles.Count; i++)
  //    {
  //      if (currentHives.Contains(possibleTiles[i]))
  //      {
  //        deleteTiles.Add(possibleTiles[i]);
  //      }
  //    }

  //    foreach (Tile tile in deleteTiles)
  //    {
  //      possibleTiles.Remove(tile);
  //    }

  //    if (possibleTiles.Count > 0)
  //    {
  //      List<Tile> tempTiles = new List<Tile>();
  //      if (lastWasSymbol)
  //      {
  //        foreach (Tile tile in possibleTiles)
  //        {
  //          for (int i = 0; i < symbols.Length; i++)
  //          {
  //            if (tile.value == symbols[i])
  //            {
  //              tempTiles.Add(tile);
  //            }
  //          }
  //        }

  //        foreach (Tile tile in tempTiles)
  //        {
  //          possibleTiles.Remove(tile);
  //        }
  //        lastWasSymbol = false;
  //      }
  //      else
  //      {
  //        foreach (Tile tile in possibleTiles)
  //        {
  //          for (int i = 0; i < symbols.Length; i++)
  //          {
  //            if (tile.value == symbols[i])
  //            {
  //              tempTiles.Add(tile);
  //            }
  //          }
  //        }
  //        possibleTiles = tempTiles;
  //        lastWasSymbol = true;
  //      }
  //    }
  //    else
  //    {
  //      List<Tile> hives = new List<Tile>();
  //      return FindNumber(3, hives, Random.Range(0, m_Grid.GetLength(0)), Random.Range(0, m_Grid.GetLength(1)), "", true);
  //    }


  //    if (possibleTiles.Count > 0)
  //    {
  //      int randomIndex = Random.Range(0, possibleTiles.Count);
  //      goalNumber += possibleTiles[randomIndex].value;
  //      for (int i = 0; i < m_Grid.GetLength(0); i++)
  //      {
  //        for (int j = 0; j < m_Grid.GetLength(1); j++)
  //        {
  //          if (m_Grid[i, j] == possibleTiles[randomIndex])
  //          {
  //            lastIndexI = i;
  //            lastIndexJ = j;
  //          }
  //        }
  //      }
  //      currentHives.Add(possibleTiles[randomIndex]);
  //      stepsLeft -= 1;
  //      return FindNumber(stepsLeft, currentHives, lastIndexI, lastIndexJ, goalNumber, lastWasSymbol);
  //    }
  //    else
  //    {
  //      List<Tile> hives = new List<Tile>();
  //      return FindNumber(3, hives, Random.Range(0, m_Grid.GetLength(0)), Random.Range(0, m_Grid.GetLength(1)), "", true);
  //    }
  //  }

  //}
}
