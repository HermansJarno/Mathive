﻿using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
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
				if (checkIfIndexIsInTheGrid(currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i])
				&& (gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]] != null && gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]].tag == "Blockage"))
				{
					IceHive iceHive = gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]].gameObject.GetComponent<IceHive>();
					iceHive.LowerNumberOfHitsLeft();
					if(iceHive.NumberOfTimesLeftToHit == 0){
						DestroyHive(gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]]);
					}
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
				if (checkIfIndexIsInTheGrid(currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i])
				&& (gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]] != null && gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]].tag == "Blockage"))
				{
					IceHive iceHive = gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]].gameObject.GetComponent<IceHive>();
					iceHive.LowerNumberOfHitsLeft();
					if (iceHive.NumberOfTimesLeftToHit == 0)
					{
						DestroyHive(gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]]);
					}
				}
			}
		}
	}

	private bool checkIfIndexIsInTheGrid(int xValue, int yValue){
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
		if(checkIfMovesLeftInGrid()){
			Invoke("SetInputActive", 0.1f * highestNumberOfMoves);
		}else{
			GameObject text = Resources.Load("PopUpNoOptionsLeft") as GameObject;
			GameObject prefabText = Instantiate(text, text.transform.position, text.transform.rotation) as GameObject;
			prefabText.transform.SetParent(GameObject.Find("MidPanel").transform, false);
			Destroy(prefabText, 2.8f);
			Invoke("ShuffleGrid", 3f);
		}	
	}

	bool checkIfNormalHive(Hive currentHive){
		bool normalHive = false;
		if(currentHive != null){
			if (currentHive.GetHiveType == HiveType.green || 
			currentHive.GetHiveType == HiveType.blue ||
			currentHive.GetHiveType == HiveType.cyan ||
			currentHive.GetHiveType == HiveType.yellow ||
			currentHive.GetHiveType == HiveType.magenta ||
			currentHive.GetHiveType == HiveType.red){
				normalHive = true;
			}
		} 
		return normalHive;
	}

	bool checkIfMovesLeftInGrid(){
		bool movesLeftInGrid = false;

		for (int i = 0; i < gridManager.Columns; i++)
		{
			for (int j = 0; j < gridManager.Rows; j++)
			{
				if(gridManager.Grid[i,j] != null && checkIfNormalHive(gridManager.Grid[i,j]) && checkIfMovesAroundHive(gridManager.Grid[i,j])){
					movesLeftInGrid = true;
					break;
				}
			}
			if(movesLeftInGrid){
				break;
			}
		}
		return movesLeftInGrid;
	}

	bool checkIfMovesAroundHive(Hive currentHive){
		bool optionalMovesLeft = false;
		HiveType type = currentHive.GetHiveType;

		// even number search indexes below
		if ((currentHive.X % 2) == 0)
		{
			// left/right
			int[] xOffsets = { -1, 1, 0, 0, -1, 1 };
			// Up/Down
			int[] yOffsets = { 0, 0, -1, 1, 1, 1};

			for (int i = 0; i < xOffsets.Length; i++)
			{
				if (checkIfIndexIsInTheGrid(currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i])
				&& (gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]] != null && gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]].GetHiveType == type))
				{
					optionalMovesLeft = true;
					break;
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
				if (checkIfIndexIsInTheGrid(currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i])
				&& (gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]] != null && gridManager.Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]].GetHiveType == type))
				{
					optionalMovesLeft = true;
					break;
				}
			}
		}
		return optionalMovesLeft;
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
		Debug.Log("Shuffle grid");
		Shuffle(gridManager.Grid);
		if(checkIfMovesLeftInGrid()){
			Invoke("SetInputActive", 1f);
		}else{
			ShuffleGrid();
		}
	}

	void Shuffle(Hive[,] array)
	{
		for (int i = 0; i < gridManager.Columns; i++)
		{
			for (int j = 0; j < gridManager.Rows; j++)
			{
				if (checkIfNormalHive(gridManager.Grid[i,j])) {
					Hive newHive = SearchRandomNormalHive();
					Swap(i, j, newHive.X, newHive.Y, array);
				}
			}
		}
	}

	Hive SearchRandomNormalHive(){
		int newI = 0;
		int newJ = 0;
		do{
			newI = UnityEngine.Random.Range(0, gridManager.Columns -1);
			newJ = UnityEngine.Random.Range(0, gridManager.Rows - 1);
		}while(!checkIfNormalHive(gridManager.Grid[newI,newJ]));
		return gridManager.Grid[newI,newJ];
	}

	void Swap(int currentI, int currentJ, int newI, int newJ, Hive[,] array)
	{
		HiveType temp = array[currentI, currentJ].GetHiveType;
		array[currentI, currentJ].OnValueChanged(array[newI, newJ].GetHiveType);
		array[newI, newJ].OnValueChanged(temp);
	}

	public List<Hive> DeselectSpecialSelection(List<Hive> hives)
	{
		foreach (Hive hive in hives)
		{
			hive.SwitchState();
		}
		return null;
	}

	public List<Hive> SpecialSelection(List<Hive> hives){
		List<Hive> tempHives = new List<Hive>();
		switch(hives.Count){
			case 3: tempHives = SpecialSelectionWith3(hives);
			break;
			case 4: tempHives = SpecialSelectionWith4(hives);
			break;
		}

		foreach (Hive hive in tempHives)
		{
			hive.SwitchState();
		}

		//results need to be added to the full list of hives to delete
		return tempHives;
	}

	private List<Hive> SpecialSelectionWith3(List<Hive> hives){
		List<Hive> tempHives = new List<Hive>();

		int mostOccuringIndex = getMostOccuringColumn(hives);
		int leastOccuringIndex = getLeastOccuringColumn(hives);
		int highestY = getHighestYIndex(hives);

		if((mostOccuringIndex + 1) % 2 == 0){
			Debug.Log("even hoogste");
			if(leastOccuringIndex < mostOccuringIndex){
				// LINKS
				foreach (Hive hive in hives)
				{
					if(hive.X == leastOccuringIndex){
						int x = hive.X;
						int y = hive.Y;
						if(checkIfValidIndex(x, y + 1)) tempHives.Add(gridManager.Grid[x, y + 1]);
						if(checkIfValidIndex(x, y - 1)) tempHives.Add(gridManager.Grid[x, y - 1]);
					}else if(hive.Y == highestY){
						int x = hive.X;
						int y = hive.Y;
						if(checkIfValidIndex(x + 1, y - 1)) tempHives.Add(gridManager.Grid[x + 1, y - 1]);
					}
				}
			}else{
				// RECHTS
				foreach (Hive hive in hives)
				{
					if(hive.X == leastOccuringIndex){
						int x = hive.X;
						int y = hive.Y;
						if(checkIfValidIndex(x, y + 1)) tempHives.Add(gridManager.Grid[x, y + 1]);
						if(checkIfValidIndex(x, y - 1)) tempHives.Add(gridManager.Grid[x, y - 1]);
					}else if(hive.Y == highestY){
						int x = hive.X;
						int y = hive.Y;
						if(checkIfValidIndex(x - 1, y - 1)) tempHives.Add(gridManager.Grid[x - 1, y - 1]);
					}
				}
			}
		}else{
				if(leastOccuringIndex < mostOccuringIndex){
				// LINKS
				foreach (Hive hive in hives)
				{
					if(hive.X == leastOccuringIndex){
						int x = hive.X;
						int y = hive.Y;
						if(checkIfValidIndex(x, y + 1)) tempHives.Add(gridManager.Grid[x, y + 1]);
						if(checkIfValidIndex(x, y - 1)) tempHives.Add(gridManager.Grid[x, y - 1]);
					}
					else if(hive.Y == highestY){
						int x = hive.X;
						int y = hive.Y;
						if(checkIfValidIndex(x + 1, y)) tempHives.Add(gridManager.Grid[x + 1, y]);
					}
				}
			}else{
				// RECHTS
				foreach (Hive hive in hives)
				{
					if(hive.X == leastOccuringIndex){
						int x = hive.X;
						int y = hive.Y;
						if(checkIfValidIndex(x, y + 1)) tempHives.Add(gridManager.Grid[x, y + 1]);
						if(checkIfValidIndex(x, y - 1)) tempHives.Add(gridManager.Grid[x, y - 1]);
					}else if(hive.Y == highestY){
						int x = hive.X;
						int y = hive.Y;
						if(checkIfValidIndex(x - 1, y)) tempHives.Add(gridManager.Grid[x - 1, y]);
					}
				}
			}
		}

		return tempHives;
	}

	private List<Hive> SpecialSelectionWith4(List<Hive> hives){
		List<int> columnIndexes = new List<int>();
		foreach (Hive hive in hives)
		{
		   if(!columnIndexes.Contains(hive.X)) columnIndexes.Add(hive.X);
		}

		List<Hive> tempHives = new List<Hive>();

		int mostOccuringIndex = getMostOccuringColumn(hives);
		int leastOccuringIndex = getLeastOccuringColumn(hives);
		int highestY = getHighestYIndex(hives);

		if(columnIndexes.Count == 3){
			foreach (Hive hive in hives)
			{
				if(hive.X != mostOccuringIndex){
					int x = hive.X;
					int y = hive.Y;
					if(checkIfValidIndex(x, y + 1)) tempHives.Add(gridManager.Grid[x, y + 1]);
					if(checkIfValidIndex(x, y - 1)) tempHives.Add(gridManager.Grid[x, y - 1]);
				} 
			}
		}else{
			List<int> hivesEvenColumn = new List<int>();
			List<int> hivesOddColumn = new List<int>();
			int indexXEven = 0;
			int indexXOdd = 0;
			foreach (Hive hive in hives)
			{
				if((hive.X + 1) % 2 == 0){
					hivesEvenColumn.Add(hive.Y);
					indexXEven = hive.X;
				}else{
					hivesOddColumn.Add(hive.Y);						
					indexXOdd = hive.X;
				}
			}
			hivesEvenColumn.Sort();
			hivesEvenColumn.Reverse();
			hivesOddColumn.Sort();
			hivesOddColumn.Reverse();

			int hightestEvenIndex = hivesEvenColumn[0];
			int lowestEvenIndex = hivesEvenColumn[1];
			int hightestOddIndex = hivesOddColumn[0];
			int lowestOddIndex = hivesOddColumn[1];

			if(getCountOfHighestHive(hives) == 1){
				// even hoogste
				if(indexXEven > indexXOdd){
					//left downwards
					// if highest Y only occurs one time, then the highest index is in the even column
					// then make the odd column the same height.
					if(checkIfValidIndex(indexXOdd, hightestEvenIndex)) tempHives.Add(gridManager.Grid[indexXOdd, hightestEvenIndex]);
					// then make right hive same height as orginal highest hive from odd column
					if (checkIfValidIndex(indexXEven + 1, hightestOddIndex)) tempHives.Add(gridManager.Grid[indexXEven + 1, hightestOddIndex]);
					// then add a 3rd hive beneath the 2 orginal even hives
					if (checkIfValidIndex(indexXEven, lowestEvenIndex - 1)) tempHives.Add(gridManager.Grid[indexXEven, lowestEvenIndex - 1]);
					// then left hive is same height as the lowest orginal even hive
					if (checkIfValidIndex(indexXOdd - 1, lowestEvenIndex)) tempHives.Add(gridManager.Grid[indexXOdd - 1, lowestEvenIndex]);
				}else{
					//right downwards
					// then make the odd column the same height.
					if(checkIfValidIndex(indexXOdd, hightestEvenIndex)) tempHives.Add(gridManager.Grid[indexXOdd, hightestEvenIndex]);
					// then make right hive same height as orginal lowest hive from even column
					if (checkIfValidIndex(indexXOdd + 1, lowestEvenIndex)) tempHives.Add(gridManager.Grid[indexXOdd + 1, lowestEvenIndex]);
					// then add a 3rd hive beneath the 2 orginal even hives
					if (checkIfValidIndex(indexXEven, lowestEvenIndex - 1)) tempHives.Add(gridManager.Grid[indexXEven, lowestEvenIndex - 1]);
					// then left hive is same height as the lowest orginal even hive
					if (checkIfValidIndex(indexXEven - 1, hightestOddIndex)) tempHives.Add(gridManager.Grid[indexXEven - 1, hightestOddIndex]);
				}
			}else{
				if(indexXEven < indexXOdd){
					Debug.Log("left");
					//left downwards
					if(checkIfValidIndex(indexXEven, hightestEvenIndex + 1)) tempHives.Add(gridManager.Grid[indexXEven, hightestEvenIndex + 1]);
					if (checkIfValidIndex(indexXOdd + 1, hightestOddIndex)) tempHives.Add(gridManager.Grid[indexXOdd + 1, hightestOddIndex]);
					if (checkIfValidIndex(indexXOdd, lowestOddIndex - 1)) tempHives.Add(gridManager.Grid[indexXOdd, lowestOddIndex - 1]);
					if (checkIfValidIndex(indexXEven - 1, lowestEvenIndex)) tempHives.Add(gridManager.Grid[indexXEven - 1, lowestEvenIndex]);
				}else{
					Debug.Log("right");
					//right downwards
					if(checkIfValidIndex(indexXEven, hightestEvenIndex + 1)) tempHives.Add(gridManager.Grid[indexXEven, hightestEvenIndex + 1]);
					if (checkIfValidIndex(indexXEven + 1, lowestOddIndex)) tempHives.Add(gridManager.Grid[indexXEven + 1, lowestOddIndex]);
					if (checkIfValidIndex(indexXOdd, lowestOddIndex - 1)) tempHives.Add(gridManager.Grid[indexXOdd, lowestOddIndex - 1]);
					if (checkIfValidIndex(indexXOdd - 1, hightestEvenIndex)) tempHives.Add(gridManager.Grid[indexXOdd - 1, hightestEvenIndex]);
				}
				// oneven hoogste
				// if highest Y occurs 2 times, then the the highest index is visually! the odd column
				// then make the even column the highest (extra hive).
				// left hive should be a same height as the odd column highest
				// the original odd column needs a 3rd hive beneath the 2 existing
				// right hive should be same height as the lowest original hive from the odd column
			}
		}

		return tempHives;
	}

	private bool checkIfValidIndex(int x, int y){
		return checkIfIndexIsInTheGrid(x, y) && checkIfNormalHive(gridManager.Grid[x, y]);
	}

	private int getMostOccuringColumn(List<Hive> hives){
		List<int> columnIndexes = new List<int>();
		foreach (Hive hive in hives)
		{
		   columnIndexes.Add(hive.X);
		}

		return columnIndexes.GroupBy(i=>i).OrderByDescending(grp=>grp.Count()).Select(grp=>grp.Key).First();
	}

	private int getLeastOccuringColumn(List<Hive> hives){
		List<int> columnIndexes = new List<int>();
		foreach (Hive hive in hives)
		{
		   columnIndexes.Add(hive.X);
		}

		return columnIndexes.GroupBy(i=>i).OrderByDescending(grp=>grp.Count()).Select(grp=>grp.Key).Last();
	}

	private int getHighestYIndex(List<Hive> hives){
		List<int> columnIndexes = new List<int>();
		foreach (Hive hive in hives)
		{
		   columnIndexes.Add(hive.Y);
		}

		return columnIndexes.GroupBy(i=>i).OrderByDescending(grp=>grp.Key).Select(grp=>grp.Key).First();
	}
			
	private int getLowestYIndex(List<Hive> hives)
	{
		List<int> columnIndexes = new List<int>();
		foreach (Hive hive in hives)
		{
			columnIndexes.Add(hive.Y);
		}
		return columnIndexes.GroupBy(i => i).OrderByDescending(grp => grp.Key).Select(grp => grp.Key).Last();
	}
			   
	private int getCountOfHighestHive(List<Hive> hives)
	{
		int highestIndex = getHighestYIndex(hives);
		int count = 0;

		List<int> columnIndexes = new List<int>();
		foreach (Hive hive in hives)
		{
			if (hive.Y == highestIndex) count++;
		}

		return count;
	}
}