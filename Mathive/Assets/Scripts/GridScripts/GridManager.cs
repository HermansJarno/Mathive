using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject m_GridContainerBackgrounds;
    public GameObject m_GridContainerBorder;
    public GameObject m_GridContainer;
    public GameObject m_GridBackgroundMakeupContainer;
    [SerializeField] float m_refWidth = 600f;
    [SerializeField] float m_refHeight = 1280;
    [SerializeField] float xHiveOffset = 85;
    [SerializeField] float yRowOffset = 50;
    GridInitializer gridInitializer;
    [SerializeField] float yHiveOffset = 100;
    [SerializeField] float lerpSpeed = 4;
    protected Vector3[,] m_HivePositions;
    protected Hive[,] m_Grid;
    private GridLevels m_GridLevels = new GridLevels();
    //private Text Score;
    protected GameObject hivePrefab;
    private GameObject borderPrefab;
    private GameObject backgroundPrefab;
    private GameObject rowPrefab;
    private GameManager _GM;
    protected float distanceBetweenHives = Mathf.Infinity;
    protected float m_scaleX;
    protected float m_scaleY;
    [SerializeField] float extraScale = 0.1f;
    float delayLerp = 0.1f;
    private int m_colums, m_rows;

    void Start()
    {
        hivePrefab = Resources.Load("Hive") as GameObject;
        borderPrefab = Resources.Load("HiveBorder") as GameObject;
        backgroundPrefab = Resources.Load("HiveBackgroundMakeup") as GameObject;
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
    public int Columns
    {
        get
        {
            return m_colums;
        }
        set
        {
            m_colums = value;
        }
    }

    public int Rows
    {
        get
        {
            return m_rows;
        }
        set
        {
            m_rows = value;
        }
    }

    public float DelayLerp
    {
        get
        {
            return delayLerp;
        }
    }

    public float ExtraScale
    {
        get
        {
            return extraScale;
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

    public float DistanceBetweenHives
    {
        get
        {
            return distanceBetweenHives;
        }
        set
        {
            distanceBetweenHives = value;
        }
    }

    public GameManager GameManager
    {
        get
        {
            return _GM;
        }
    }

    public GameObject RowPrefab
    {
        get
        {
            return rowPrefab;
        }
    }

    public GameObject BorderPrefab
    {
        get
        {
            return borderPrefab;
        }
    }

        public GameObject BackgroundPrefab
    {
        get
        {
            return backgroundPrefab;
        }
    }

    public GameObject HivePrefab
    {
        get
        {
            return hivePrefab;
        }
    }

    public Vector3[,] HivePositions
    {
        get
        {
            return m_HivePositions;
        }
        set
        {
            m_HivePositions = value;
        }
    }

    public Hive[,] Grid
    {
        get
        {
            return m_Grid;
        }
        set
        {
            m_Grid = value;
        }
    }

    public GridLevels GridLevels
    {
        get
        {
            return m_GridLevels;
        }
    }

    public float YRowOffset
    {
        get
        {
            return yRowOffset;
        }
        set
        {
            yRowOffset = value;
        }
    }

    public float XHiveOffset
    {
        get
        {
            return xHiveOffset;
        }
        set
        {
            xHiveOffset = value;
        }
    }

    public float RefHeight
    {
        get
        {
            return m_refHeight;
        }
    }

    public float RefWidth
    {
        get
        {
            return m_refWidth;
        }
    }

    public float YHiveOffset
    {
        get
        {
            return yHiveOffset;
        }
        set
        {
            yHiveOffset = value;
        }
    }

    public float LerpSpeed
    {
        get
        {
            return lerpSpeed;
        }
    }

}
