using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
	private GridManager gridManager;
	private GameManager gameManager;

	Score m_score;

	private bool inputAvailable = true;
	public bool InputAvailable
	{
		get
		{
			return inputAvailable;
		}
		set
		{
			inputAvailable = value;
		}
	}

	// Use this for initialization
	void Start()
	{
		gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		m_score = GameObject.Find("Scripts").GetComponent<Score>();
	}

	public bool IsHiveNear(Hive lastHive, Hive nextHive)
	{
		bool hiveIsNear = false;

		if (((lastHive.X + 1 == nextHive.X) || (lastHive.X - 1 == nextHive.X) || (lastHive.X == nextHive.X)))
		{
			if (((lastHive.Y + 1 == nextHive.Y) || (lastHive.Y - 1 == nextHive.Y) || (lastHive.Y == nextHive.Y)))
			{
				float result = (float)Math.Round((double)Vector3.Distance(lastHive.transform.position, nextHive.transform.position), 1);
				if (result == gridManager.DistanceBetweenHives)
				{
					hiveIsNear = true;
				}
			}
		}
		return hiveIsNear;
	}

	public void DestroyNearBlockages(Hive currentHive)
	{
		// even number search indexes below
		if ((currentHive.X % 2) == 0)
		{
			// left/right
			int[] xOffsets = { -1, 1, 0, 0, -1, 1 };
			// Up/Down
			int[] yOffsets = { 0, 0, -1, 1, 1, 1};

			for (int i = 0; i < xOffsets.Length; i++)
			{
				if (IndexAroundHiveIsInTheGrid(currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i])
				&& (gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]] != null && gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]].tag == "Blockage"))
				{
					DestroyHive(gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]]);
				}
			}
		}
		else
		{
			// Left/right
			int[] xOffsets = { -1, 1, 0, 0, -1, 1 };
			// Up/Down
			int[] yOffsets = { 0, 0, -1, 1, -1, -1 };
			for (int i = 0; i < xOffsets.Length; i++)
			{
				if (IndexAroundHiveIsInTheGrid(currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i])
				&& (gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]] != null && gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]].tag == "Blockage"))
				{
					DestroyHive(gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]]);
				}
			}
		}
	}

	private bool IndexAroundHiveIsInTheGrid(int xValue, int yValue){
		bool isInTheGrid = false;
		if ((gridManager.Grid.GetLength(0) > xValue) && (gridManager.Grid.GetLength(1) > yValue))
		{
			isInTheGrid |= ((0 <= xValue) && (0 <= yValue));
		}
		return isInTheGrid;
	}

	void DestroyHives(List<Hive> hives)
	{
		foreach (Hive hive in hives)
		{
			DestroyNearBlockages(hive);
			DestroyHive(hive);
		}
	}

	void DestroyHive(Hive hive){
		StartCoroutine(hive.transform.Scale(Vector3.zero, 0.2f, hive));
		RemoveFromGrid(hive);
	}

	public void UpdateGrid(List<Hive> hives)
	{
		inputAvailable = false;
		m_score.CalculateScore(hives);
		DestroyHives(hives);
		int highestNumberOfMoves = 0;
		for (int i = 0; i < gridManager.Columns; i++)
		{
			int numberOfNewIndex = 0;
			float numberOfMoves = -1;
			for (int j = 0; j < gridManager.Rows; j++)
			{
				if (gridManager.Grid[i, j] == null)
				{
					int blockageLevel = GetIndexOfBlockage(i, j);
					float numberOfIndexesHigher = 0;
					if (blockageLevel < j || blockageLevel == -1)
					{
						// blockage is beneath index
						bool hadToCreateNewIndex = false;
						gridManager.Grid[i, j] = FindNextHive(i, j, out hadToCreateNewIndex, ref numberOfIndexesHigher);
						if (gridManager.Grid[i, j] != null)
						{
							gridManager.Grid[i, j].gameObject.AddComponent<MoveHive>();

							if (hadToCreateNewIndex)
							{
								numberOfNewIndex++;
								gridManager.Grid[i, j].gameObject.GetComponent<MoveHive>().BeginLerp(new Vector3(gridManager.Grid[i, j].transform.localPosition.x, gridManager.Grid[i, j].transform.localPosition.y + (gridManager.YHiveOffset * (gridManager.Rows - j)), gridManager.Grid[i, j].transform.localPosition.z), gridManager.HivePositions[i, j], gridManager.LerpSpeed, 0f, (gridManager.DelayLerp * (numberOfMoves + numberOfNewIndex)), numberOfIndexesHigher);
							}
							else
							{
								numberOfMoves++;
								gridManager.Grid[i, j].gameObject.GetComponent<MoveHive>().BeginLerp(gridManager.Grid[i, j].transform.localPosition, gridManager.HivePositions[i, j], gridManager.LerpSpeed, (gridManager.DelayLerp * numberOfMoves), numberOfIndexesHigher);
							}

							gridManager.Grid[i, j].OnPositionChanged(i, j);
						}
					}
					else
					{
						//blockage is above index
						gridManager.Grid[i, j] = FindNextHive(i, j, blockageLevel, ref numberOfIndexesHigher);
						if (gridManager.Grid[i, j] != null)
						{
							numberOfMoves++;
							gridManager.Grid[i, j].gameObject.AddComponent<MoveHive>();
							gridManager.Grid[i, j].gameObject.GetComponent<MoveHive>().BeginLerp(gridManager.Grid[i, j].transform.localPosition, gridManager.HivePositions[i, j], gridManager.LerpSpeed, (gridManager.DelayLerp * numberOfMoves), numberOfIndexesHigher);
							gridManager.Grid[i, j].OnPositionChanged(i, j);
						}
					}
				}
				if (highestNumberOfMoves < (int)numberOfMoves)
				{
					highestNumberOfMoves = (int)numberOfMoves;
				}
			}
		}
		Invoke("SetInputActive", 0.1f * highestNumberOfMoves);
	}

	void SetInputActive()
	{
		inputAvailable = true;
	}

	void RemoveFromGrid(Hive hive)
	{
		for (int i = 0; i < gridManager.Columns; i++)
		{
			for (int j = 0; j < gridManager.Rows; j++)
			{
				if (gridManager.Grid[i, j] == hive)
				{
					gridManager.Grid[i, j] = null;
				}
			}
		}
	}

	Hive FindNextHive(int i, int j, out bool newHive, ref float numberOfIndexesHigher)
	{
		newHive = false;
		// MAX INDEX
		if (j == gridManager.Rows - 1)
		{
			newHive = true;
			return gridManager.InstantiateHive(i, j);
		}
		else
		{
			j++;
			numberOfIndexesHigher++;
			// check the index above the last one, if its not empty, take it as the next block to fall
			if (gridManager.Grid[i, j] != null && gridManager.Grid[i, j].Value != (int)HiveType.empty)
			{
				Hive tempHive = gridManager.Grid[i, j];
				gridManager.Grid[i, j] = null;
				return tempHive;
			}
			else
			{
				// if no next hive was found, search for the next
				return FindNextHive(i, j, out newHive, ref numberOfIndexesHigher);
			}
		}
	}

	Hive FindNextHive(int i, int j, int blockageIndex, ref float numberOfIndexesHigher)
	{
		// MAX INDEX
		if (j == blockageIndex - 1)
		{
			// search side hive, if found, return bool made side movement
			return null;
		}
		else
		{
			j++;
			numberOfIndexesHigher++;
			// check the index above the last one, if its not empty, take it as the next block to fall
			if (gridManager.Grid[i, j] != null && gridManager.Grid[i, j].Value != (int)HiveType.empty)
			{
				Hive tempHive = gridManager.Grid[i, j];
				gridManager.Grid[i, j] = null;
				return tempHive;
			}
			else
			{
				// if no next hive was found, search for the next
				return FindNextHive(i, j, blockageIndex, ref numberOfIndexesHigher);
			}
		}
	}

	int GetIndexOfBlockage(int i, int j)
	{
		int index = -1;
		for (int jj = 0; jj < gridManager.Grid.GetLength(1); jj++)
		{
			if (gridManager.Grid[i, jj] != null && gridManager.Grid[i, jj].GetHiveType == HiveType.blockage)
			{
				// if index you're checking on, is lower than the blockage, return first occurence of blockage
				if (j < jj)
				{
					return jj;
				}
				// if index you're checking on, is higher than the blockage, check if there is another blockage.
				else if (j > jj)
				{
					index = jj;
				}
			}
		}
		return index;
	}

	public List<Hive> ReturnAllHivesOfSameValue(HiveType hiveType, List<Hive> notToBeSelected)
	{
		List<Hive> temphives = new List<Hive>();
		for (int i = 0; i < gridManager.Columns; i++)
		{
			for (int j = 0; j < gridManager.Rows; j++)
			{
				if(gridManager.Grid[i, j] != null){
					if (gridManager.Grid[i, j].GetHiveType == hiveType && !notToBeSelected.Contains(gridManager.Grid[i, j]))
					{
						temphives.Add(gridManager.Grid[i, j]);
						gridManager.Grid[i, j].SetSelectedImage();
					}
				}
			}
		}
		return temphives;
	}

	public void ShuffleGrid()
	{
		Shuffle(gridManager.Grid);
	}

	void Shuffle(Hive[,] array)
	{
		for (int i = 0; i < gridManager.Columns; i++)
		{
			for (int j = 0; j < gridManager.Rows; j++)
			{
				int newI = UnityEngine.Random.Range(0, i - 1);
				int newJ = UnityEngine.Random.Range(0, j - 1);
				if (gridManager.Grid[i, j].Value != 0 && gridManager.Grid[newI, newJ].Value != 0)
					Swap(i, j, newI, newJ, array);
			}
		}
	}

	void Swap(int currentI, int currentJ, int newI, int newJ, Hive[,] array)
	{
		HiveType temp = array[currentI, currentJ].GetHiveType;
		array[currentI, currentJ].OnValueChanged(array[newI, newJ].GetHiveType);
		array[newI, newJ].OnValueChanged(temp);
	}

	public List<Hive> DeselectAllHivesOfSameValue(List<Hive> hives)
	{
		foreach (Hive hive in hives)
		{
			hive.SwitchState();
		}
		return null;
	}
}