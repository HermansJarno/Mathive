using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpawner : MonoBehaviour {

  GameObject canvas;
  Tile[,] Tiles;
  public GameObject prefabTile;
  float widthTile, heightTile;

  public void SpawnTiles(int width, int length)
  {
    Tiles = new Tile[width, length];
    for (int i = 1; i < width+1; i++)
    {
      for (int j = 1; j < length+1; j++)
      {
        Vector3 TilePos;
        if (i % 2 != 0)
        {
          TilePos = new Vector3(-(canvas.GetComponent<RectTransform>().rect.width/2) + ((widthTile/4)*i), (canvas.GetComponent<RectTransform>().rect.height / 2) - ((widthTile/4)*j), 0);
        }
        else
        {
          TilePos = new Vector3(-(canvas.GetComponent<RectTransform>().rect.width / 2) + ((widthTile / 4) * i), (canvas.GetComponent<RectTransform>().rect.height / 2) - ((widthTile / 4)*j), 0);
        }
        GameObject iTile = Instantiate(prefabTile, TilePos, prefabTile.transform.rotation);
        iTile.transform.SetParent(canvas.transform, false);
        Tiles[i-1, j-1] = new Tile(iTile, 6);
      }
    }
  }
	// Use this for initialization
	void Start () {
    canvas = GameObject.Find("Canvas");
    RectTransform rt = (RectTransform)prefabTile.transform;
    widthTile = Mathf.Abs(prefabTile.GetComponent<RectTransform>().rect.width);
    Debug.Log(widthTile);
    heightTile = Mathf.Abs(prefabTile.GetComponent<RectTransform>().rect.height);
    Debug.Log(heightTile);
    SpawnTiles(4, 5);

  }
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class Tile
{
  public GameObject tileObj;
  public int number;
  public Tile(GameObject tileObject, int num)
  {
    tileObj = tileObject;
    number = num;
    tileObj.transform.Find("Text").GetComponent<Text>().text = num.ToString();
  }
}
