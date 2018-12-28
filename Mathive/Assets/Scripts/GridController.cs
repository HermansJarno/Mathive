using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
	public GameObject m_GridContainerBackgrounds;
	public GameObject m_GridContainerBorder;
	public GameObject m_GridContainer;
	[SerializeField] float m_refWidth = 600f;
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
	private GameObject borderPrefab;
	private GameObject rowPrefab;
	private GameManager GM;

	float distanceBetweenHives = Mathf.Infinity;
	[SerializeField] float m_scaleX;
	[SerializeField] float m_scaleY;
	float m_midScale;
	[SerializeField] float extraScale = 0.1f;

	private int m_colums, m_rows;
	private int midNumber;
	Score m_score;

	private int blockageLayer = -1;
	private int emptyHiveLayer = 0;

	// Use this for initialization
	void Start()
	{
		m_score = GameObject.Find("Scripts").GetComponent<Score>();
		Score = GameObject.Find("Score").GetComponent<Text>();
		hivePrefab = Resources.Load("Hive") as GameObject;
		borderPrefab = Resources.Load("HiveBorder") as GameObject;
		rowPrefab = Resources.Load("Row_") as GameObject;
		GM = GameObject.Find("Scripts").GetComponent<GameManager>();

		Debug.Log("number of columns: " + m_GridLevels.Levels[GM.Level - 1][1] + ", Number of rows: " + m_GridLevels.Levels[GM.Level - 1][0]);

		InitializeGrid(m_GridLevels.Levels[GM.Level - 1][0], m_GridLevels.Levels[GM.Level - 1][1]);
	}

	void InitializeGrid(int colums, int rows)
	{
		// Get current width and height of Screen
		RectTransform rtGrid = m_GridContainer.GetComponent<RectTransform>();
		float widthScreen = rtGrid.rect.width;
		float heightScreen = rtGrid.rect.height;

		// Calculate scale
		m_scaleX = (widthScreen / m_refWidth) * extraScale;
		m_scaleY = (heightScreen / m_refHeight) * extraScale;

		//m_scaleX = (m_scaleX + m_scaleY) / 2.5f;
		if(m_scaleX > m_scaleY){
			m_scaleX = m_scaleY;
		}

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
				InstantiateRows(i + 1, yRowOffset, m_GridContainer);
				InstantiateRows(i + 1, yRowOffset, m_GridContainerBackgrounds);
				InstantiateRows(i + 1, yRowOffset, m_GridContainerBorder);
			}
			else
			{
				InstantiateRows(i + 1, 0, m_GridContainer);
				InstantiateRows(i + 1, 0, m_GridContainerBackgrounds);
				InstantiateRows(i + 1, 0, m_GridContainerBorder);
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
					if (m_GridLevels.EmptyHivesLevels[GM.Level - 1][i][r] == (j + 1))
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
					InstantiateBorder(i,j);
					int num = UnityEngine.Random.Range(1, 7);
					m_Grid[i, j] = InstantiateHive(i, j);
					m_Grid[i, j].SetHive(num, i, j);

					if (j - 1 > 0)
					{
						float dist = Vector3.Distance(m_Grid[i, j].transform.position, m_Grid[i, j - 1].transform.position);
						if (distanceBetweenHives > dist)
						{
							distanceBetweenHives = (float)Math.Round((double)dist, 1);
						}
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
					if (m_Grid[i, j] != null)
					{
						if (m_Grid[i, j].GetHiveType != HiveType.empty)
						{
							m_Grid[i, j].SetHive(-1, i, j);
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
				float result = (float)Math.Round((double)Vector3.Distance(lastHive.transform.position, nextHive.transform.position), 1);
				if (result == distanceBetweenHives)
				{
					hiveIsNear = true;
				}
			}
		}
		return hiveIsNear;
	}

	public void DestroyNearBlockages(Hive currentHive)
	{
		Debug.Log("delete blockages");
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
				&& (m_Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]] != null && m_Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]].tag == "Blockage"))
				{
					DestroyHive(m_Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]]);
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
				&& (m_Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]] != null && m_Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]].tag == "Blockage"))
				{
					DestroyHive(m_Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]]);
				}
			}
		}
	}

	private bool IndexAroundHiveIsInTheGrid(int xValue, int yValue){
		bool isInTheGrid = false;
		if ((m_Grid.GetLength(0) > xValue) && (m_Grid.GetLength(1) > yValue))
		{
			isInTheGrid |= ((0 <= xValue) && (0 <= yValue));
		}
		return isInTheGrid;
	}

	public void CalculateScore(List<Hive> hives)
	{
		int numberOfBlue = 0, numberOfRed = 0, numberOfGreen = 0, numberOfYellow = 0, numberOfCyan = 0, numberOfMagenta = 0;

		foreach (Hive hive in hives)
		{
			switch (hive.Value)
			{
				case 1:
					numberOfRed++;
					break;
				case 2:
					numberOfGreen++;
					break;
				case 3:
					numberOfBlue++;
					break;
				case 4:
					numberOfYellow++;
					break;
				case 5:
					numberOfCyan++;
					break;
				case 6:
					numberOfMagenta++;
					break;
			}
		}
		m_score.UpdateScores(numberOfBlue, numberOfYellow, numberOfCyan, numberOfGreen, numberOfRed, numberOfMagenta);

		int resultNumber = 0;
		int macht = hives.Count;
		int currentNumber = hives[0].Value;
		resultNumber = currentNumber * 2;

		hives[hives.Count - 1].OnValueChanged((HiveType)resultNumber);

		int tempScore = int.Parse(Score.text);
		foreach (Hive hive in hives)
		{
			resultNumber += hive.Value;
		}
		resultNumber *= macht;
		resultNumber += tempScore;
		Score.text = resultNumber.ToString();
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
		DestroyHives(hives);
		for (int i = 0; i < m_colums; i++)
		{
			for (int j = 0; j < m_rows; j++)
			{
				if (m_Grid[i, j] == null)
				{
					int blockageLevel = GetIndexOfBlockage(i, j);
					Debug.Log(blockageLevel);
					if (blockageLevel < j || blockageLevel == -1)
					{
						bool hadToCreateNewIndex = false;
						m_Grid[i, j] = FindNextHive(i, j, out hadToCreateNewIndex);
						if (m_Grid[i, j] != null)
						{
							m_Grid[i, j].gameObject.AddComponent<MoveHive>();

							if (hadToCreateNewIndex)
							{
								m_Grid[i, j].gameObject.GetComponent<MoveHive>().BeginLerp(new Vector3(m_Grid[i, j].transform.localPosition.x, m_Grid[i, j].transform.localPosition.y + (yHiveOffset * (m_rows - j)), m_Grid[i, j].transform.localPosition.z), m_HivePositions[i, j], lerpSpeed, 0f);
							}
							else
							{
								m_Grid[i, j].gameObject.GetComponent<MoveHive>().BeginLerp(m_Grid[i, j].transform.localPosition, m_HivePositions[i, j], lerpSpeed);
							}

							m_Grid[i, j].OnPositionChanged(i, j);
						}
					}
					else
					{
						m_Grid[i, j] = FindNextHive(i, j, blockageLevel);
						if (m_Grid[i, j] != null)
						{
							m_Grid[i, j].gameObject.AddComponent<MoveHive>();
							m_Grid[i, j].gameObject.GetComponent<MoveHive>().BeginLerp(m_Grid[i, j].transform.localPosition, m_HivePositions[i, j], lerpSpeed);
							m_Grid[i, j].OnPositionChanged(i, j);
						}
					}
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

	Hive FindNextHive(int i, int j, out bool newHive)
	{
		newHive = false;
		// MAX INDEX
		if (j == m_rows - 1)
		{
			newHive = true;
			return InstantiateHive(i, j);
		}
		else
		{
			j++;
			// check the index above the last one, if its not empty, take it as the next block to fall
			if (m_Grid[i, j] != null && m_Grid[i, j].Value != emptyHiveLayer && m_Grid[i, j].Value != blockageLayer)
			{
				Hive tempHive = m_Grid[i, j];
				m_Grid[i, j] = null;
				return tempHive;
			}
			else
			{
				// if no next hive was found, search for the next
				return FindNextHive(i, j, out newHive);
			}
		}
	}

	Hive FindNextHive(int i, int j, int blockageIndex)
	{
		// MAX INDEX
		if (j == blockageIndex - 1)
		{
			return null;
		}
		else
		{
			j++;
			// check the index above the last one, if its not empty, take it as the next block to fall
			if (m_Grid[i, j] != null && m_Grid[i, j].Value != emptyHiveLayer)
			{
				Hive tempHive = m_Grid[i, j];
				m_Grid[i, j] = null;
				return tempHive;
			}
			else
			{
				// if no next hive was found, search for the next
				return FindNextHive(i, j, blockageIndex);
			}
		}
	}

	int GetIndexOfBlockage(int i, int j)
	{
		int index = -1;
		for (int jj = 0; jj < m_Grid.GetLength(1); jj++)
		{
			if (m_Grid[i, jj] != null && m_Grid[i, jj].GetHiveType == HiveType.blockage)
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
		for (int i = 0; i < m_colums; i++)
		{
			for (int j = 0; j < m_rows; j++)
			{
				if(m_Grid[i, j] != null){
					if (m_Grid[i, j].GetHiveType == hiveType && !notToBeSelected.Contains(m_Grid[i, j]))
					{
						temphives.Add(m_Grid[i, j]);
						m_Grid[i, j].SetSelectedImage();
					}
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
				int newI = UnityEngine.Random.Range(0, i - 1);
				int newJ = UnityEngine.Random.Range(0, j - 1);
				if (m_Grid[i, j].Value != 0 && m_Grid[newI, newJ].Value != 0)
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

	void InstantiateRows(int number, float offsetY, GameObject grid)
	{
		GameObject row = Instantiate(rowPrefab, new Vector3(0, 0 + offsetY, 0), rowPrefab.transform.rotation) as GameObject;
		row.transform.SetParent(grid.transform, false);
		row.name = string.Format("{0}{1}", "Row_", number);
	}

	Hive InstantiateHive(int x, int y)
	{
		GameObject prefabHive = Instantiate(hivePrefab, m_HivePositions[x, y], hivePrefab.transform.rotation) as GameObject;
		prefabHive.GetComponent<RectTransform>().localScale = new Vector3(m_scaleX, m_scaleX, m_scaleX);
		prefabHive.transform.SetParent(m_GridContainer.transform.GetChild(x), false);
		Hive tempHive = prefabHive.GetComponent<Hive>();
		tempHive.OnPositionChanged(x, y);
		tempHive.OnValueChanged(getRandomHiveType());
		return tempHive;
	}

	void InstantiateBorder(int x, int y){
		float scale = 1.3f;
		GameObject prefabHive = Instantiate(borderPrefab, m_HivePositions[x, y], hivePrefab.transform.rotation) as GameObject;
		prefabHive.GetComponent<RectTransform>().localScale = new Vector3(m_scaleX * scale, m_scaleX * scale, m_scaleX * scale);
		prefabHive.transform.SetParent(m_GridContainerBorder.transform.GetChild(x), false);
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

		number = Mathf.FloorToInt(number / (m_Grid.Length / 2));
		midNumber = number;
	}

	HiveType getRandomHiveType()
	{
		return (HiveType)UnityEngine.Random.Range(1, 7);
	}
}