using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderLevels : MonoBehaviour {

  public LineRenderer lineRenderer;
  public Transform containerLevels;
  public float lineDrawSpeed = 6f;

  private void Start()
  {
    lineRenderer.sortingOrder = 1;
    lineRenderer.sortingLayerName = "LineRender";
    lineRenderer.positionCount = containerLevels.childCount;

    for (int i = 0; i < containerLevels.childCount; i++)
    {
      lineRenderer.SetPosition(i, containerLevels.GetChild(i).transform.position);
    }
  }
}
