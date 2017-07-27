using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchController : MonoBehaviour {

  public Sprite normalSprite;
  public Sprite selectedSprite;

  //Change me to change the touch phase used.
  TouchPhase touchPhase = TouchPhase.Moved;
  public GraphicRaycaster myGRaycaster;

  List<GameObject> selectedHives = new List<GameObject>();
  List<GameObject> currentHives = new List<GameObject>();

  void Update()
  {
    //We check if we have more than one touch happening.
    //We also check if the first touches phase is Ended (that the finger was lifted)
    if (Input.touchCount > 0 && (Input.GetTouch(0).phase == touchPhase || Input.GetTouch(0).phase == TouchPhase.Began))
    {
      List<RaycastResult> rayResults = new List<RaycastResult>();
      PointerEventData ped = new PointerEventData(null);
      ped.position = Input.GetTouch(0).position;
      myGRaycaster.Raycast(ped, rayResults);
      foreach (RaycastResult result in rayResults)
      {
        GameObject resultObject = result.gameObject;
        if (resultObject.tag == "Hive")
        {
          if (!selectedHives.Contains(resultObject))
          {
            resultObject.GetComponent<Image>().sprite = selectedSprite;
            selectedHives.Add(resultObject);
          }
          else
          {
            resultObject.GetComponent<Image>().sprite = normalSprite;
            selectedHives.Remove(resultObject);
          }
        }
      }
    }
    if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
    {
      foreach (GameObject Hive in GameObject.FindGameObjectsWithTag("Hive"))
      {
        Hive.GetComponent<Image>().sprite = normalSprite;
      }
    }
  }
}
