using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{

	//Change me to change the touch phase used.
	TouchPhase touchPhase = TouchPhase.Moved;
	public GraphicRaycaster myGRaycaster;

	// 4 Lists to controll them all
	Stack<GameObject> selectedHives = new Stack<GameObject>();
	List<Vector3> lineHives = new List<Vector3>();
	List<Hive> listHives = new List<Hive>();
	List<Hive> specialSelectionHives = new List<Hive>();

	// Keeps track of current/last hive selected
	string currentHive = "";
	Hive lastHive;

	// Get functions of grid
	GridController grid;

	//Get functions of LR
	LineRenderController LRController;

	//Get function of GM
	GameManager GM;

	//Handle special moment
	bool specialSelectionActivated = false;

	private void Start()
	{
		grid = GameObject.Find("Scripts").GetComponent<GridController>();
		LRController = GameObject.Find("LineRenderer").GetComponent<LineRenderController>();
		GM = GameObject.Find("Scripts").GetComponent<GameManager>();
	}

	void Update()
	{
		//We check if we have more than one touch happening.
		//We also check if the first touches phase is Ended (that the finger was lifted)
		if (Input.touchCount > 0 && (Input.GetTouch(0).phase == touchPhase || Input.GetTouch(0).phase == TouchPhase.Began))
		{
			List<RaycastResult> rayResults = new List<RaycastResult>();
			PointerEventData ped = new PointerEventData(null);
			ped.position = Input.GetTouch(0).position;
			myGRaycaster.Raycast(ped, rayResults);
			foreach (RaycastResult result in rayResults)
			{
				GameObject resultObj = result.gameObject;
				if (resultObj.tag == "Hive")
				{
					Hive resultHive = result.gameObject.GetComponent<Hive>();
					// Is there a hive selected?
					if (selectedHives.Count > 0)
					{
						//Is it the same as last frame?
						if (currentHive != resultObj.name)
						{
							GameObject tempHive = null;
							if (selectedHives.Count > 1)
							{
								tempHive = selectedHives.Pop();
							}
							if (specialSelectionActivated)
							{
								// Are we deselecting? Remove all special selection again
								if (selectedHives.Peek() == resultObj)
								{
									if (resultHive == lastHive)
									{
										specialSelectionHives = grid.DeselectAllHivesOfSameValue(specialSelectionHives);
										specialSelectionActivated = false;
										lineHives.RemoveAt(lineHives.Count - 1);
									}
									else
									{
										selectedHives.Push(tempHive);
									}
								}
								else
								{
									selectedHives.Push(tempHive);
								}
							}
							else
							{
								// Are we deselecting?
								if (selectedHives.Peek() == resultObj)
								{
									if (tempHive != null)
									{
										lineHives.Remove(tempHive.transform.position);
										if (tempHive.GetComponent<Hive>() != null)
										{
											Hive hive = tempHive.GetComponent<Hive>();
											listHives.Remove(hive);
											lastHive = resultHive;
											hive.SetNormalImage();
										}
									}
								}
								else
								{
									// If not deselecting, repush
									if (tempHive != null)
									{
										selectedHives.Push(tempHive);
									}
									// If selected hive is already in the Stack, do nothing
									if (grid.IsHiveNear(lastHive, resultHive))
									{
										if (!selectedHives.Contains(resultObj))
										{
											if (lastHive.Value == resultHive.Value)
											{
												selectedHives.Push(resultObj);
												lastHive = resultHive;
												resultHive.SetSelectedImage();
												lineHives.Add(resultObj.transform.position);
												listHives.Add(resultHive);
											}
										}
										else
										{
											/* 
											// Make a full round, reach first hive again
											if (resultHive == listHives[0])
											{
												selectedHives.Push(resultObj);
												lineHives.Add(resultObj.transform.position);
												specialSelectionHives = grid.ReturnAllHivesOfSameValue(resultHive.GetHiveType, listHives);
												specialSelectionActivated = true;
											}*/
										}
									}
								}
							}
							LRController.UpdatePoints(lineHives, Vector3.zero, listHives[0].GetHiveType);
						}
					}
					else
					{
						// Push the first hive of the list
						selectedHives.Push(resultObj);
						lineHives.Add(resultObj.transform.position);
						listHives.Add(resultHive);
						lastHive = resultHive;
						resultHive.SetSelectedImage();
					}
					currentHive = resultObj.name;
				}
				if (!specialSelectionActivated && listHives.Count > 0)
					LRController.UpdatePoints(lineHives, Input.GetTouch(0).position, listHives[0].GetHiveType);
			}
		}
		// Deselect all hives on touch end.
		// Check if it's the right result.
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			if (listHives.Count > 1)
			{
				if (specialSelectionActivated)
				{
					//Store last index in temporary hive, otherwise it gets deleted
					Hive tempHive = listHives[0];
					listHives.RemoveAt(0);

					foreach (Hive hive in specialSelectionHives)
					{
						listHives.Add(hive);
					}

					//This hive gets the sum of all hives
					listHives.Add(tempHive);
				}
				grid.CalculateScore(listHives);
				grid.UpdateGrid(listHives);
				GM.MovesLeft--;
			}

			if (listHives.Count > 0)
			{
				foreach (Hive Hive in listHives)
				{
					Hive.SetNormalImage();
				}
			}

			specialSelectionActivated = false;
			selectedHives.Clear();
			lineHives.Clear();
			listHives.Clear();
			LRController.UpdatePoints(lineHives, Vector3.zero, HiveType.empty);
		}
	}
}
