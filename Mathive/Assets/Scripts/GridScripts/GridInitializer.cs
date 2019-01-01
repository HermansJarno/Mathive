using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInitializer : MonoBehaviour {

	GridManager gridManager;
	GameManager gameManager;

	public void InitializeGrid()
	{
		gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		gridManager.Columns = gridManager.GridLevels.Levels[gameManager.Level - 1][0];
		gridManager.Rows = gridManager.GridLevels.Levels[gameManager.Level - 1][1];

		// Get current width and height of Screen
		RectTransform rtGrid = gridManager.m_GridContainer.GetComponent<RectTransform>();
		float widthScreen = rtGrid.rect.width;
		float heightScreen = rtGrid.rect.height;

		// Calculate scale
		gridManager.ScaleX = (widthScreen / gridManager.RefWidth) * gridManager.ExtraScale;
		gridManager.ScaleY = (heightScreen / gridManager.RefHeight) * gridManager.ExtraScale;

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
				for (int r = 0; r < gridManager.GridLevels.EmptyHivesLevels[gameManager.Level - 1][i].Length; r++)
				{
					//first the level, then the row, then check for index and validate them with J
					if (gridManager.GridLevels.EmptyHivesLevels[gameManager.Level - 1][i][r] == (j + 1))
					{
						gridManager.Grid[i, j] = gridManager.InstantiateHive(i, j);
						gridManager.Grid[i, j].SetHive(0, i, j);
					}
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
					int num = UnityEngine.Random.Range(1, 7);
					gridManager.Grid[i, j] = gridManager.InstantiateHive(i, j);
					gridManager.Grid[i, j].SetHive(num, i, j);

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

		//Blockage
		//Columns
		for (int i = 0; i < gridManager.Columns; i++)
		{
			//rows
			for (int j = 0; j < gridManager.Rows; j++)
			{
				if (j == 4)
				{
					if (gridManager.Grid[i, j] != null)
					{
						if (gridManager.Grid[i, j].GetHiveType != HiveType.empty)
						{
							gridManager.Grid[i, j].SetHive(-1, i, j);
						}
					}
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
}
