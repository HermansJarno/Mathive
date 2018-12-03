using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInitializer : MonoBehaviour {

	/*
	private GameObject rowPrefab;

	public void Awake()
	{
		rowPrefab = Resources.Load("Row_") as GameObject;
	}

	public void InitializeGrid(int colums, int rows)
	{
		Debug.Log("number of columns: " +  colums + ", Number of rows: " + rows);

		// Get current width and height of Screen
		RectTransform rtGrid = m_GridContainer.GetComponent<RectTransform>();
		float widthScreen = rtGrid.rect.width;
		float heightScreen = rtGrid.rect.height;

		// Calculate scale
		ScaleX = (widthScreen / m_refWidth) * extraScale;
		ScaleY = (heightScreen / m_refHeight) * extraScale;

		ScaleX = (ScaleX + ScaleY) / 2;

		yRowOffset *= ScaleX;

		xHiveOffset *= ScaleX;
		YHiveOffset *= ScaleX;
		float width = xHiveOffset * colums;
		float height = YHiveOffset * rows;

		float xPosition = -(width / 2);
		float yPosition = -(height / 2);

		Grid = new Hive[colums, rows];
		HivePositions = new Vector3[colums, rows];


		for (int i = 0; i < colums; i++)
		{
			if ((i % 2) == 0)
			{
				InstantiateRows(i + 1, yRowOffset, m_GridContainer);
				InstantiateRows(i + 1, yRowOffset, m_GridContainerBackgrounds);
			}
			else
			{
				InstantiateRows(i + 1, 0, m_GridContainer);
				InstantiateRows(i + 1, 0, m_GridContainerBackgrounds);
			}
		}

		// Create The Positions
		for (int i = 0; i < colums; i++)
		{
			for (int j = 0; j < rows; j++)
			{
				HivePositions[i, j] = new Vector3(xPosition + (xHiveOffset / 2), yPosition, 0);
				yPosition += YHiveOffset;
			}
			xPosition += xHiveOffset;
			yPosition = -(height / 2);
		}

		//Fill the empty Hives
		for (int i = 0; i < colums; i++)
		{
			for (int j = 0; j < rows; j++)
			{
				for (int r = 0; r < GridLevels.EmptyHivesLevels[GetManager.Level - 1][i].Length; r++)
				{
					//first the level, then the row, then check for index and validate them with J
					if (GridLevels.EmptyHivesLevels[GetManager.Level - 1][i][r] == (j + 1))
					{
						Grid[i, j] = InstantiateHive(i, j);
						Grid[i, j].SetHive(0, i, j);
					}
				}
			}
		}

		//Blockage
		//Columns
		for (int i = 0; i < colums; i++)
		{
			//rows
			for (int j = 0; j < rows; j++)
			{
				if (j == 4)
				{
					Grid[i, j] = InstantiateHive(i, j);
					Grid[i, j].SetHive(-1, i, j);
				}
			}
		}

		// Generate the hives
		for (int i = 0; i < colums; i++)
		{
			for (int j = 0; j < rows; j++)
			{
				if (Grid[i, j] == null)
				{
					int num = UnityEngine.Random.Range(1, 7);
					Grid[i, j] = InstantiateHive(i, j);
					Grid[i, j].SetHive(num, i, j);

					if (j - 1 > 0)
					{
						float dist = Vector3.Distance(Grid[i, j].transform.position, Grid[i, j - 1].transform.position);
						if (DistanceBetweenHives > dist)
						{
							DistanceBetweenHives = (float)Math.Round((double)dist, 1);
						}
					}
					//Debug.Log(m_Grid[i, j].Value);
				}
			}
		}
	}

	void InstantiateRows(int number, float offsetY, GameObject grid)
	{
		GameObject row = Instantiate(rowPrefab, new Vector3(0, 0 + offsetY, 0), rowPrefab.transform.rotation) as GameObject;
		row.transform.SetParent(grid.transform, false);
		row.name = string.Format("{0}{1}", "Row_", number);
	}

	void SetIndexToZero(int x, int y)
	{
		Grid[x, y].SetHive(0, x, y);
	}*/

}
