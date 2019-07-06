using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LineRenderLevels : MonoBehaviour {

  public LineRenderer lineRenderer;
  public Transform containerLevels;
  public float lineDrawSpeed = 6f;

  private Vector3 p0, p1;
  private float distance;
  private float counter;

  private LevelController levelController;
  private int currentLevel;

  private bool drawSlow = false;

  private bool draw = false;

  List<Transform> levelsToDraw = new List<Transform>();

  private void Start(){
      levelController = GameObject.Find("LevelData").GetComponent<LevelController>();
      currentLevel = levelController.level;

      //GameObject starPrefab = Resources.Load("Star") as GameObject;

      for (int i = 1; i <= currentLevel; i++)
      {
        string levelName = "Level " + i.ToString();
        GameObject level = containerLevels.transform.Find(levelName).gameObject;
     
        level.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprites/blueSelected");
        /* 
        for (int j = 0; j < 3; j++)
        {
          GameObject spawnedPrefab = Instantiate(starPrefab, transform.position, transform.rotation) as GameObject;
          spawnedPrefab.transform.SetParent(level.transform.Find("StarContainer"), false);
        }*/
        levelsToDraw.Add(level.transform);
      }

      if(currentLevel > 1){
          draw = true;
      }


      /* 
      if(levelController.OldLevel < currentLevel){
          drawSlow = true;
          lineRenderer.SetPosition(0, p0);
          distance = Vector3.Distance(p0, p1);
      }
      */
  }

  private void Update()
  {
      if(draw){
        lineRenderer.sortingOrder = 1;
        lineRenderer.sortingLayerName = "LineRender";
        lineRenderer.positionCount = levelsToDraw.Count;
        for (int i = 0; i < currentLevel; i++)
        {
            lineRenderer.SetPosition(i, levelsToDraw[i].position);
        }
      }


      /* 
      if(drawSlow){
        if (counter < distance)
        {
        counter += .1f / lineDrawSpeed;
        float x = Mathf.Lerp(0, distance, counter);
        var point0 = p0;
        var point1 = p1;

        var pointALongLine= x * Vector3.Normalize(point1 - point0) + point0;

        lineRenderer.SetPosition(1, pointALongLine);
        }else{
            levelController.OldLevel = currentLevel;
        }
    }*/

  }
}
