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
    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < length; j++)
      {
        Vector3 TilePos;
        if (i % 2 != 0)
        {
          TilePos = new Vector3(widthTile + (i * widthTile), heightTile + (j * heightTile), 0);
        }
        else
        {
          TilePos = new Vector3(widthTile + (i * widthTile), heightTile + (j * heightTile), 0);
        }
        GameObject iTile = Instantiate(prefabTile, TilePos, prefabTile.transform.rotation);
        iTile.transform.SetParent(canvas.transform, false);
        Tiles[i, j] = new Tile(iTile, 6);
      }
    }
  }
	// Use this for initialization
	void Start () {
    canvas = GameObject.Find("Canvas");
    RectTransform rt = (RectTransform)prefabTile.transform;
    widthTile = prefabTile.GetComponent<Image>().sprite.bounds.size.x;
    heightTile = prefabTile.GetComponent<Image>().sprite.bounds.size.y;
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
