using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHivesController : MonoBehaviour {
	/*
	private GridManager gridManager;


	// Use this for initialization
	void Start()
	{
		gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
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
			int[] yOffsets = { 0, 0, -1, 1, -1, -1 };

			if (currentHive.X + 1 < Colums - 1)
			{
				if (Grid[currentHive.X + 1, currentHive.Y] != null)
				{
					if (Grid[currentHive.X + 1, currentHive.Y].GetHiveType == HiveType.blockage)
					{
						DestroyHive(Grid[currentHive.X + 1, currentHive.Y]);
					}
				}
			}
			else if (currentHive.X - 1 > 0)
			{
				if (Grid[currentHive.X - 1, currentHive.Y] != null)
				{
					if (Grid[currentHive.X - 1, currentHive.Y].Value == -1)
					{
						DestroyHive(Grid[currentHive.X - 1, currentHive.Y]);
					}
				}
			}
			else if (currentHive.Y + 1 < Rows - 1)
			{
				Debug.Log("lower than max Y");
				if (Grid[currentHive.X, currentHive.Y + 1] != null)
				{
					if (Grid[currentHive.X, currentHive.Y + 1].Value == -1)
					{
						DestroyHive(Grid[currentHive.X, currentHive.Y + 1]);
					}
				}
			}
			else if (currentHive.Y - 1 > 0)
			{
				if (Grid[currentHive.X, currentHive.Y - 1] != null)
				{
					if (Grid[currentHive.X, currentHive.Y - 1].Value == -1)
					{
						DestroyHive(Grid[currentHive.X, currentHive.Y - 1]);
					}
				}
			}
		}
		else
		{
			
			// Left/right
			int[] xOffsets = { -1, 1, 0, 0, -1, 1 };
			// Up/Down
			int[] yOffsets = { 0, 0, -1, 1, 1, 1 };
			for (int i = 0; i < xOffsets.Length; i++)
			{
				if ((currentHive.X + xOffsets[i] < m_rows - 1 || currentHive.X + xOffsets[i] > 0) && (currentHive.Y + yOffsets[i] < m_colums - 1 || currentHive.Y + yOffsets[i] > 0))
				{
					if (m_Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]] != null)
					{
						if (m_Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]].Value == -1)
						{
							DestroyHive(m_Grid[currentHive.X + xOffsets[i], currentHive.Y + yOffsets[i]]);
						}
					}
				}
			}
		}
	}*/
}
