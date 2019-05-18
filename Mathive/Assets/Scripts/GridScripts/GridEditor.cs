using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridEditor : MonoBehaviour
{
    public GameObject m_GridContainerBackgrounds;
    public GameObject m_GridContainerBorder;
    public GameObject m_GridContainer;

	public GameSpecs _gameSpecs;

	public GraphicRaycaster _raycaster;
    
    [SerializeField] float yRowOffset = 50;
    [SerializeField] float xHiveOffset = 85;
    [SerializeField] float yHiveOffset = 100;
    private GameObject rowPrefab;
    public HiveType selectedHiveToEdit = HiveType.empty;
    public int columns = 10;
    public int rows = 10;
    protected Vector3[,] hivePositions;
    protected Hive[,] Grid;
    private float _scale = 1;
    private GameObject borderPrefab;
	private GameObject hivePrefab;
	private	TouchPhase touchPhase = TouchPhase.Moved;

    // Start is called before the first frame update
    void Start()
    {
		_scale = _gameSpecs.getScale();
        yRowOffset *= _scale;
		xHiveOffset *= _scale;
		yHiveOffset *= _scale;

        borderPrefab = Resources.Load("HiveBorder") as GameObject;
		hivePrefab = Resources.Load("Hive") as GameObject;
        rowPrefab = Resources.Load("Row_") as GameObject;
        for (int i = 0; i < columns; i++)
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
        spawnGrid();
    }

	void Update(){
			//We check if we have more than one touch happening.
			//We also check if the first touches phase is Ended (that the finger was lifted)
			if (Input.touchCount > 0 && (Input.GetTouch(0).phase == touchPhase || Input.GetTouch(0).phase == TouchPhase.Began))
			{
				List<RaycastResult> rayResults = new List<RaycastResult>();
				PointerEventData ped = new PointerEventData(null);
				ped.position = Input.GetTouch(0).position;
				_raycaster.Raycast(ped, rayResults);
				Debug.Log("got a touch");
				foreach (RaycastResult result in rayResults)
				{
					Debug.Log("not yet hive");
					GameObject resultObj = result.gameObject;
					Debug.Log(resultObj.gameObject.name);
					if (resultObj.tag == "Hive")
					{
						Debug.Log("hive alright");
						Hive resultHive = result.gameObject.GetComponent<Hive>();
						resultHive.SetHive((int)selectedHiveToEdit, resultHive.X, resultHive.Y);
					}
				}
			}
	}
	
	void UpdateGrid(Hive selectedHive){
		for (int i = 0; i < columns; i++)
		{
			for (int j = 0; j < rows; j++)
			{
				
			}
		}
	}

    void InstantiateRows(int number, float offsetY, GameObject gridContainer)
	{
		GameObject row = Instantiate(rowPrefab, new Vector3(0, 0 + offsetY, 0), rowPrefab.transform.rotation) as GameObject;
		row.transform.SetParent(gridContainer.transform, false);
		row.name = string.Format("{0}{1}", "Row_", number);
	}

    void InstantiateBorder(int x, int y){
		float scale = 1.3f;
		GameObject prefabHive = Instantiate(borderPrefab, hivePositions[x, y], borderPrefab.transform.rotation) as GameObject;
		prefabHive.GetComponent<RectTransform>().localScale = new Vector3(_scale * scale, _scale * scale, _scale * scale);
		prefabHive.transform.SetParent(m_GridContainerBorder.transform.GetChild(x), false);
	}

	public virtual Hive InstantiateHive(int x, int y)
    {
        GameObject prefabHive = Instantiate(hivePrefab, hivePositions[x, y], hivePrefab.transform.rotation) as GameObject;
        prefabHive.GetComponent<RectTransform>().localScale = new Vector3(_scale, _scale, _scale);
        prefabHive.transform.SetParent(m_GridContainer.transform.GetChild(x), false);
        Hive tempHive = prefabHive.GetComponent<Hive>();
        tempHive.OnPositionChanged(x, y);
        return tempHive;
    }

    void spawnGrid(){
        GenerateGridPositions();
        Grid = new Hive[columns, rows];
        for (int i = 0; i < columns; i++)
		{
			for (int j = 0; j < rows; j++)
			{
				if (Grid[i, j] == null)
				{
					InstantiateBorder(i,j);
					Grid[i, j] = InstantiateHive(i,j);
					Grid[i, j].OnValueChanged((HiveType)0);
					Grid[i, j].SetVisible();
				}
			}
		}
    }

    void GenerateGridPositions(){
        float _width = xHiveOffset * columns;
		float _height = yHiveOffset * rows;

		float xPosition = -(_width / 2);
		float yPosition = -(_height / 2);
        hivePositions = new Vector3[columns, rows];
        // Create The Positions
		for (int i = 0; i < columns; i++)
		{
			for (int j = 0; j < rows; j++)
			{
				hivePositions[i, j] = new Vector3(xPosition + (xHiveOffset / 2), yPosition, 0);
				yPosition += yHiveOffset;
			}
			xPosition += xHiveOffset;
			yPosition = -(_height / 2);
		}
    }

	public void SetSelectedHive(int index){
		selectedHiveToEdit = (HiveType)index;
	}


}
