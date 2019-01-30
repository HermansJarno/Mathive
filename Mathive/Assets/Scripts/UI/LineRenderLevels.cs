using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

  private void Start(){
      levelController = GameObject.Find("LevelData").GetComponent<LevelController>();
      currentLevel = levelController.level;
      if(currentLevel - 1 > 0){
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
        lineRenderer.positionCount = currentLevel;
        for (int i = 0; i < currentLevel; i++)
        {
            lineRenderer.SetPosition(i, containerLevels.GetChild(i).transform.position);
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
