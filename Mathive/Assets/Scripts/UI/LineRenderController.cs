using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineRenderController : MonoBehaviour {

  public LineRenderer lineRenderer;
  public float lineDrawSpeed = 6f;

  public Color c1 = Color.red;
  public Color c2 = Color.green;
  public Color c3 = Color.blue;
  public Color c4 = Color.yellow;
  public Color c5 = Color.cyan;
  public Color c6 = Color.magenta;

  private void Start()
  {
    lineRenderer.sortingOrder = 4;
    lineRenderer.sortingLayerName = "UI";
  }

  public void UpdatePoints(List<Vector3> points, Vector3 touchPoint, int value)
  {
    switch (value)
    {
      case 1:
        lineRenderer.startColor = c1;
        lineRenderer.endColor = c1;
        break;
      case 2:
        lineRenderer.startColor = c2;
        lineRenderer.endColor = c2;
        break;
      case 3:
        lineRenderer.startColor = c3;
        lineRenderer.endColor = c3;
        break;
      case 4:
        lineRenderer.startColor = c4;
        lineRenderer.endColor = c4;
        break;
      case 5:
        lineRenderer.startColor = c5;
        lineRenderer.endColor = c5;
        break;
      case 6:
        lineRenderer.startColor = c6;
        lineRenderer.endColor = c6;
        break;
      default:
        break;
    }

    if (points.Count > 1)
    {
      lineRenderer.positionCount = points.Count;
      if (touchPoint != Vector3.zero)
      {
        touchPoint = Camera.main.ScreenToWorldPoint(touchPoint);
        lineRenderer.positionCount++;
      }
      for (int i = 0; i < points.Count; i++)
      {
        lineRenderer.SetPosition(i, points[i]);
      }
      if (touchPoint != Vector3.zero)
      {
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, touchPoint);
      }
    }
    else if (points.Count > 0 && touchPoint != Vector3.zero)
    {
      touchPoint = Camera.main.ScreenToWorldPoint(touchPoint);
      lineRenderer.positionCount = points.Count + 1;
      for (int i = 0; i < points.Count; i++)
      {
        lineRenderer.SetPosition(i, points[i]);
      }
      lineRenderer.SetPosition(lineRenderer.positionCount - 1, touchPoint);
      Debug.Log(touchPoint);
    }
    else
    {
      lineRenderer.positionCount = 0;
    }
  }
}
