using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject m_GridContainerBackgrounds;
	public GameObject m_GridContainerBorder;
	public GameObject m_GridContainer;
	[SerializeField] float m_refWidth = 600f;
	[SerializeField] float m_refHeight = 1280;
	[SerializeField] float xHiveOffset = 85;
	[SerializeField] float yRowOffset = 50;

    GridInitializer gridInitializer;

    public float YRowOffset{
        get {
            return yRowOffset;
        }
        set {
            yRowOffset = value;
        }
    }

    public float XHiveOffset{
        get {
            return xHiveOffset;
        }
        set {
            xHiveOffset = value;
        }
    }

    public float RefHeight{
        get {
            return m_refHeight;
        }
    }

    public float RefWidth{
        get {
            return m_refWidth;
        }
    }

	[SerializeField] float yHiveOffset = 100;

    public float YHiveOffset{
        get {
            return yHiveOffset;
        }
        set {
            yHiveOffset = value;
        }
    }

	[SerializeField] float lerpSpeed = 4;

    public float LerpSpeed{
        get{
           return lerpSpeed; 
        }
    }

	protected Vector3[,] m_HivePositions;

    public Vector3[,] HivePositions{
        get{
            return m_HivePositions;
        }
        set
        {
            m_HivePositions = value;
        }
    }

	protected Hive[,] m_Grid;

    public Hive[,] Grid{
        get{
            return m_Grid;
        }
        set{
            m_Grid = value;
        }
    }


	private GridLevels m_GridLevels = new GridLevels();
	//private Text Score;

    public GridLevels GridLevels{
        get{
            return m_GridLevels;
        }
    }

	protected GameObject hivePrefab;

    public GameObject HivePrefab{
        get{
            return hivePrefab;
        }
    }

	private GameObject borderPrefab;

    public GameObject BorderPrefab{
        get{
            return borderPrefab;
        }
    }

	private GameObject rowPrefab;

    public GameObject RowPrefab{
        get{
            return rowPrefab;
        }
    }

	private GameManager _GM;

    public GameManager GameManager{
        get{
            return _GM;
        }
    }

	protected float distanceBetweenHives = Mathf.Infinity;

    public float DistanceBetweenHives{
        get{
            return distanceBetweenHives;
        }
        set{
            distanceBetweenHives = value;
        }
    }

	protected float m_scaleX;

    public float ScaleX{
        get{
            return m_scaleX;
        }
        set{
            m_scaleX = value;
        }
    }

	protected float m_scaleY;
    public float ScaleY{
        get{
            return m_scaleY;
        }
        set{
            m_scaleY = value;
        }
    }
    
	[SerializeField] float extraScale = 0.1f;

    public float ExtraScale {
        get{
            return extraScale;
        }
    }
	float delayLerp = 0.1f;

    public float DelayLerp {
        get{
            return delayLerp;
        }
    }

	private int m_colums, m_rows;

    public int Columns{
        get {
            return m_colums;
        }set{
            m_colums = value;
        }
    }

    public int Rows{
        get {
            return m_rows;
        }set{
            m_rows = value;
        }
    }

	Score m_score;

	private int emptyHiveLayer = 0;

    
	void Start()
	{
		m_score = GameObject.Find("Scripts").GetComponent<Score>();
		//Score = GameObject.Find("Score").GetComponent<Text>();
		hivePrefab = Resources.Load("Hive") as GameObject;
		borderPrefab = Resources.Load("HiveBorder") as GameObject;
		rowPrefab = Resources.Load("Row_") as GameObject;
		_GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        gridInitializer = GameObject.Find("GridManager").GetComponent<GridInitializer>();
        gridInitializer.InitializeGrid();

	}

    public virtual Hive InstantiateHive(int x, int y)
	{
		GameObject prefabHive = Instantiate(hivePrefab, m_HivePositions[x, y], hivePrefab.transform.rotation) as GameObject;
		prefabHive.GetComponent<RectTransform>().localScale = new Vector3(m_scaleX, m_scaleX, m_scaleX);
		prefabHive.transform.SetParent(m_GridContainer.transform.GetChild(x), false);
		Hive tempHive = prefabHive.GetComponent<Hive>();
		tempHive.OnPositionChanged(x, y);
		tempHive.OnValueChanged(getRandomHiveType());
		return tempHive;
	}

	HiveType getRandomHiveType()
	{
		return (HiveType)UnityEngine.Random.Range(1, 7);
	}
}
