using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderController : MonoBehaviour {

  public LineRenderer lineRenderer;
  public float lineDrawSpeed = 6f;

  private void Start()
  {
    lineRenderer.sortingOrder = 4;
    lineRenderer.sortingLayerName = "UI";
  }

  public void UpdatePoints(List<GameObject> hives)
  {
    if (hives.Count > 1)
    {
      lineRenderer.positionCount = hives.Count;
      for (int i = 0; i < lineRenderer.positionCount; i++)
      {
        lineRenderer.SetPosition(i, hives[i].transform.position);
      }
    }
    else
    {
      lineRenderer.positionCount = 0;
    }
  }
}
