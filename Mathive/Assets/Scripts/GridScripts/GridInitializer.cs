using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInitializer : MonoBehaviour {

	GridManager gridManager;
	GameManager gameManager;
	GridController gridController;
	LevelData levelData;

	public void InitializeGrid()
	{
		gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		gridController = GameObject.Find("Scripts").GetComponent<GridController>();
		levelData = new LevelDataController().LoadLevelFromResources(gameManager.Level);

		gridManager.Columns = levelData.NumberOfColumns;
		gridManager.Rows = levelData.NumberOfRows;
		gameManager.SetTargets(levelData.Targets, levelData.NumberOfMoves);

		// Get current width and height of Screen
		RectTransform rtGrid = gridManager.m_GridContainer.GetComponent<RectTransform>();
		float widthScreen = rtGrid.rect.width;
		float heightScreen = rtGrid.rect.height;

		// Calculate scale
		gridManager.ScaleX = (widthScreen / gridManager.RefWidth) * gridManager.ExtraScale;
		gridManager.ScaleY = (heightScreen / gridManager.RefHeight) * gridManager.ExtraScale;

		Debug.Log(gridManager.ScaleX);
		Debug.Log(gridManager.ScaleY);

		if(gridManager.ScaleX > gridManager.ScaleY){
			gridManager.ScaleX = gridManager.ScaleY;
		}

		gridManager.YRowOffset *= gridManager.ScaleX;

		gridManager.XHiveOffset *= gridManager.ScaleX;
		gridManager.YHiveOffset *= gridManager.ScaleX;
		float width = gridManager.XHiveOffset * gridManager.Columns;
		float height = gridManager.YHiveOffset * gridManager.Rows;

		float xPosition = -(width / 2);
		float yPosition = -(height / 2);

		gridManager.Grid = new Hive[gridManager.Columns, gridManager.Rows];
		gridManager.HivePositions = new Vector3[gridManager.Columns, gridManager.Rows];

		for (int i = 0; i < gridManager.Columns; i++)
		{
			if ((i % 2) == 0)
			{
				InstantiateRows(i + 1, gridManager.YRowOffset, gridManager.m_GridContainer);
				InstantiateRows(i + 1, gridManager.YRowOffset, gridManager.m_GridContainerBackgrounds);
				InstantiateRows(i + 1, gridManager.YRowOffset, gridManager.m_GridContainerBorder);
			}
			else
			{
				InstantiateRows(i + 1, 0, gridManager.m_GridContainer);
				InstantiateRows(i + 1, 0, gridManager.m_GridContainerBackgrounds);
				InstantiateRows(i + 1, 0, gridManager.m_GridContainerBorder);
			}
		}

		// Create The Positions
		for (int i = 0; i < gridManager.Columns; i++)
		{
			for (int j = 0; j < gridManager.Rows; j++)
			{
				gridManager.HivePositions[i, j] = new Vector3(xPosition + (gridManager.XHiveOffset / 2), yPosition, 0);
				yPosition += gridManager.YHiveOffset;
			}
			xPosition += gridManager.XHiveOffset;
			yPosition = -(height / 2);
		}

		//Fill the empty Hives
		for (int i = 0; i < gridManager.Columns; i++)
		{
			for (int j = 0; j < gridManager.Rows; j++)
			{
				if(levelData.Hives[i,j] == (int)HiveType.empty){
					gridManager.Grid[i, j] = gridManager.InstantiateHive(i, j);
					gridManager.Grid[i, j].SetHive(0, i, j);
				}
			}
		}

		// Generate the hives
		for (int i = 0; i < gridManager.Columns; i++)
		{
			for (int j = 0; j < gridManager.Rows; j++)
			{
				if (gridManager.Grid[i, j] == null)
				{
					InstantiateBorder(i,j);
					
					gridManager.Grid[i, j] = gridManager.InstantiateHive(i, j);
					gridManager.Grid[i, j].SetHive(levelData.Hives[i,j], i, j);
					
					if (j - 1 > 0)
					{
						float dist = Vector3.Distance(gridManager.Grid[i, j].transform.position, gridManager.Grid[i, j - 1].transform.position);
						if (gridManager.DistanceBetweenHives > dist)
						{
							gridManager.DistanceBetweenHives = (float)Math.Round((double)dist, 1);
						}
					}
				}
			}
		}

/* 
		for (int i = 0; i < gridManager.Columns; i++)
		{
			for (int j = 0; j < gridManager.Rows; j++)
			{
				if(gridManager.Grid[i, j].Value != 0){
					Destroy(gridManager.Grid[i, j].gameObject);
					gridManager.Grid[i, j] = null;
				}
			}
		}

		InstantiateGrid();*/

		//Blockage
		//Columns
		for (int i = 0; i < gridManager.Columns; i++)
		{
			//rows
			for (int j = 0; j < gridManager.Rows; j++)
			{
				if (levelData.Hives[i,j] == (int)HiveType.blockage)
				{
					gridManager.Grid[i, j].SetHive(-1, i, j);	
				}
				
			}
		}

		Destroy(gameObject.GetComponent<GridInitializer>());
	}

	void InstantiateRows(int number, float offsetY, GameObject gridContainer)
	{
		GameObject row = Instantiate(gridManager.RowPrefab, new Vector3(0, 0 + offsetY, 0), gridManager.RowPrefab.transform.rotation) as GameObject;
		row.transform.SetParent(gridContainer.transform, false);
		row.name = string.Format("{0}{1}", "Row_", number);
	}

	void InstantiateBorder(int x, int y){
		float scale = 1.3f;
		GameObject prefabHive = Instantiate(gridManager.BorderPrefab, gridManager.HivePositions[x, y], gridManager.HivePrefab.transform.rotation) as GameObject;
		prefabHive.GetComponent<RectTransform>().localScale = new Vector3(gridManager.ScaleX * scale, gridManager.ScaleX * scale, gridManager.ScaleX * scale);
		prefabHive.transform.SetParent(gridManager.m_GridContainerBorder.transform.GetChild(x), false);
	}

	public void InstantiateGrid()
	{
		gridController.InputAvailable = false;
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
		gridController.InputAvailable = true;
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
}
