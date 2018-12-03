using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour {

	/*
	protected float m_refWidth = 800f;
	protected float m_refHeight = 1280;
	protected float xHiveOffset = 85;
	protected float yRowOffset = 50;
	protected float extraScale = 0.1f;

	public GameObject m_GridContainerBackgrounds;
	public GameObject m_GridContainer;

	float lerpSpeed = 4;
	float offsetTop = 200;
	float yHiveOffset = 100;

	private Vector3[,] m_HivePositions;
	private Hive[,] m_Grid;

	private GridLevels m_GridLevels = new GridLevels();
	private GridInitializer gridInitializer = new GridInitializer();
	private Text scoreText;

	private GameObject hivePrefab;
	private GameManager m_GM;

	float distanceBetweenHives = Mathf.Infinity;
	float m_scaleX;
	float m_scaleY;
	float m_midScale;

	private int m_colums, m_rows;
	Score m_score;
	IceHivesController iceHivesController = new IceHivesController();

	// Use this for initialization
	public virtual void Start () {
		hivePrefab = Resources.Load("Hive") as GameObject;
		m_score = GameObject.Find("Scripts").GetComponent<Score>();
		scoreText = GameObject.Find("Score").GetComponent<Text>();
		m_GM = GameObject.Find("Scripts").GetComponent<GameManager>();
		m_colums = GridLevels.Levels[GetManager.Level - 1][0];
		m_rows = GridLevels.Levels[GetManager.Level - 1][1];

		gridInitializer.InitializeGrid(Colums, Rows);
	}
	
	// Update is called once per frame
	public virtual void Update () {
		
	}

	public virtual void Awake(){}

	public virtual Hive InstantiateHive(int x, int y)
	{
		GameObject prefabHive = Instantiate(hivePrefab, m_HivePositions[x, y], hivePrefab.transform.rotation) as GameObject;
		prefabHive.GetComponent<RectTransform>().localScale = new Vector3(m_scaleX, m_scaleX, m_scaleX);
		prefabHive.transform.SetParent(m_GridContainer.transform.GetChild(x), false);
		Hive tempHive = prefabHive.GetComponent<Hive>();
		tempHive.OnPositionChanged(x, y);
		tempHive.OnValueChanged(getRandomValue());
		return tempHive;
	}

	int getRandomValue()
	{
		return UnityEngine.Random.Range(1, 7);
	}

	public virtual void DestroyHive(Hive hive)
	{
		StartCoroutine(hive.transform.Scale(Vector3.zero, 0.2f, hive));
		RemoveFromGrid(hive);
	}

	void RemoveFromGrid(Hive hive)
	{
		for (int i = 0; i < Colums; i++)
		{
			for (int j = 0; j < Rows; j++)
			{
				if (Grid[i, j] == hive)
				{
					Grid[i, j] = null;
				}
			}
		}
	}

	public Score GetScore{
		get{
			return m_score;
		}
	}

	public Text ScoreText{
		get {
			return scoreText;
		}
	}

	public GameObject HivePrefab{
		get{
			return hivePrefab;
		}
	}

	public int Colums{
		get{
			return m_colums;
		}
	}

	public int Rows
	{
		get
		{
			return m_rows;
		}
	}

	public IceHivesController IceHivesController{
		get{
			return iceHivesController;
		}
	}

	public Hive[,] Grid {
		get{
			return m_Grid;
		}
		set{
			m_Grid = value;
		}
	}

	public float ScaleX
	{
		get
		{
			return m_scaleX;
		}
		set
		{
			m_scaleX = value;
		}
	}

	public float ScaleY
	{
		get
		{
			return m_scaleY;
		}
		set
		{
			m_scaleY = value;
		}
	}

	public GridLevels GridLevels {
		get{
			return m_GridLevels;
		}
	}

	public GameManager GetManager {
		get{
			return m_GM;
		}
	}

	public float DistanceBetweenHives {
		get
		{
			return distanceBetweenHives;
		}
		set
		{
			distanceBetweenHives = value;
		}
	}

	public Vector3[,] HivePositions {
		get{
			return m_HivePositions;
		}
		set
		{
			m_HivePositions = value;
		}
	}

	public float LerpSpeed {
		get{
			return lerpSpeed;
		}
	}

	public float YHiveOffset {
		get
		{
			return yHiveOffset;
		}
		set
		{
			yHiveOffset = value;
		}
	}

*/
}
