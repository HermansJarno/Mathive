using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineRenderController : MonoBehaviour
{

	public LineRenderer lineRenderer;
	public float lineDrawSpeed = 6f;

	public Color colorRed = Color.red;
	public Color colorGreen = Color.green;
	public Color colorBlue = Color.blue;
	public Color colorYellow = Color.yellow;
	public Color colorCyan = Color.cyan;
	public Color colorMagenta = Color.magenta;

	private void Start()
	{
		lineRenderer.sortingOrder = 4;
		lineRenderer.sortingLayerName = "UI";
	}

	public void UpdatePoints(List<Vector3> points, Vector3 touchPoint, HiveType hiveType)
	{
		switch (hiveType)
		{
			case HiveType.red:
				lineRenderer.startColor = colorRed;
				lineRenderer.endColor = colorRed;
				break;
			case HiveType.green:
				lineRenderer.startColor = colorGreen;
				lineRenderer.endColor = colorGreen;
				break;
			case HiveType.blue:
				lineRenderer.startColor = colorBlue;
				lineRenderer.endColor = colorBlue;
				break;
			case HiveType.yellow:
				lineRenderer.startColor = colorYellow;
				lineRenderer.endColor = colorYellow;
				break;
			case HiveType.cyan:
				lineRenderer.startColor = colorCyan;
				lineRenderer.endColor = colorCyan;
				break;
			case HiveType.magenta:
				lineRenderer.startColor = colorMagenta;
				lineRenderer.endColor = colorMagenta;
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
		}
		else
		{
			lineRenderer.positionCount = 0;
		}
	}
}
